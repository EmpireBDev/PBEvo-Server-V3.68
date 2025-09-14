namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_RESULT_ACK : GameServerPacket
    {
        public PROTOCOL_CLAN_WAR_RESULT_ACK()
        {
        }
        public override void Write()
        {
            WriteH(6964);
        }
    }
}
