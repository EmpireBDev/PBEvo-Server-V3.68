using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHANGE_PASSWD_ACK : GameServerPacket
    {
        private readonly string Password;
        public PROTOCOL_ROOM_CHANGE_PASSWD_ACK(string Password)
        {
            this.Password = Password;
        }
        public override void Write()
        {
            WriteH(3859);
            WriteS(Password, 4);
        }
    }
}