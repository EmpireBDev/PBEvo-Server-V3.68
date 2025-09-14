using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_GIVEUPBATTLE_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly int QuitType;
        public PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(Account Player, int QuitType)
        {
            this.Player = Player;
            this.QuitType = QuitType;
        }
        public override void Write()
        {
            WriteH(4110);
            WriteD(Player.SlotId);
            WriteC((byte)QuitType);
            WriteD(Player.Exp);
            WriteD(Player.Rank);
            WriteD(Player.Gold);
            WriteD(Player.Statistic.Season.EscapesCount);
            WriteD(Player.Statistic.Basic.EscapesCount);
            WriteD(0);
            WriteC(0);
        }
    }
}