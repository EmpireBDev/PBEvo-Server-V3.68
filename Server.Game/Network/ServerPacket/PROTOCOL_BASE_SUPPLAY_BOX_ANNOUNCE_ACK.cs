namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_SUPPLAY_BOX_ANNOUNCE_ACK : GameServerPacket
    {
        private readonly string Message;
        public PROTOCOL_BASE_SUPPLAY_BOX_ANNOUNCE_ACK(string Message)
        {
            this.Message = Message;
        }
        public override void Write()
        {
            WriteH(617);
            WriteD(0);
        }
    }
}
