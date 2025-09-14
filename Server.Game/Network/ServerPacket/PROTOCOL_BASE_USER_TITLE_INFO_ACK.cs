using Server.Game.Data.Models;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_USER_TITLE_INFO_ACK : GameServerPacket
    {
        private readonly Account Player;
        public PROTOCOL_BASE_USER_TITLE_INFO_ACK(Account Player)
        {
            this.Player = Player;
        }
        public override void Write()
        {
            WriteH(591);
            WriteB(BitConverter.GetBytes(Player.PlayerId), 0, 4);
            WriteQ(Player.Title.Flags);
            WriteC((byte)Player.Title.Equiped1);
            WriteC((byte)Player.Title.Equiped2);
            WriteC((byte)Player.Title.Equiped3);
            WriteD(Player.Title.Slots);
        }
    }
}
