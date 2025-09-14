using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_RECV_WHISPER_REQ : GameClientPacket
    {
        private string receiverName, text;
        public PROTOCOL_AUTH_RECV_WHISPER_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            receiverName = ReadU(66);
            text = ReadU(ReadH() * 2);
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.Nickname == receiverName)
                {
                    return;
                }
                Account TargetUser = AccountManager.GetAccount(receiverName, 1, 0);
                if (TargetUser == null || TargetUser.Nickname != receiverName || !TargetUser.IsOnline)
                {
                    Client.SendPacket(new PROTOCOL_AUTH_SEND_WHISPER_ACK(receiverName, text, 0x80000000));
                }
                else
                {
                    TargetUser.SendPacket(new PROTOCOL_AUTH_RECV_WHISPER_ACK(Player.Nickname, text, Player.UseChatGM()), false);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}