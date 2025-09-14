using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_INVITE_SELECTED_LOBBY_USER_REQ : GameClientPacket
    {
        private int count;
        private uint erro;
        private int session;
        public PROTOCOL_ROOM_INVITE_SELECTED_LOBBY_USER_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            count = ReadD();
            session = ReadD();
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
                ChannelModel Channel = Player.GetChannel();
                if (Channel != null)
                {
                    using (PROTOCOL_SERVER_MESSAGE_INVITED_ACK Packet = new PROTOCOL_SERVER_MESSAGE_INVITED_ACK(Player, Player.Room))
                    {
                        byte[] Data = Packet.GetCompleteBytes("PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_REQ");
                        for (int i = 0; i < count; i++)
                        {
                            Account Member = AccountManager.GetAccount(Channel.GetPlayer(session).PlayerId, true);
                            if (Member != null)
                            {
                                Member.SendCompletePacket(Data, Packet.GetType().Name);
                            }
                        }
                    }
                }
                else
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_ROOM_INVITE_SELECTED_LOBBY_USER_ACK(erro));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}