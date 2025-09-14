using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_QUEST_BUY_CARD_SET_REQ : GameClientPacket
    {
        private int MissionId;
        private uint Error;
        public PROTOCOL_BASE_QUEST_BUY_CARD_SET_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            MissionId = ReadC();
        }
        public override void Run()
        {
            Account Player = Client.Player;
            if (Player == null)
            {
                return;
            }
            PlayerMissions missions = Player.Mission;
            int price = MissionConfigXML.GetMissionPrice(MissionId);
            if (Player == null || price == -1 || 0 > Player.Gold - price || missions.Mission1 == MissionId || missions.Mission2 == MissionId || missions.Mission3 == MissionId)
            {
                Error = price == -1 ? 0x8000104C : 0x8000104D;
            }
            else
            {
                if (missions.Mission1 == 0)
                {
                    if (DaoManagerSQL.UpdatePlayerMissionId(Player.PlayerId, MissionId, 0))
                    {
                        missions.Mission1 = MissionId;
                        missions.List1 = new byte[40];
                        missions.ActualMission = 0;
                        missions.Card1 = 0;
                    }
                    else
                    {
                        Error = 0x8000104C;
                    }
                }
                else if (missions.Mission2 == 0)
                {
                    if (DaoManagerSQL.UpdatePlayerMissionId(Player.PlayerId, MissionId, 1))
                    {
                        missions.Mission2 = MissionId;
                        missions.List2 = new byte[40];
                        missions.ActualMission = 1;
                        missions.Card2 = 0;
                    }
                    else
                    {
                        Error = 0x8000104C;
                    }
                }
                else if (missions.Mission3 == 0)
                {
                    if (DaoManagerSQL.UpdatePlayerMissionId(Player.PlayerId, MissionId, 2))
                    {
                        missions.Mission3 = MissionId;
                        missions.List3 = new byte[40];
                        missions.ActualMission = 2;
                        missions.Card3 = 0;
                    }
                    else
                    {
                        Error = 0x8000104C;
                    }
                }
                else
                {
                    Error = 0x8000104E;
                }
                if (Error == 0)
                {
                    if (price == 0 || DaoManagerSQL.UpdateAccountGold(Player.PlayerId, Player.Gold - price))
                    {
                        Player.Gold -= price;
                    }
                    else
                    {
                        Error = 0x8000104C;
                    }
                }
            }
            Client.SendPacket(new PROTOCOL_BASE_QUEST_BUY_CARD_SET_ACK(Error, Player));
            /*
             * 0x8000104C STR_TBL_GUI_BASE_FAIL_BUY_CARD_BY_NO_CARD_INFO
             * 0x8000104D STR_TBL_GUI_BASE_NO_POINT_TO_GET_ITEM
             * 0x8000104E STR_TBL_GUI_BASE_LIMIT_CARD_COUNT
             * 0x800010D5 STR_TBL_GUI_BASE_DID_NOT_TUTORIAL_MISSION_CARD
             */
        }
    }
}