using Plugin.Core.Utility;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_MATCH_CLAN_SEASON_ACK : AuthServerPacket
    {
        private readonly int EventEnable;
        public PROTOCOL_MATCH_CLAN_SEASON_ACK(int EventEnable)
        {
            this.EventEnable = EventEnable;
        }
        public override void Write()
        {
            WriteH(7700);
            WriteH(0);
            WriteD(2);//servers count?
            WriteB(ComDiv.AddressBytes("127.0.0.1")); //ip?
            WriteB(ComDiv.AddressBytes("255.255.255.255")); //mask?
            WriteB(new byte[109]);
            WriteB(ComDiv.AddressBytes("127.0.0.1")); //ip?
            WriteB(ComDiv.AddressBytes("127.0.0.1")); //ip?
            WriteB(ComDiv.AddressBytes("255.255.255.255")); //mask?
            WriteB(new byte[103]);
        }
    }
}