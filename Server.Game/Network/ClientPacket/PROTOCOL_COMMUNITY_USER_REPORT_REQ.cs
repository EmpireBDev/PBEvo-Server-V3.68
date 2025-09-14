using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_COMMUNITY_USER_REPORT_REQ : GameClientPacket
    {
        private uint Answer = 0;
        private int ReportType;
        private string Nickname;
        private string Message;
        public PROTOCOL_COMMUNITY_USER_REPORT_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            Message = ReadU(ReadC() * 2);
            ReportType = ReadC();
            Nickname = ReadU(ReadC() * 2);
        }
        public override void Run()
        {
            bool Limit = false; //reports limit
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                Account Reporter = AccountManager.GetAccount(Nickname, 1, 31);
                if (Reporter != null && (Player.Nickname.Length > 0 && Player.Nickname != Nickname))
                {
                    if (Player.Rank < 5)
                    {
                        Answer = 0x800010E9; //rank limit
                    }
                    else if (Limit == true)
                    {
                        Answer = 0x800010E8; //reports limit
                    }
                }
                Client.SendPacket(new PROTOCOL_COMMUNITY_USER_REPORT_ACK(Answer));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}