namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_LEAVE_TEAM_ACK : GameServerPacket
    {
        private readonly uint erro;
        public PROTOCOL_CLAN_WAR_LEAVE_TEAM_ACK(uint erro)
        {
            this.erro = erro;
        }
        public override void Write()
        {
            WriteH(6923);
            WriteD(erro);
        }
    }
}