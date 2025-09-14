using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CHECK_DUPLICATE_REQ : GameClientPacket
    {
        private string clanName;
        public PROTOCOL_CS_CHECK_DUPLICATE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            clanName = ReadU(ReadC() * 2);
        }
        public override void Run()
        {
            try
            {
                Client.SendPacket(new PROTOCOL_CS_CHECK_DUPLICATE_ACK(!ClanManager.IsClanNameExist(clanName) ? 0 : 0x80000000));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}