using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_INV_ITEM_DATA_ACK : GameServerPacket
    {
        private readonly int Type;
        private readonly Account Player;
        public PROTOCOL_BASE_INV_ITEM_DATA_ACK(int Type, Account Player)
        {
            this.Type = Type;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(603);
            WriteC((byte)Type);
            WriteC((byte)Player.NickColor);
            WriteD(Player.Bonus.FakeRank);
            WriteD(Player.Bonus.FakeRank);
            WriteU(Player.Bonus.FakeNick, 66);
            WriteH((short)Player.Bonus.CrosshairColor);
            WriteH((short)Player.Bonus.MuzzleColor);
        }
    }
}