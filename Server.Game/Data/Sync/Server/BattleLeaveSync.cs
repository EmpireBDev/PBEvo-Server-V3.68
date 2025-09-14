using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using System;

namespace Server.Game.Data.Sync.Server
{
    public class BattleLeaveSync
    {
        public static void SendUDPPlayerLeave(RoomModel Room, int SlotId)
        {
            try
            {
                if (Room == null)
                {
                    return;
                }
                int Count = Room.GetPlayingPlayers(TeamEnum.TEAM_DRAW, SlotState.BATTLE, 0, SlotId);
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(2);
                    S.WriteD(Room.UniqueRoomId);
                    S.WriteD(Room.Seed);
                    S.WriteC((byte)SlotId);
                    S.WriteC((byte)Count);
                    GameXender.Sync.SendPacket(S.ToArray(), Room.UdpServer.Connection);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}