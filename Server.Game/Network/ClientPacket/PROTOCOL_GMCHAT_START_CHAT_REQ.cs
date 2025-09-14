using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_GMCHAT_START_CHAT_REQ : GameClientPacket
    {
        private string Name;
        private int SessionId;
        public PROTOCOL_GMCHAT_START_CHAT_REQ(GameClient client, byte[] buff)
        {
            Makeme(client, buff);
        }
        public override void Read()
        {
            Name = ReadU(ReadC() * 2);
            SessionId = ReadD();
            ReadC(); //? always 1?
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
                PlayerSession Session = Player.GetChannel().GetPlayer(SessionId);
                if (Session != null && Player.Session != Session)
                {
                    Account TargetUser = AccountManager.GetAccount(Name, 1, 31);
                    if (TargetUser != null && TargetUser.Session == Session)
                    {
                        Client.SendPacket(new PROTOCOL_GMCHAT_START_CHAT_ACK(0, TargetUser));
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_GMCHAT_START_CHAT_ACK(0x80000000, null));
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_GMCHAT_START_CHAT_ACK(0x80000000, null));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_GMCHAT_START_CHAT_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
