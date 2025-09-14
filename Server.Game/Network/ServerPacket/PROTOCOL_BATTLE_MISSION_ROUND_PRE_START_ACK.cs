using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly List<int> Dinos;
        private readonly bool IsBotMode;
        public PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(RoomModel Room, List<int> Dinos, bool IsBotMode)
        {
            this.Room = Room;
            this.Dinos = Dinos;
            this.IsBotMode = IsBotMode;
        }
        public PROTOCOL_BATTLE_MISSION_ROUND_PRE_START_ACK(RoomModel Room)
        {
            this.Room = Room;
            Dinos = AllUtils.GetDinossaurs(Room, false, -1);
            IsBotMode = this.Room.IsBotMode();
        }
        public override void Write()
        {
            WriteH(4127);
            WriteH(AllUtils.GetSlotsFlag(Room, false, false));
            WriteB(DinoData(Room, IsBotMode, Dinos));
            WriteC(0);
        }
        private byte[] DinoData(RoomModel Room, bool IsBotMode, List<int> Dinos)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (IsBotMode)
                {
                    S.WriteB(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });
                }
                else if (Room.IsDinoMode())
                {
                    int TRex = Dinos.Count == 1 || Room.IsDinoMode("CC") ? 255 : Room.TRex;
                    S.WriteC((byte)TRex);
                    S.WriteC(10);
                    for (int i = 0; i < Dinos.Count; i++)
                    {
                        int slotId = Dinos[i];
                        if (slotId != Room.TRex && Room.IsDinoMode("DE") || Room.IsDinoMode("CC"))
                        {
                            S.WriteC((byte)slotId);
                        }
                    }
                    int Fault = 8 - Dinos.Count - (TRex == 255 ? 1 : 0);
                    for (int i = 0; i < Fault; i++)
                    {
                        S.WriteC(255);
                    }
                    S.WriteC(255);
                }
                else
                {
                    S.WriteB(new byte[10]);
                }
                return S.ToArray();
            }
        }
    }
}