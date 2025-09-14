using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_GMCHAT_START_CHAT_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly uint Error;
        public PROTOCOL_GMCHAT_START_CHAT_ACK(uint Error, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(6658);
            WriteH(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteC((byte)(Player.Nickname.Length + 1));
                WriteN(Player.Nickname, Player.Nickname.Length + 2, "UTF-16LE");
                WriteQ(Player.PlayerId);
            }
        }
    }
}
