using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly uint Error;
        private readonly int Type;
        public PROTOCOL_BASE_QUEST_ACTIVE_IDX_CHANGE_ACK(uint Error, int Type, Account Player)
        {
            this.Error = Error;
            this.Type = Type;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(569);
            WriteD(Error); 
            WriteC((byte)Type);
            if ((Error & 1) == 1)
            {
                WriteD(Player.Exp);
                WriteD(Player.Gold);
                WriteD(Player.Ribbon);
                WriteD(Player.Ensign);
                WriteD(Player.Medal);
                WriteD(Player.MasterMedal);
                WriteD(Player.Rank);
            }
        }
    }
}