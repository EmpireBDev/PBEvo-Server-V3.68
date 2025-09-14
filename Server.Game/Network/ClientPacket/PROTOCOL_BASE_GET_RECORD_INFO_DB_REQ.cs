using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_RECORD_INFO_DB_REQ : GameClientPacket
    {
        private long PlayerId;
        public PROTOCOL_BASE_GET_RECORD_INFO_DB_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
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
                if (TargetUser != null && Player.PlayerId != TargetUser.PlayerId)
                {
                    Client.SendPacket(new PROTOCOL_BASE_GET_RECORD_INFO_DB_ACK(TargetUser));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}