using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_USER_SUBTASK_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly int SessionId;
        public PROTOCOL_BASE_GET_USER_SUBTASK_ACK(PlayerSession Session)
        {
            Player = AccountManager.GetAccount(Session.PlayerId, true);
            SessionId = Session.SessionId;
        }
        public override void Write()
        {
            WriteH(656);
            WriteD(SessionId);
            WriteH(0);
            WriteC(0);
            WriteD(SessionId);
            WriteC(0);
        }
    }
}
