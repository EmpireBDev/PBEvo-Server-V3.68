namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_GM_PAUSE_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_BATTLE_GM_PAUSE_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(4182);
            WriteD(Error);
            if (Error == 0)
            {
                WriteD(1);
            }
            /*
             * 0x80000000 : STR_GM_PAUSE_FAIL_REASON_C4_INSTALL
             * 0x80000001 : STR_GM_PAUSE_FAIL_REASON_ALREADY_PAUSE
             * 0x80000002 : STR_GM_PAUSE_FAIL_REASON_U_R_NOT_GM
             * 0x80000003 : STR_GM_PAUSE_FAIL_REASON_TIME_LIMIT
             */
        }
    }
}
