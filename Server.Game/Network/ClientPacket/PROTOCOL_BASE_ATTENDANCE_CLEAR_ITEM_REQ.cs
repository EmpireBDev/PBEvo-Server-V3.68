using Plugin.Core;
using Plugin.Core.Managers;
using Plugin.Core.Managers.Events;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Network.ServerPacket;
using System;
using Plugin.Core.Models;
using Server.Game.Data.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ : GameClientPacket
    {
        private EventErrorEnum Error = EventErrorEnum.VISIT_EVENT_SUCCESS;
        private int EventId, Type, DayCheckedIdx;
        public PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            EventId = ReadD();
            Type = ReadC();
            DayCheckedIdx = ReadC();
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
                if (Player.Nickname.Length == 0 || Type > 1)
                {
                    Error = EventErrorEnum.VISIT_EVENT_USERFAIL;
                }
                else if (Player.Event != null)
                {
                    if (Player.Event.LastVisitSequence1 == Player.Event.LastVisitSequence2)
                    {
                        Error = EventErrorEnum.VISIT_EVENT_ALREADYCHECK;
                    }
                    else
                    {
                        EventVisitModel Event = EventVisitSync.GetEvent(EventId);
                        if (Event == null)
                        {
                            Client.SendPacket(new PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK(EventErrorEnum.VISIT_EVENT_UNKNOWN));
                            return;
                        }
                        if (Event.EventIsEnabled())
                        {
                            VisitItemModel Reward = Event.GetReward(Player.Event.LastVisitSequence2, Type);
                            if (Reward != null)
                            {
                                GoodsItem Good = ShopManager.GetGood(Reward.GoodId);
                                if (Good != null)
                                {
                                    Player.Event.DayCheckedIdx = DayCheckedIdx;
                                    Player.Event.NextVisitDate = int.Parse(DateTimeUtil.Now().AddDays(1).ToString("yyMMdd"));
                                    ComDiv.UpdateDB("player_events", "owner_id", Player.PlayerId, new string[] { "next_visit_date", "last_visit_sequence2" }, Player.Event.NextVisitDate, ++Player.Event.LastVisitSequence2);
                                    Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(Good.Item)));
                                }
                                else
                                {
                                    Error = EventErrorEnum.VISIT_EVENT_NOTENOUGH;
                                }
                            }
                            else
                            {
                                Error = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                            }
                        }
                        else
                        {
                            Error = EventErrorEnum.VISIT_EVENT_WRONGVERSION;
                        }
                    }
                }
                else
                {
                    Error = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                }
                Client.SendPacket(new PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK(Error));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}