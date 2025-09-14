using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_REQ : GameClientPacket
    {
        public PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_REQ(GameClient client, byte[] data)
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
                if (Room != null)
                {
                    Room.State = RoomState.BattleEnd;
                    Client.SendPacket(new PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_ACK(Room));
                    Client.SendPacket(new PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(Client.Player.Room, 0, RoundEndType.Tutorial));
                    Client.SendPacket(new PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(Room));
                    if (Room.State == RoomState.BattleEnd)
                    {
                        Room.State = RoomState.Ready;
                        Client.SendPacket(new PROTOCOL_BATTLE_ENDBATTLE_ACK(Client.Player));
                        Client.SendPacket(new PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(Room));
                    }
                    AllUtils.ResetBattleInfo(Room);
                    Client.SendPacket(new PROTOCOL_ROOM_GET_SLOTINFO_ACK(Room));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}