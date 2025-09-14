using Plugin.Core.Network;
using Plugin.Core.Utility;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_SERVER_MESSAGE_ERROR_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_SERVER_MESSAGE_ERROR_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(2566);
            //WriteD(uint.Parse(DateTimeUtil.Now("MMddHHmmss")));
            WriteD(erro);
            /*
             * 0x800010AD STBL_IDX_EP_GAME_EXIT_HACKUSER
             * 0x800010AE STBL_IDX_EP_GAMEGUARD_ERROR (Ocorreu um problema no HackShield)
             * 0x800010AF STBL_IDX_EP_GAME_EXIT_ASSERT_E
             * 0x800010B0 STBL_IDX_EP_GAME_EXIT_ASSERT_E
             */
        }
    }
}