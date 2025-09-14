using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ : GameClientPacket
    {
        private int CardIdx, ActualMission, CardFlags;
        public PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ActualMission = ReadC();
            CardIdx = ReadC();
            CardFlags = ReadUH();
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
                DBQuery query = new DBQuery();
                PlayerMissions missions = Player.Mission;
                if (missions.GetCard(ActualMission) != CardIdx)
                {
                    if (ActualMission == 0)
                    {
                        missions.Card1 = CardIdx;
                    }
                    else if (ActualMission == 1)
                    {
                        missions.Card2 = CardIdx;
                    }
                    else if (ActualMission == 2)
                    {
                        missions.Card3 = CardIdx;
                    }
                    else if (ActualMission == 3)
                    {
                        missions.Card4 = CardIdx;
                    }
                    query.AddQuery($"card{(ActualMission + 1)}", CardIdx);
                }
                missions.SelectedCard = CardFlags == 65535;
                if (missions.ActualMission != ActualMission)
                {
                    query.AddQuery("current_mission", ActualMission);
                    missions.ActualMission = ActualMission;
                }
                ComDiv.UpdateDB("player_missions", "owner_id", Client.PlayerId, query.GetTables(), query.GetValues());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}