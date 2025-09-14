using Server.Game.Network.ServerPacket;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_CHANNELTYPE_CHANGE_CONDITION_REQ : GameClientPacket
    {
        public PROTOCOL_BASE_CHANNELTYPE_CHANGE_CONDITION_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
        }
        public override void Run()
        {
            Client.SendPacket(new PROTOCOL_BASE_CHANNELTYPE_CHANGE_CONDITION_ACK());
        }
    }
}
