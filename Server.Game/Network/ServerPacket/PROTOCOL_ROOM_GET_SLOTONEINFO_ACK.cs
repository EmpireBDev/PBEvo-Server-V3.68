using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_SLOTONEINFO_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly RoomModel Room;
        private readonly ClanModel Clan;
        public PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(Account Player)
        {
            this.Player = Player;
            if (Player != null)
            {
                Room = Player.Room;
                Clan = ClanManager.GetClan(Player.ClanId);
            }
        }
        public PROTOCOL_ROOM_GET_SLOTONEINFO_ACK(Account Player, ClanModel Clan)
        {
            this.Player = Player;
            if (Player != null)
            {
                Room = Player.Room;
            }
            this.Clan = Clan;
        }
        public override void Write()
        {
            WriteH(3846);
            WriteH(0);
            WriteC((byte)(Player.SlotId % 2));
            WriteC((byte)Room.GetSlot(Player.SlotId).State);
            WriteC((byte)Player.GetRank());
            WriteD(Clan.Id);
            WriteD(Player.ClanAccess);
            WriteC((byte)Clan.Rank);
            WriteD(Clan.Logo);
            WriteC((byte)Player.CafePC);
            WriteC((byte)Player.TourneyLevel);
            WriteD((uint)Player.Effects);
            WriteC((byte)Clan.Effect);
            WriteC(0);
            WriteH((ushort)NATIONS);
            WriteD(Player.Equipment.NameCardId);
            WriteH(0);
            WriteU(Clan.Name, 34);
            WriteC((byte)Player.SlotId);
            WriteU(Player.Nickname, 66);
            WriteC((byte)Player.NickColor);
            WriteC((byte)Player.Bonus.MuzzleColor);
            WriteC(0);
            WriteC(255);
            WriteC(255);
        }
    }
}