using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_STARTBATTLE_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly SlotModel Slot;
        private readonly bool Type;
        private readonly List<int> Dinos;
        public PROTOCOL_BATTLE_STARTBATTLE_ACK(SlotModel Slot, Account Player, List<int> Dinos, bool IsBotMode, bool Type)
        {
            this.Slot = Slot;
            Room = Player.Room;
            if (Room != null)
            {
                this.Type = Type;
                this.Dinos = Dinos;
                if (!IsBotMode && Room.RoomType != RoomCondition.Tutorial)
                {
                    AllUtils.CompleteMission(Room, Player, Slot, Type ? MissionType.STAGE_ENTER : MissionType.STAGE_INTERCEPT, 0);
                }
            }
        }
        public PROTOCOL_BATTLE_STARTBATTLE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(4108);
            WriteH(0);
            WriteD(0);
            WriteC(0);
            WriteB(DinoData(Room, Dinos));
            WriteC((byte)Room.Rounds);
            WriteH(AllUtils.GetSlotsFlag(Room, true, false));
            WriteC((byte)((Room.ThisModeHaveRounds() || Room.IsDinoMode() || Room.RoomType == RoomCondition.FreeForAll) ? 2 : 0));
            if (Room.ThisModeHaveRounds() || Room.IsDinoMode() || Room.RoomType == RoomCondition.FreeForAll)
            {
                WriteH((ushort)(Room.IsDinoMode("DE") ? Room.FRDino : Room.IsDinoMode("CC") ? Room.FRKills : Room.FRRounds));
                WriteH((ushort)(Room.IsDinoMode("DE") ? Room.CTDino : Room.IsDinoMode("CC") ? Room.CTKills : Room.CTRounds));
            }
            WriteC((byte)((Room.ThisModeHaveRounds() || Room.IsDinoMode() || Room.RoomType == RoomCondition.FreeForAll) ? 2 : 0));
            if (Room.ThisModeHaveRounds() || Room.IsDinoMode() || Room.RoomType == RoomCondition.FreeForAll)
            {
                WriteH(0); //Change Team Score (FR)
                WriteH(0); //Change Team Score (CT)
            }
            WriteH(AllUtils.GetSlotsFlag(Room, false, false));
            WriteC((byte)(Type ? 0 : 1));
            WriteC((byte)Slot.Id);
        }
        private byte[] DinoData(RoomModel Room, List<int> Dinos)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (Room.IsDinoMode())
                {
                    int TRex = Dinos.Count == 1 || Room.IsDinoMode("CC") ? 255 : Room.TRex;
                    S.WriteC((byte)TRex);
                    S.WriteC(10);
                    for (int i = 0; i < Dinos.Count; i++)
                    {
                        int SlotId = Dinos[i];
                        if (SlotId != Room.TRex && Room.IsDinoMode("DE") || Room.IsDinoMode("CC"))
                        {
                            S.WriteC((byte)SlotId);
                        }
                    }
                    int Value = 8 - Dinos.Count - (TRex == 255 ? 1 : 0);
                    for (int i = 0; i < Value; i++)
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