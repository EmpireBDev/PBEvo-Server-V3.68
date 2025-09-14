using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_READYBATTLE_REQ : GameClientPacket
    {
        private int Error;
        public PROTOCOL_BATTLE_READYBATTLE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ReadC();
            Error = ReadD(); // 0 - NORMAL || 1 - OBSERVER (GM)
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
                if (Room == null || Room.GetLeader() == null || !Room.GetChannel(out ChannelModel Channel) || !Room.GetSlot(Player.SlotId, out SlotModel Slot))
                {
                    return;
                }
                if (Slot.Equipment == null)
                { 
                    Client.SendPacket(new PROTOCOL_BATTLE_READYBATTLE_ACK(0x800010AB)); //STBL_IDX_EP_ROOM_ERROR_READY_WEAPON_EQUIP
                    return;
                }
                bool IsBotMode = Room.IsBotMode();
                Slot.SpecGM = (Error == 1 && Player.IsGM() || Room.RoomType == RoomCondition.Ace && !(Slot.Id >= 0 && Slot.Id <= 1));
                if (Room.Leader == Player.SlotId)
                {
                    if (Room.State != RoomState.Ready && Room.State != RoomState.CountDown)
                    {
                        return;
                    }
                    int TotalEnemys = 0, FRPlayers = 0, CTPlayers = 0;
                    GetReadyPlayers(Room, ref FRPlayers, ref CTPlayers, ref TotalEnemys);
                    MapMatch Match = SystemMapXML.GetMapLimit((int)Room.MapId, (int)Room.Rule);
                    if (Match != null && Match.Limit == 8 && (FRPlayers >= 4 || CTPlayers >= 4) && Channel.Type != ChannelType.Clan)
                    {
                        Client.SendPacket(new PROTOCOL_ROOM_UNREADY_4VS4_ACK());
                        return;
                    }
                    if (ClanMatchCheck(Room, Channel.Type, TotalEnemys))
                    {
                        return;
                    }
                    if (IsBotMode || Room.RoomType == RoomCondition.Tutorial || TotalEnemys > 0 && !IsBotMode)
                    {
                        Room.ChangeSlotState(Slot, SlotState.READY, false);
                        if (Room.State != RoomState.CountDown)
                        {
                            TryBalanceTeams(Room, IsBotMode);
                        }
                        if (Room.ThisModeHaveCD())
                        {
                            if (Room.State == RoomState.Ready)
                            {
                                SlotModel[] Slots = new SlotModel[2] { Room.GetSlot(0), Room.GetSlot(1) };
                                if (Room.RoomType == RoomCondition.Ace && !(Slots[0].State == SlotState.READY && Slots[1].State == SlotState.READY))
                                {
                                    Client.SendPacket(new PROTOCOL_BATTLE_READYBATTLE_ACK(0x80001009));
                                    Room.ChangeSlotState(Room.Leader, SlotState.NORMAL, false);
                                    Room.StopCountDown(CountDownEnum.StopByHost);
                                }
                                else
                                {
                                    Room.State = RoomState.CountDown;
                                    Room.UpdateRoomInfo();
                                    Room.StartCountDown();
                                }
                            }
                            else if (Room.State == RoomState.CountDown)
                            {
                                Room.ChangeSlotState(Room.Leader, SlotState.NORMAL, false);
                                Room.StopCountDown(CountDownEnum.StopByHost);
                            }
                        }
                        else
                        {
                            Room.StartBattle(false);
                        }
                        Room.UpdateSlotsInfo();
                    }
                    else if (TotalEnemys == 0 && !IsBotMode)
                    {
                        Client.SendPacket(new PROTOCOL_BATTLE_READYBATTLE_ACK(0x80001009));
                    }
                }
                else if (Room.Slots[Room.Leader].State >= SlotState.LOAD)
                {
                    if (Slot.State == SlotState.NORMAL)
                    {
                        if (Room.BalanceType == 1 && !IsBotMode)
                        {
                            AllUtils.TryBalancePlayer(Room, Player, true, ref Slot);
                        }
                        Room.ChangeSlotState(Slot, SlotState.LOAD, true);
                        Slot.SetMissionsClone(Player.Mission);
                        Client.SendPacket(new PROTOCOL_BATTLE_READYBATTLE_ACK((uint)Slot.State));
                        Client.SendPacket(new PROTOCOL_BATTLE_START_GAME_ACK(Room));
                        using (PROTOCOL_BATTLE_START_GAME_TRANS_ACK packet = new PROTOCOL_BATTLE_START_GAME_TRANS_ACK(Room, Slot, Player.Title))
                        {
                            Room.SendPacketToPlayers(packet, SlotState.READY, 1, Slot.Id);
                        }
                    }
                }
                else if (Slot.State == SlotState.NORMAL)
                {
                    Room.ChangeSlotState(Slot, SlotState.READY, true);
                    if (Room.State == RoomState.CountDown)
                    {
                        TryBalanceTeams(Room, IsBotMode);
                    }
                }
                else if (Slot.State == SlotState.READY)
                {
                    Room.ChangeSlotState(Slot, SlotState.NORMAL, false);
                    if (Room.State == RoomState.CountDown && Room.GetPlayingPlayers((TeamEnum)(Room.Leader % 2 == 0 ? 1 : 0), SlotState.READY, 0) == 0)
                    {
                        Room.ChangeSlotState(Room.Leader, SlotState.NORMAL, false);
                        Room.StopCountDown(CountDownEnum.StopByPlayer);
                    }
                    Room.UpdateSlotsInfo();
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_READYBATTLE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
        private void GetReadyPlayers(RoomModel Room, ref int FRPlayers, ref int CTPlayers, ref int TotalEnemys)
        {
            foreach (SlotModel Slot in Room.Slots)
            {
                if (Slot.State == SlotState.READY)
                {
                    if (Slot.Team == 0)
                    {
                        FRPlayers++;
                    }
                    else
                    {
                        CTPlayers++;
                    }
                }
            }
            if (Room.Leader % 2 == 0)
            {
                TotalEnemys = CTPlayers;
            }
            else
            {
                TotalEnemys = FRPlayers;
            }
        }
        private bool ClanMatchCheck(RoomModel Room, ChannelType Type, int TotalEnemys)
        {
            if (ConfigLoader.IsTestMode || Type != ChannelType.Clan)
            {
                return false;
            }
            if (!AllUtils.Have2ClansToClanMatch(Room))
            {
                Client.SendPacket(new PROTOCOL_BATTLE_READYBATTLE_ACK(0x80001071)); //STBL_IDX_EP_ROOM_NO_START_FOR_NO_CLAN_TEAM
                return true;
            }
            if (TotalEnemys > 0 && !AllUtils.HavePlayersToClanMatch(Room))
            {
                Client.SendPacket(new PROTOCOL_BATTLE_READYBATTLE_ACK(0x80001072)); //STBL_IDX_EP_ROOM_NO_START_FOR_TEAM_NOTFULL
                return true;
            }
            return false;
        }
        private void TryBalanceTeams(RoomModel Room, bool IsBotMode)
        {
            if (Room.BalanceType != 1 || IsBotMode)
            {
                return;
            }
            TeamEnum TeamIdx = AllUtils.GetBalanceTeamIdx(Room, false, TeamEnum.ALL_TEAM);
            if (TeamIdx == TeamEnum.ALL_TEAM)
            {
                return;
            }
            int[] TeamArray = TeamIdx == TeamEnum.CT_TEAM ? Room.FR_TEAM : Room.CT_TEAM;
            SlotModel LastSlot = null;
            for (int i = TeamArray.Length - 1; i >= 0; i--)
            {
                SlotModel Slot = Room.Slots[TeamArray[i]];
                if (Slot.State == SlotState.READY && Room.Leader != Slot.Id)
                {
                    LastSlot = Slot;
                    break;
                }
            }
            if (LastSlot != null && Room.GetPlayerBySlot(LastSlot, out Account Player))
            {
                AllUtils.TryBalancePlayer(Room, Player, false, ref LastSlot);
            }
        }
    }
}