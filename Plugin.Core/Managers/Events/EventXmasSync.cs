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
    public class EventXmasSync
    {
        private static List<EventXmasModel> Events = new List<EventXmasModel>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM system_events_xmas";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = command.ExecuteReader();
                    while (Data.Read())
                    {
                        EventXmasModel ev = new EventXmasModel()
                        {
                            StartDate = uint.Parse(Data["start_date"].ToString()),
                            EndDate = uint.Parse(Data["end_date"].ToString()),
                            GoodId = int.Parse(Data["goods_id"].ToString())
                        };
                        Events.Add(ev);
                    }
                    command.Dispose();
                    Data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
            CLogger.Print($"Plugin Loaded: {Events.Count} Event Christmas", LoggerType.Info);
        }
        public static void Reload()
        {
            Events.Clear();
            Load();
        }
        public static EventXmasModel GetRunningEvent()
        {
            uint date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
            foreach (EventXmasModel ev in Events)
            {
                if (ev.StartDate <= date && date < ev.EndDate)
                {
                    return ev;
                }
            }
            return null;
        }
    }
}
