using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_GMCHAT_END_CHAT_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly uint Error;
        public PROTOCOL_GMCHAT_END_CHAT_ACK(uint Error, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(6662);
            WriteH(0);
            WriteD(Error);
            if (Error == 0)
            {
                //TODO HERE
            }
        }
    }
}
