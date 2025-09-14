using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.JSON;
using Plugin.Core.Models;
using Plugin.Core.Network;

namespace Server.Auth.Data.Sync.Client
{
    public class ReloadConfig
    {
        public static void Load(SyncClientPacket C)
        {
            int Config = C.ReadC();
            ServerConfig MainCFG = ServerConfigJSON.GetConfig(Config);
            if (MainCFG != null && MainCFG.ConfigId > 0)
            {
                AuthXender.Client.Config = MainCFG;
                CLogger.Print($"Configuration (Database) Refills; Config: {Config}", LoggerType.Command);
            }
        }
    }
}
