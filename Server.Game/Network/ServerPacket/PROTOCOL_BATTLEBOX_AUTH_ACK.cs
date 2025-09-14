using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLEBOX_AUTH_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly int ItemId;
        public PROTOCOL_BATTLEBOX_AUTH_ACK(Account Player, int ItemId)
        {
            this.Player = Player;
            this.ItemId = ItemId;
        }
        public override void Write()
        {
            WriteH(7430);
            WriteB(new byte[6]);
            WriteD(ItemId);
            WriteB(new byte[5]);
            WriteD(Player.Tags);
        }
    }
}
