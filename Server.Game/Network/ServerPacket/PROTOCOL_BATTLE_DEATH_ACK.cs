using Plugin.Core.Models;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_DEATH_ACK : GameServerPacket
    {
        private readonly RoomModel Room;
        private readonly FragInfos Kills;
        private readonly SlotModel Killer;
        public PROTOCOL_BATTLE_DEATH_ACK(RoomModel Room, FragInfos Kills, SlotModel Killer)
        {
            this.Room = Room;
            this.Kills = Kills;
            this.Killer = Killer;
        }
        public override void Write()
        {
            WriteH(4112);
            WriteC((byte)Kills.KillingType);
            WriteC(Kills.KillsCount);
            WriteC(Kills.KillerIdx);
            WriteD(Kills.Weapon);
            WriteT(Kills.X);
            WriteT(Kills.Y);
            WriteT(Kills.Z);
            WriteC(Kills.Flag);
            WriteC(Kills.Unk);
            for (int i = 0; i < Kills.KillsCount; i++)
            {
                FragModel Frag = Kills.Frags[i];
                WriteC(Frag.VictimWeaponClass);
                WriteC(Frag.HitspotInfo);
                WriteH((ushort)Frag.KillFlag);
                WriteC(Frag.Flag);
                WriteT(Frag.X);
                WriteT(Frag.Y);
                WriteT(Frag.Z);
                WriteC(Frag.AssistSlot);
                WriteB(Frag.Unk);
            }
            WriteH((ushort)Room.FRKills);
            WriteH((ushort)Room.FRDeaths);
            WriteH((ushort)Room.FRAssists);
            WriteH((ushort)Room.CTKills);
            WriteH((ushort)Room.CTDeaths);
            WriteH((ushort)Room.CTAssists);
            foreach (SlotModel Slot in Room.Slots)
            {
                WriteH((ushort)Slot.AllKills);
                WriteH((ushort)Slot.AllDeaths);
                WriteH((ushort)Slot.AllAssists);
            }
            WriteC((byte)(Killer.KillsOnLife == 2 ? 1 : Killer.KillsOnLife == 3 ? 2 : Killer.KillsOnLife > 3 ? 3 : 0));
            WriteH((ushort)Kills.Score);
            if (Room.IsDinoMode("DE"))
            {
                WriteH((ushort)Room.FRDino);
                WriteH((ushort)Room.CTDino);
            }
        }
    }
}