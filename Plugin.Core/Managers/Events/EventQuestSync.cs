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
    public class EventQuestSync
    {
        private static List<EventQuestModel> Events = new List<EventQuestModel>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "SELECT * FROM system_events_quest";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        EventQuestModel ev = new EventQuestModel()
                        {
                            StartDate = uint.Parse(Data["start_date"].ToString()),
                            EndDate = uint.Parse(Data["end_date"].ToString())
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
            CLogger.Print($"Plugin Loaded: {Events.Count} Event Quest", LoggerType.Info);
        }
        public static void Reload()
        {
            Events.Clear();
            Load();
        }
        public static EventQuestModel GetRunningEvent()
        {
            uint Date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
            foreach (EventQuestModel Event in Events)
            {
                if (Event.StartDate <= Date && Date < Event.EndDate)
                {
                    return Event;
                }
            }
            return null;
        }
        public static void ResetPlayerEvent(long pId, PlayerEvent pE)
        {
            if (pId == 0)
            {
                return;
            }
            ComDiv.UpdateDB("player_events", "owner_id", pId, new string[] { "last_quest_date", "last_quest_finish" }, (long)pE.LastQuestDate, pE.LastQuestFinish);
        }
    }
}
