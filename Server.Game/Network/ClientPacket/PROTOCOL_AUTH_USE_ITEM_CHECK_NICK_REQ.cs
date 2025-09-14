using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SQL;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_REQ : GameClientPacket
    {
        private string name;
        public PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            name = ReadU(66);
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_AUTH_USE_ITEM_CHECK_NICK_ACK(!DaoManagerSQL.IsPlayerNameExist(name) ? 0 : 2147483923));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}