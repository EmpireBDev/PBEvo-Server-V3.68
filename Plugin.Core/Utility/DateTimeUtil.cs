using Plugin.Core.Enums;
using Plugin.Core.Settings;
using System;
using System.IO;

namespace Plugin.Core.Utility
{
    public class DateTimeUtil
    {
        public static DateTime Now()
        {
            DateTime BaseDT = DateTime.Now;
            try
            {
                int ChangeYear = (ConfigLoader.LockYear - BaseDT.Year);
                return (ConfigLoader.IsLockedTime ? BaseDT.AddYears(ChangeYear) : BaseDT.AddYears(-ConfigLoader.BackYear));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return new DateTime();
            }
        }
        public static string Now(string Format)
        {
            return Now().ToString(Format);
        }
        public static string LogNow(string Format)
        {
            DateTime BaseDT = DateTime.Now;
            ConfigEngine CFG = new ConfigEngine("Config/Timeline.ini", FileAccess.Read);
            try
            {
                int ChangeYear = (CFG.ReadD("LockYear", 2000, "Runtime") - BaseDT.Year);
                return (CFG.ReadX("LockedTime", true, "Addons") ? BaseDT.AddYears(ChangeYear) : BaseDT.AddYears(-CFG.ReadD("BackYear", 10, "Runtime"))).ToString(Format);
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return new DateTime().ToString(Format);
            }
        }
    }
}
