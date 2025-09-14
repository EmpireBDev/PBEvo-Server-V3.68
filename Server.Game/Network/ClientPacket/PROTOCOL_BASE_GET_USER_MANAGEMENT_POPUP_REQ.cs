using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_GET_USER_MANAGEMENT_POPUP_REQ : GameClientPacket
    {
        private string Name;
        public PROTOCOL_BASE_GET_USER_MANAGEMENT_POPUP_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Name = ReadU(33);
        }
        public override void Run()
        {
            try
            {
                Account Player =Client.Player;
                if (Player == null || Player.Nickname.Length == 0 || Player.Nickname == Name)
                {
                    return;
                }
                Client.SendPacket(new PROTOCOL_BASE_GET_USER_MANAGEMENT_POPUP_ACK());

            }
            catch (Exception ex)
            {
                CLogger.Print(ex.Message, LoggerType.Error, ex);
            }
        }
    }
}
