using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_ROOM_INVITED_REQ : GameClientPacket
    {
        private long pId;
        public PROTOCOL_CS_ROOM_INVITED_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            pId = ReadQ();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.ClanId == 0)
                {
                    return;
                }
                Account member = AccountManager.GetAccount(pId, 31);
                if (member != null && member.ClanId == Player.ClanId)
                {
                    member.SendPacket(new PROTOCOL_CS_ROOM_INVITED_RESULT_ACK(Client.PlayerId), false);
                }
                Player.SendPacket(new PROTOCOL_CS_ROOM_INVITED_ACK(0));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
