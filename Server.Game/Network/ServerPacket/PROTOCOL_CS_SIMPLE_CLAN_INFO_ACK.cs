namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_SIMPLE_CLAN_INFO_ACK : GameServerPacket
    {
        private readonly int selectId;
        public PROTOCOL_CS_SIMPLE_CLAN_INFO_ACK(int selectId)
        {
            this.selectId = selectId;
        }
        public override void Write()
        {
            WriteH(2024);
            WriteD(selectId);
            //WriteC((byte)selectId);
        }
    }
}