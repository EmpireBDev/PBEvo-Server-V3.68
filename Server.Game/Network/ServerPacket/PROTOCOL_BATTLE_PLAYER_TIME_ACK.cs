using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_PLAYER_TIME_ACK : GameServerPacket
    {
        private readonly int Type;
        private readonly EventPlaytimeModel Event;
        public PROTOCOL_BATTLE_PLAYER_TIME_ACK(int Type, EventPlaytimeModel Event)
        {
            this.Type = Type;
            this.Event = Event;
        }
        public override void Write()
        {
            WriteH(3911);
            WriteC((byte)Type);
            WriteS(Event.Title, 30);
            WriteD(Event.GoodReward1);
            WriteD(Event.GoodReward2);
            WriteQ(Event.Time);
        }
    }
}
