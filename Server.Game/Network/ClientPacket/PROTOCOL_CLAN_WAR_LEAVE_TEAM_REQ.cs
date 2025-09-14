using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CLAN_WAR_LEAVE_TEAM_REQ : GameClientPacket
    {
        private uint erro;
        public PROTOCOL_CLAN_WAR_LEAVE_TEAM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
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
                if (Match == null || !Match.RemovePlayer(Player))
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_CLAN_WAR_LEAVE_TEAM_ACK(erro));
                if (erro == 0)
                {
                    Player.Status.UpdateClanMatch(255);
                    AllUtils.SyncPlayerToClanMembers(Player);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}