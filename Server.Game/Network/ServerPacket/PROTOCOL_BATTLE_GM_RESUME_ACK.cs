namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_GM_RESUME_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_BATTLE_GM_RESUME_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(4246);
            WriteD(Error);
            /*
             * 0x80002004 : STR_GM_RESUME_FAIL_REASON_NOT_PAUSE
             * 0x80002005 : STR_GM_RESUME_FAIL_REASON_U_R_NOT_LOCKER
             */
        }
    }
}
