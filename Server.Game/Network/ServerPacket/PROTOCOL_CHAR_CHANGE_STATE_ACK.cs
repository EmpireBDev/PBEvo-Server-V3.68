using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CHAR_CHANGE_STATE_ACK : GameServerPacket
    {
        private readonly CharacterModel Chara;
        public PROTOCOL_CHAR_CHANGE_STATE_ACK(CharacterModel Chara)
        {
            this.Chara = Chara;
        }
        public override void Write()
        {
            WriteH(6153);
            WriteH(0);
            WriteD(0);
            WriteC(20);
            WriteC((byte)Chara.Slot);
        }
    }
}
