using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Filters;
using Plugin.Core.JSON;
using Plugin.Core.Managers;
using Plugin.Core.Managers.Events;
using Plugin.Core.XML;
using System;

namespace Executable.Utility
{
    public class ReloaderUtil
    {
        public static bool ReloadConfig()
        {
            try
            {
                ConfigLoader.Load();
                ServerConfigJSON.Configs.Clear();
                ServerConfigJSON.Load();
                NickFilter.Filters.Clear();
                NickFilter.Load();
                Translation.Strings.Clear();
                Translation.Load();
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static bool ReloadShop()
        {
            try
            {
                ShopManager.Reset();
                ShopManager.Load(1);
                ShopManager.Load(2);
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static bool ReloadEvent()
        {
            try
            {
                EventLoginSync.Reload();
                EventMapSync.Reload();
                EventPlaytimeSync.Reload();
                EventQuestSync.Reload();
                EventRankUpSync.Reload();
                EventVisitSync.Reload();
                EventXmasSync.Reload();
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static bool ReloadRules()
        {
            try
            {
                GameRuleXML.Reload();
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
        public static bool ReloadAttachments()
        {
            try
            {
                TemplatePackXML.Basics.Clear();
                TemplatePackXML.Awards.Clear();
                TemplatePackXML.CafePCs.Clear();
                TemplatePackXML.Load();
                SystemMapXML.Rules.Clear();
                SystemMapXML.Matches.Clear();
                SystemMapXML.Load();
                RandomBoxXML.RBoxes.Clear();
                RandomBoxXML.Load();
                InternetCafeXML.Cafes.Clear();
                InternetCafeXML.Load();
                return true;
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
                return false;
            }
        }
    }
}
