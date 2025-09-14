using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_CREATE_NICK_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly Account Player;
        public PROTOCOL_BASE_CREATE_NICK_ACK(uint Error, Account Player)
        {
            this.Error = Error;
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(535);
            WriteH(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteC(0);
                WriteD(Player.Equipment.DinoItem);
                WriteD((uint)Player.Inventory.GetItem(Player.Equipment.DinoItem).ObjectId);
                WriteC((byte)(Player.Nickname.Length * 2));
                WriteU(Player.Nickname, Player.Nickname.Length * 2);
            }
        }
    }
}