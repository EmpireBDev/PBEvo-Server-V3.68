using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_JOIN_REQ : GameClientPacket
    {
        private int roomId, type;
        private string password;
        public PROTOCOL_ROOM_JOIN_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            roomId = ReadD();
            password = ReadS(4);
            type = ReadC();
            ReadC();
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
                if (Player.Nickname.Length > 0 && Player.Room == null && Player.Match == null && Player.GetChannel(out ChannelModel Channel))
                {
                    RoomModel Room = Channel.GetRoom(roomId);
                    if (Room != null && Room.GetLeader() != null)
                    {
                        if (Room.RoomType == RoomCondition.Tutorial)
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0x8000107C, null));
                        }
                        else if (Room.Password.Length > 0 && password != Room.Password && Player.Rank != 53 && !Player.HaveGMLevel() && type != 1)
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0x80001005, null));
                        }
                        else if (Room.Limit == 1 && Room.State >= RoomState.CountDown && !Player.HaveGMLevel())
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0x80001013, null));
                        }
                        else if (Room.KickedPlayers.Contains(Player.PlayerId) && !Player.HaveGMLevel())
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0x8000100C, null));
                        }
                        else if (Room.AddPlayer(Player) >= 0)
                        {
                            Player.ResetPages();
                            using (PROTOCOL_ROOM_GET_SLOTONEINFO_ACK Packet = new PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(Player))
                            {
                                Room.SendPacketToPlayers(Packet, Player.PlayerId);
                            }
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0, Player));
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0x80001003, null)); //SLOTFULL
                        }
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0x80001004, null)); //INVALIDROOM
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0x80001004, null));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_JOIN_ROOM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}