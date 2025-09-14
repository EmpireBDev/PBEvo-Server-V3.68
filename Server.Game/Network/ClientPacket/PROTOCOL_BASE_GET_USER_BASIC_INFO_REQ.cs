using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_USER_BASIC_INFO_REQ : GameClientPacket
    {
        private uint Error;
        private long PlayerId;
        public PROTOCOL_BASE_GET_USER_BASIC_INFO_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
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
                Account UserT = AccountManager.GetAccountDB(PlayerId, 2, 31);
                if (UserT == null || !(Player.Nickname.Length > 0 && Player.PlayerId != PlayerId))
                {
                    Error = 0x80001803;
                }
                Client.SendPacket(new PROTOCOL_BASE_GET_USER_BASIC_INFO_ACK(Error, UserT));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
