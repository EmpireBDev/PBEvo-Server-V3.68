using Plugin.Core.Enums;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_START_COUNTDOWN_ACK : GameServerPacket
    {
        private readonly CountDownEnum type;
        public PROTOCOL_BATTLE_START_COUNTDOWN_ACK(CountDownEnum timer)
        {
            type = timer;
        }
        public override void Write()
        {
            WriteH(4102);
            WriteC((byte)type);
        }
    }
}