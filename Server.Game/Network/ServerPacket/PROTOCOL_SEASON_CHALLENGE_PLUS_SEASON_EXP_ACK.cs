using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SEASON_CHALLENGE_PLUS_SEASON_EXP_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_SEASON_CHALLENGE_PLUS_SEASON_EXP_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(8455);
            WriteH(0);
            WriteD(Error);
            WriteC(1); //unk
            WriteC(6); //Next current level

            WriteD(2580); //Total earned points

            WriteC(5); //Current normal level complete
            WriteC(5); //Current premium level complete
            WriteC(1); //Premium access
        }
    }
}