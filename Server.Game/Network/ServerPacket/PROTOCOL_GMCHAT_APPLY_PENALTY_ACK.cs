namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_GMCHAT_APPLY_PENALTY_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_GMCHAT_APPLY_PENALTY_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(6664);
        }
    }
}
