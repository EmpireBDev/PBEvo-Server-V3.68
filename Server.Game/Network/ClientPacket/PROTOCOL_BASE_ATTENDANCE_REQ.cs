using Plugin.Core;
using Plugin.Core.Managers.Events;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Network.ServerPacket;
using System;
using Server.Game.Data.Models;
using Plugin.Core.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_ATTENDANCE_REQ : GameClientPacket
    {
        private EventErrorEnum Error = EventErrorEnum.VISIT_EVENT_SUCCESS;
        private int EventId;
        private int DayCheckedIdx;
        private EventVisitModel Event;
        public PROTOCOL_BASE_ATTENDANCE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            EventId = ReadD();
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
                if (string.IsNullOrEmpty(Player.Nickname))
                {
                    Error = EventErrorEnum.VISIT_EVENT_USERFAIL;
                }
                else if (Player.Event != null)
                {
                    int dateNow = int.Parse(DateTimeUtil.Now("yyMMdd"));
                    if (Player.Event.NextVisitDate <= dateNow)
                    {
                        Event = EventVisitSync.GetEvent(EventId);
                        if (Event == null)
                        {
                            Client.SendPacket(new PROTOCOL_BASE_ATTENDANCE_ACK(EventErrorEnum.VISIT_EVENT_UNKNOWN, null, Player.Event));
                            return;
                        }
                        if (Event.EventIsEnabled())
                        {
                            Player.Event.DayCheckedIdx = DayCheckedIdx;
                            Player.Event.NextVisitDate = int.Parse(DateTimeUtil.Now().AddDays(1).ToString("yyMMdd"));
                            ComDiv.UpdateDB("player_events", "owner_id", Player.PlayerId, new string[] { "next_visit_date", "last_visit_sequence1" }, Player.Event.NextVisitDate, ++Player.Event.LastVisitSequence1);
                            bool IsReward = Event.Boxes[Player.Event.LastVisitSequence2].Reward1.IsReward;
                            if (!IsReward)
                            {
                                ComDiv.UpdateDB("player_events", "last_visit_sequence2", ++Player.Event.LastVisitSequence2, "owner_id", Player.PlayerId);
                            }
                        }
                        else
                        {
                            Error = EventErrorEnum.VISIT_EVENT_WRONGVERSION;
                        }
                    }
                    else
                    {
                        Error = EventErrorEnum.VISIT_EVENT_ALREADYCHECK;
                    }
                }
                else
                {
                    Error = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                }
                Client.SendPacket(new PROTOCOL_BASE_ATTENDANCE_ACK(Error, Event, Player.Event));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_ATTENDANCE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}