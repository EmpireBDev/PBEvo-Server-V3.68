using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Network.ServerPacket;

namespace Server.Game.Data.Sync.Client
{
    public static class ServerMessage
    {
        public static void Load(SyncClientPacket C)
        {
            byte MessageLength = C.ReadC();
            string Message = C.ReadS(MessageLength);
            if (string.IsNullOrEmpty(Message) || MessageLength > 60)
            {
                return;
            }
            int Count = 0;
            using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Message))
            {
                Count = GameXender.Client.SendPacketToAllClients(Packet);
            }
            CLogger.Print($"Message sent to '{Count}' Players", LoggerType.Command);
        }
    }
}
