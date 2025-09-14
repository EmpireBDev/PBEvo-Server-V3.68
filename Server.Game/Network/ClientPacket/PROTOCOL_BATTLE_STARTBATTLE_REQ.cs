using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_STARTBATTLE_REQ : GameClientPacket
    {
        public PROTOCOL_BATTLE_STARTBATTLE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                RoomModel Room = Player.Room;
                if (Room != null && Room.IsPreparing())
                {
                    bool IsBotMode = Room.IsBotMode();
                    SlotModel Slot = Room.GetSlot(Player.SlotId);
                    if (Slot == null)
                    {
                        return;
                    }
                    if (Slot.State == SlotState.PRESTART)
                    {
                        Room.ChangeSlotState(Slot, SlotState.BATTLE_READY, true);
                        Slot.StopTiming();
                        if (IsBotMode)
                        {
                            Client.SendPacket(new PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK(Room));
                        }
                        Client.SendPacket(new PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(Room, IsBotMode));
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum.BATTLE_FIRST_HOLE));
                        Client.SendPacket(new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Player, 0));
                        Room.ChangeSlotState(Slot, SlotState.NORMAL, true);
                        AllUtils.BattleEndPlayersCount(Room, IsBotMode);
                        return;
                    }
                    int TeamCT = 0, TeamFR = 0, TotalTeam = 0, FR = 0, CT = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        SlotModel PlayerSlot = Room.Slots[i];
                        if (PlayerSlot.State >= SlotState.LOAD)
                        {
                            TotalTeam++;
                            if (PlayerSlot.Team == 0)
                            {
                                FR++;
                            }
                            else
                            {
                                CT++;
                            }
                            if (PlayerSlot.State >= SlotState.BATTLE_READY)
                            {
                                if (PlayerSlot.Team == 0)
                                {
                                    TeamFR++;
                                }
                                else
                                {
                                    TeamCT++;
                                }
                            }
                        }
                    }
                    if (Room.State == RoomState.Battle || Room.Slots[Room.Leader].State >= SlotState.BATTLE_READY && IsBotMode && (Room.Leader % 2 == 0 && TeamFR > FR / 2 || Room.Leader % 2 == 1 && TeamCT > CT / 2) || Room.Slots[Room.Leader].State >= SlotState.BATTLE_READY && ((!ConfigLoader.IsTestMode || ConfigLoader.UdpType != UdpState.RELAY) && TeamCT > CT / 2 && TeamFR > FR / 2 || ConfigLoader.IsTestMode && ConfigLoader.UdpType == UdpState.RELAY))
                    {
                        Room.SpawnReadyPlayers(IsBotMode);
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum.BATTLE_FIRST_HOLE));
                    Client.SendPacket(new PROTOCOL_BATTLE_STARTBATTLE_ACK());
                    if (Room != null)
                    {
                        Room.ChangeSlotState(Player.SlotId, SlotState.NORMAL, true);
                    }
                    if (Room == null && Player != null)
                    {
                        Client.SendPacket(new PROTOCOL_LOBBY_ENTER_ACK(0));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_STARTBATTLE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}