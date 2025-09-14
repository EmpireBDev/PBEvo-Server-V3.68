using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_GMCHAT_APPLY_PENALTY_REQ : GameClientPacket
    {
        private long PlayerId;
        private int BanTime;
        private byte Type;
        public PROTOCOL_GMCHAT_APPLY_PENALTY_REQ(GameClient client, byte[] buff)
        {
            Makeme(client, buff);
        }
        public override void Read()
        {
            BanTime = ReadD();
            Type = ReadC();
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
                    Client.SendPacket(new PROTOCOL_GMCHAT_APPLY_PENALTY_ACK(0));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_GMCHAT_APPLY_PENALTY_ACK(0x80000000));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_GMCHAT_APPLY_PENALTY_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
