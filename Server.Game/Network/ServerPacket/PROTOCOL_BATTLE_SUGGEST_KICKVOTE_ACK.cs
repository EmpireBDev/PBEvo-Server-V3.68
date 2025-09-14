namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_SUGGEST_KICKVOTE_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_BATTLE_SUGGEST_KICKVOTE_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(3397);
            WriteD(erro);
        }
    }
}