namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_MISSION_SUCCESS_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_MISSION_SUCCESS_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
        }
    }
}