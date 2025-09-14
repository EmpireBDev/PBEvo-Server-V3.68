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
    public class EventRankUpSync
    {
        private static List<EventRankUpModel> Events = new List<EventRankUpModel>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "SELECT * FROM system_events_rankup";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        EventRankUpModel Event = new EventRankUpModel()
                        {
                            StartDate = uint.Parse(Data["start_date"].ToString()),
                            EndDate = uint.Parse(Data["end_date"].ToString()),
                            PercentExp = int.Parse(Data["percent_exp"].ToString()),
                            PercentGold = int.Parse(Data["percent_gold"].ToString())
                        };
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
            CLogger.Print($"Plugin Loaded: {Events.Count} Event Rank Up", LoggerType.Info);
        }
        public static void Reload()
        {
            Events.Clear();
            Load();
        }
        public static EventRankUpModel GetRunningEvent()
        {
            uint Date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
            foreach (EventRankUpModel Event in Events)
            {
                if (Event.StartDate <= Date && Date < Event.EndDate)
                {
                    return Event;
                }
            }
            return null;
        }
    }
}
