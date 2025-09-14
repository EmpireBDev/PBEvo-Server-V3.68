using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_MEDAL_GET_INFO_ACK : GameServerPacket
    {
        private readonly Account Player;
        public PROTOCOL_BASE_MEDAL_GET_INFO_ACK(Account Player)
        {
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(571);
            WriteQ(Player.PlayerId);
            WriteD(Player.Ribbon);
            WriteD(Player.Ensign);
            WriteD(Player.Medal);
            WriteD(Player.MasterMedal);
        }
    }
}
