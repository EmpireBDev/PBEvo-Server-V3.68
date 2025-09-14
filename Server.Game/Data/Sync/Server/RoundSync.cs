using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using System;

namespace Server.Game.Data.Sync.Server
{
    public class RoundSync
    {
        public static void SendUDPRoundSync(RoomModel Room)
        {
            try
            {
                if (Room == null)
                {
                    return;
                }
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(3);
                    S.WriteD(Room.UniqueRoomId);
                    S.WriteD(Room.Seed);
                    S.WriteC((byte)Room.Rounds);
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
