using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_MESSENGER_NOTE_SEND_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_MESSENGER_NOTE_SEND_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(898);
            WriteD(erro);
        }
    }
}