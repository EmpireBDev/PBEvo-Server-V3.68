using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_ACK : GameServerPacket
    {
        private readonly int Slot;
        private readonly float X, Y, Z;
        private readonly byte Zone;
        private readonly ushort Unk;
        public PROTOCOL_BATTLE_MISSION_BOMB_INSTALL_ACK(int Slot, byte Zone, ushort Unk, float X, float Y, float Z)
        {
            this.Zone = Zone;
            this.Slot = Slot;
            this.Unk = Unk;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public override void Write()
        {
            WriteH(4133);
            WriteD(Slot);
            WriteC(Zone);
            WriteH(Unk);
            WriteT(X);
            WriteT(Y);
            WriteT(Z);
        }
    }
}