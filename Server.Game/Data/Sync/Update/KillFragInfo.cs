using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Client;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;

namespace Server.Game.Data.Sync.Update
{
    public class KillFragInfo
    {
        public static void GenDeath(RoomModel Room, SlotModel Killer, FragInfos Kill, bool IsSuicide)
        {
            bool IsBotMode = Room.IsBotMode();
            RoomDeath.RegistryFragInfos(Room, Killer, out int score, IsBotMode, IsSuicide, Kill);
            if (IsBotMode)
            {
                Killer.Score += Killer.KillsOnLife + Room.IngameAiLevel + score;
                if (Killer.Score > 65535)
                {
                    Killer.Score = 65535;
                    CLogger.Print("[PlayerId: " + Killer.Id + "] reached the maximum score of the BOT.", LoggerType.Warning);
                }
                Kill.Score = Killer.Score;
            }
            else
            {
                Killer.Score += score;
                AllUtils.CompleteMission(Room, Killer, Kill, MissionType.NA, 0);
                Kill.Score = score;
            }
            using (PROTOCOL_BATTLE_DEATH_ACK Packet = new PROTOCOL_BATTLE_DEATH_ACK(Room, Kill, Killer))
            {
                Room.SendPacketToPlayers(Packet, SlotState.BATTLE, 0);
            }
            RoomDeath.EndBattleByDeath(Room, Killer, IsBotMode, IsSuicide);
        }
    }
}
