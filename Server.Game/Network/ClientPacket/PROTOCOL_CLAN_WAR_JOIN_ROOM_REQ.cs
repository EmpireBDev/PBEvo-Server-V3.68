using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ : GameClientPacket
    {
        private int match, channel;
        private TeamEnum TeamIdx;
        public PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            match = ReadD();
            TeamIdx = (TeamEnum)ReadH();
            channel = ReadH();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.ClanId == 0 || Player.Match == null)
                {
                    return;
                }
                if (Player != null && Player.Nickname.Length > 0 && Player.Room == null && Player.GetChannel(out ChannelModel Channel))
                {
                    RoomModel Room = Channel.GetRoom(match);
                    if (Room != null && Room.GetLeader() != null)
                    {
                        if (Room.Password.Length > 0 && !Player.HaveGMLevel())
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(2147487749, null));
                        }
                        else if (Room.Limit == 1 && Room.State >= RoomState.CountDown)
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(2147487763, null)); //Entrada proibida com partida em andamento
                        }
                        else if (Room.KickedPlayers.Contains(Player.PlayerId))
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(2147487756, null)); //Você foi expulso dessa sala.
                        }
                        else if (Room.AddPlayer(Player, TeamIdx) >= 0)
                        {
                            using (PROTOCOL_ROOM_GET_SLOTONEINFO_ACK packet = new PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(Player))
                            {
                                Room.SendPacketToPlayers(packet, Player.PlayerId);
                            }
                            Room.UpdateSlotsInfo();
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(0, Player));
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(2147487747, null));
                        }
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(2147487748, null));
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_ROOM_JOIN_ACK(2147487748, null));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CLAN_WAR_JOIN_ROOM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}