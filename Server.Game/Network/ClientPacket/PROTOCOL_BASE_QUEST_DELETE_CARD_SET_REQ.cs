using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ : GameClientPacket
    {
        private uint Error;
        private int MissionIdx;
        public PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            MissionIdx = ReadC();
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
                PlayerMissions missions = Player.Mission;
                bool failed = false;
                if (MissionIdx >= 3 || MissionIdx == 0 && missions.Mission1 == 0 || MissionIdx == 1 && missions.Mission2 == 0 || MissionIdx == 2 && missions.Mission3 == 0)
                {
                    failed = true;
                }
                if (!failed && DaoManagerSQL.UpdatePlayerMissionId(Player.PlayerId, 0, MissionIdx) && ComDiv.UpdateDB("player_missions", "owner_id", Player.PlayerId, new string[] { $"card{(MissionIdx + 1)}", $"mission{(MissionIdx + 1)}_raw" }, 0, new byte[0]))
                {
                    if (MissionIdx == 0)
                    {
                        missions.Mission1 = 0;
                        missions.Card1 = 0;
                        missions.List1 = new byte[40];
                    }
                    else if (MissionIdx == 1)
                    {
                        missions.Mission2 = 0;
                        missions.Card2 = 0;
                        missions.List2 = new byte[40];
                    }
                    else if (MissionIdx == 2)
                    {
                        missions.Mission3 = 0;
                        missions.Card3 = 0;
                        missions.List3 = new byte[40];
                    }
                }
                else
                {
                    Error = 0x80001050;
                }
                Client.SendPacket(new PROTOCOL_BASE_QUEST_DELETE_CARD_SET_ACK(Error, Player));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_QUEST_DELETE_CARD_SET_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}