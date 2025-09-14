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
    public class EventLoginSync
    {
        private static List<EventLoginModel> Events = new List<EventLoginModel>();
        public static void Load()
        {
            try
            {
                using (NpgsqlConnection Connection = ConnectionSQL.GetInstance().Conn())
                {
                    NpgsqlCommand Command = Connection.CreateCommand();
                    Connection.Open();
                    Command.CommandText = "SELECT * FROM system_events_login";
                    Command.CommandType = CommandType.Text;
                    NpgsqlDataReader Data = Command.ExecuteReader();
                    while (Data.Read())
                    {
                        EventLoginModel Event = new EventLoginModel()
                        {
                            BeginDate = uint.Parse(Data["start_date"].ToString()),
                            FinishDate = uint.Parse(Data["end_date"].ToString()),
                            GoodId = int.Parse(Data["goods_id"].ToString())
                        };
                        if (Event.GoodId < 10000000)
                        {
                            CLogger.Print($"Event with incorrect reward! [Id: {Event.GoodId}]", LoggerType.Warning);
                        }
                        else
                        {
                            Events.Add(Event);
                        }
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
            CLogger.Print($"Plugin Loaded: {Events.Count} Event Login", LoggerType.Info);
        }
        public static void Reload()
        {
            Events.Clear();
            Load();
        }
        public static EventLoginModel GetRunningEvent()
        {
            uint Date = uint.Parse(DateTimeUtil.Now("yyMMddHHmm"));
            foreach (EventLoginModel Event in Events)
            {
                if (Event.BeginDate <= Date && Date < Event.FinishDate)
                {
                    return Event;
                }
            }
            return null;
        }
    }
}
