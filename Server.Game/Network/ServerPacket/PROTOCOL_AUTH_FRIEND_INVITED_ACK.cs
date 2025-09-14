using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_FRIEND_INVITED_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_AUTH_FRIEND_INVITED_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(788);
            WriteD(Error);
            /*
             * 2147495938 STR_TBL_NETWORK_FAIL_INVITED_USER
             * 2147495939 STR_TBL_NETWORK_FAIL_INVITED_USER_IN_CLAN_MATCH
             */
        }
    }
}