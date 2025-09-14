using Plugin.Core.Models;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK : GameServerPacket
    {
        private readonly MatchModel Match;
        public PROTOCOL_CLAN_WAR_REGIST_MERCENARY_ACK(MatchModel Match)
        {
            this.Match = Match;
        }
        public override void Write()
        {
            WriteH(6939);
            WriteH((short)Match.GetServerInfo());
            WriteC((byte)Match.State);
            WriteC((byte)Match.FriendId);
            WriteC((byte)Match.Training);
            WriteC((byte)Match.GetCountPlayers());
            WriteD(Match.Leader);
            WriteC(0);
            foreach (SlotMatch Slot in Match.Slots)
            {
                Account p = Match.GetPlayerBySlot(Slot);
                if (p != null)
                {
                    WriteC((byte)p.Rank);
                    WriteS(p.Nickname, 33);
                    WriteQ(Slot.PlayerId);
                    WriteC((byte)Slot.State);
                }
                else
                {
                    WriteB(new byte[43]);
                }
            }
        }
    }
}