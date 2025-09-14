using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REPLACE_MEMBER_BASIC_RESULT_ACK : GameServerPacket
    {
        private readonly Account pl;
        private readonly ulong status;
        public PROTOCOL_CS_REPLACE_MEMBER_BASIC_RESULT_ACK(Account pl)
        {
            this.pl = pl;
            status = ComDiv.GetClanStatus(pl.Status, pl.IsOnline);
        }
        public override void Write()
        {
            WriteH(1900);
            WriteQ(pl.PlayerId);
            WriteU(pl.Nickname, 66);
            WriteC((byte)pl.Rank);
            WriteC((byte)pl.ClanAccess);
            WriteQ(status);
            WriteD(pl.ClanDate);
            WriteC((byte)pl.NickColor);
            WriteD(0); // ?
            WriteD(0); // ?
        }
    }
}