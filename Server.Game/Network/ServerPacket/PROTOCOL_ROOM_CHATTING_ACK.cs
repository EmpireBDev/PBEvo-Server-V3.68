using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHATTING_ACK : GameServerPacket
    {
        private readonly string Message;
        private readonly int Type, SlotId;
        private readonly bool GMColor;
        public PROTOCOL_ROOM_CHATTING_ACK(int Type, int SlotId, bool GMColor, string Message)
        {
            this.Type = Type;
            this.SlotId = SlotId;
            this.GMColor = GMColor;
            this.Message = Message;
        }
        public override void Write()
        {
            WriteH(3862);
            WriteH((short)Type);
            WriteD(SlotId);
            WriteC((byte)(GMColor ? 1 : 0));
            WriteD(Message.Length + 1);
            WriteN(Message, Message.Length + 2, "UTF-16LE");
        }
    }
}