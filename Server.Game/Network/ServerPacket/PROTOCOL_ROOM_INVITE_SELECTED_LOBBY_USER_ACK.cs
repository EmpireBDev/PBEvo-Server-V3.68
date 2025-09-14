using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_INVITE_SELECTED_LOBBY_USER_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_ROOM_INVITE_SELECTED_LOBBY_USER_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(3676);
            WriteD(erro);
        }
    }
}