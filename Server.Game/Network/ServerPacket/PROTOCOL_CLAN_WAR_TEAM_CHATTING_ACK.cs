using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CLAN_WAR_TEAM_CHATTING_ACK : GameServerPacket
    {
        private readonly int type, bantime;
        private readonly string message, sender;
        public PROTOCOL_CLAN_WAR_TEAM_CHATTING_ACK(string sender, string message)
        {
            this.sender = sender;
            this.message = message;
        }
        public PROTOCOL_CLAN_WAR_TEAM_CHATTING_ACK(int type, int bantime)
        {
            this.type = type;
            this.bantime = bantime;
        }
        public override void Write()
        {
            WriteH(1577);
            WriteC((byte)type);
            if (type == 0)
            {
                WriteC((byte)(sender.Length + 1));
                WriteN(sender, sender.Length + 2, "UTF-16LE");
                WriteC((byte)(message.Length + 1));
                WriteN(message, message.Length + 2, "UTF-16LE");
            }
            else
            {
                WriteD(bantime);
            }
            /*
             * 1=STR_MESSAGE_BLOCK_ING
             * 2=STR_MESSAGE_BLOCK_START
             */
        }
    }
}