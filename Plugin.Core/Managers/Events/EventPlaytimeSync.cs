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
    public class EventPlaytimeSync
    {
        private static readonly List<EventPlaytimeModel> Events = new List<EventPlaytimeModel>();

        public static void Load()
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "SELECT * FROM system_events_playtime";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        EventPlaytimeModel ev = new EventPlaytimeModel()
                        {
                            StartDate = uint.Parse(Data["start_date"].ToString()),
                            EndDate = uint.Parse(Data["end_date"].ToString()),
                            Title = Data["title"].ToString(),
                            Time = long.Parse(Data["playtime_req"].ToString()),
                            GoodReward1 = int.Parse(Data["goods_reward1"].ToString()),
                            GoodReward2 = int.Parse(Data["goods_reward2"].ToString())
                        };
                        Events.Add(ev);
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
            CLogger.Print($"Plugin Loaded: {Events.Count} Event Playtime", LoggerType.Info);
        }
        public static void Reload()
        {
            Events.Clear();
            Load();
        }
        public static EventPlaytimeModel GetRunningEvent()
        {
            lock (Events)
            {
                uint Date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
                foreach (EventPlaytimeModel Event in Events)
                {
                    if (Event.StartDate <= Date && Date < Event.EndDate)
                    {
                        return Event;
                    }
                }
                return null;
            }
        }
        public static void ResetPlayerEvent(long PlayerId, PlayerEvent Event)
        {
            if (PlayerId == 0)
            {
                return;
            }
            ComDiv.UpdateDB("player_events", "owner_id", PlayerId, new string[] { "last_playtime_value", "last_playtime_finish", "last_playtime_date" }, Event.LastPlaytimeValue, Event.LastPlaytimeFinish, (long)Event.LastPlaytimeDate);
        }
    }
}
