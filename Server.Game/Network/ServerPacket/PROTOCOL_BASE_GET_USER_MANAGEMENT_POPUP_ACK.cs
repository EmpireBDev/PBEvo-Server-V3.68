namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_USER_MANAGEMENT_POPUP_ACK : GameServerPacket
    {
        public PROTOCOL_BASE_GET_USER_MANAGEMENT_POPUP_ACK()
        {
        }
        public override void Write()
        {
            WriteH(6658);
        }
    }
}
