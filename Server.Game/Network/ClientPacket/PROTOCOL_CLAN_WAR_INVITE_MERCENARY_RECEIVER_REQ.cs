using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_REQ : GameClientPacket
    {
        private int formacao;
        public PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            formacao = ReadC();
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
                MatchModel Match = Player.Match;
                if (Match != null && Player.MatchSlot == Match.Leader)
                {
                    Match.Training = formacao;
                    using (PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK packet = new PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK(0, formacao))
                    {
                        Match.SendPacketToPlayers(packet);
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_CLAN_WAR_INVITE_MERCENARY_RECEIVER_ACK(0x80000000));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}