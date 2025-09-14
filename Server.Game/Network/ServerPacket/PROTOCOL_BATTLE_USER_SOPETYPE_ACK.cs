using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_USER_SOPETYPE_ACK : GameServerPacket
    {
        private readonly Account Player;
        public PROTOCOL_BATTLE_USER_SOPETYPE_ACK(Account Player)
        {
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(4253);
            WriteD(Player.SlotId);
            WriteC((byte)Player.Sight);
        }
    }
}
