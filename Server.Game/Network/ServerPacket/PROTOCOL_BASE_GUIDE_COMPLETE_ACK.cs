namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GUIDE_COMPLETE_ACK : GameServerPacket
    {
        public PROTOCOL_BASE_GUIDE_COMPLETE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(549);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteD(0);
        }
    }
}
