using Npgsql;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace Plugin.Core.Managers.Events
{
    public class EventVisitSync
    {
        public static readonly List<EventVisitModel> Events = new List<EventVisitModel>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "SELECT * FROM system_events_visit";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        EventVisitModel Event = new EventVisitModel()
                        {
                            Id = int.Parse(Data["id"].ToString()),
                            StartDate = uint.Parse(Data["start_date"].ToString()),
                            EndDate = uint.Parse(Data["end_date"].ToString()),
                            Title = Data["title"].ToString(),
                            Checks = int.Parse(Data["total_check"].ToString())
                        };
                        string[] GoodsLists1 = Data["goods_list1"].ToString().Split(',');
                        string[] GoodsLists2 = Data["goods_list2"].ToString().Split(',');
                        for (int i = 0; i < GoodsLists1.Length; i++)
                        {
                            Event.Boxes[i].Reward1.SetGoodId(GoodsLists1[i]);
                        }
                        for (int i = 0; i < GoodsLists2.Length; i++)
                        {
                            Event.Boxes[i].Reward2.SetGoodId(GoodsLists2[i]);
                        }
                        Event.SetBoxCounts();
                        Events.Add(Event);
                    }
                    Command.Dispose();
                    Data.Close();
                    Connection.Dispose();
                    Connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            CLogger.Print($"Plugin Loaded: {Events.Count} Event Visit", LoggerType.Info);
        }
        public static void Reload()
        {
            Events.Clear();
            Load();
        }
        public static EventVisitModel GetEvent(int EventId)
        {
            lock (Events)
            {
                foreach (EventVisitModel Event in Events)
                {
                    if (Event.Id == EventId)
                    {
                        return Event;
                    }
                }
                return null;
            }
        }
        public static EventVisitModel GetRunningEvent()
        {
            lock (Events)
            {
                uint Date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
                foreach (EventVisitModel Event in Events)
                {
                    if (Event.StartDate <= Date && Date < Event.EndDate)
                    {
                        return Event;
                    }
                }
                return null;
            }
        }
        public static void ResetPlayerEvent(long PlayerId, int EventId)
        {
            if (PlayerId == 0)
            {
                return;
            }
            ComDiv.UpdateDB("player_events", "owner_id", PlayerId, new string[] { "last_visit_event_id", "last_visit_sequence1", "last_visit_sequence2", "next_visit_date" }, EventId, 0, 0, 0);
        }
    }
}
