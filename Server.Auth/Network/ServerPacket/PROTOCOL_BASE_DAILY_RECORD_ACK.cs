using Plugin.Core.Models;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_DAILY_RECORD_ACK : AuthServerPacket
    {
        private readonly StatDaily Dailies;
        public PROTOCOL_BASE_DAILY_RECORD_ACK(StatDaily Dailies)
        {
            this.Dailies = Dailies;
        }
        public override void Write()
        {
            WriteH(623);
            WriteH((ushort)Dailies.Matches);
            WriteH((ushort)Dailies.MatchWins);
            WriteH((ushort)Dailies.MatchLoses);
            WriteH((ushort)Dailies.MatchDraws);
            WriteH((ushort)Dailies.KillsCount);
            WriteH((ushort)Dailies.HeadshotsCount);
            WriteH((ushort)Dailies.DeathsCount);
            WriteD(Dailies.ExpGained);
            WriteD(Dailies.PointGained);
            WriteD(0); // Play Time // 0
            WriteC(1); // 0
            WriteD(0); // ??? Play Time ??? //
            WriteH(1); // ?
            WriteD(0); // ?
            WriteD(0); // ?
            WriteD(0); // ?
            WriteC(0); // ?
        }
    }
}
