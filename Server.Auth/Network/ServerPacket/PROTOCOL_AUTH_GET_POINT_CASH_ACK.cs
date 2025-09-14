using Server.Auth.Data.Models;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_AUTH_GET_POINT_CASH_ACK : AuthServerPacket
    {
        private readonly uint Error;
        private readonly Account Player;
        public PROTOCOL_AUTH_GET_POINT_CASH_ACK(uint Error, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(1058);
            WriteD(Error);
            if (Error >= 0)
            {
                WriteD(Player.Gold);
                WriteD(Player.Cash);
                WriteD(Player.Tags);
            }
            WriteD(0);
        }
    }
}