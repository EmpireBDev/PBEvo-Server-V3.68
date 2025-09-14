using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;

namespace Executable.UDP.Client
{
    public class MessageStatus
    {
        public static void Load(SyncClientPacket C)
        {
            byte MessageLength = C.ReadC();
            string Message = C.ReadS(MessageLength);
            if (!string.IsNullOrEmpty(Message) && MessageLength < 60)
            {
                CLogger.Print($"From Server Message: {Message}", LoggerType.Info);
            }
        }
    }
}
