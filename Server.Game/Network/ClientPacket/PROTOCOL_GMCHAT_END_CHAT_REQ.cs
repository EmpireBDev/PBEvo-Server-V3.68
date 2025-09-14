using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_GMCHAT_END_CHAT_REQ : GameClientPacket
    {
        private long PlayerId;
        public PROTOCOL_GMCHAT_END_CHAT_REQ(GameClient client, byte[] buff)
        {
            Makeme(client, buff);
        }
        public override void Read()
        {
            PlayerId = ReadQ();
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
                Account TargetUser = AccountManager.GetAccount(PlayerId, 31);
                if (TargetUser != null)
                {
                    Client.SendPacket(new PROTOCOL_GMCHAT_END_CHAT_ACK(0, TargetUser));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_GMCHAT_END_CHAT_ACK(0x80000000, null));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_GMCHAT_START_CHAT_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
