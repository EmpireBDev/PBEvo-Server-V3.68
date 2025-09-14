using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_FIND_USER_REQ : GameClientPacket
    {
        private string name;
        private uint Error;
        public PROTOCOL_AUTH_FIND_USER_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            name = ReadU(33);
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
                Player.FindPlayer = name;
                Account UserT = AccountManager.GetAccount(Player.FindPlayer, 1, 31);
                if (UserT == null || !(Player.Nickname.Length > 0 && Player.Nickname != name))
                {
                    Error = 0x80001803;
                }
                Client.SendPacket(new PROTOCOL_AUTH_FIND_USER_ACK(Error, UserT));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}