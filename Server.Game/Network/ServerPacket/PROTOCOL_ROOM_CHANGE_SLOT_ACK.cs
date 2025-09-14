using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHANGE_SLOT_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_ROOM_CHANGE_SLOT_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(3861);
            WriteD(erro);
        }
    }
}