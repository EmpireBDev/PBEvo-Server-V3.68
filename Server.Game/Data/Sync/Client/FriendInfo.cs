using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;

namespace Server.Game.Data.Sync.Client
{
    public class FriendInfo
    {
        public static void Load(SyncClientPacket C)
        {
            int Type = C.ReadC();
            int IsConnect = C.ReadC();
            long PlayerId = C.ReadQ();
            long FriendId = C.ReadQ();
            Account Player = AccountManager.GetAccount(PlayerId, 31);
            if (Player != null)
            {
                Account Friend = AccountManager.GetAccount(FriendId, true);
                if (Friend != null)
                {
                    FriendState State = IsConnect == 1 ? FriendState.Online : FriendState.Offline;
                    if (Type == 0)
                    {
                        int Index = -1;
                        FriendModel F = Friend.Friend.GetFriend(Player.PlayerId, out Index);
                        if (Index != -1 && F != null && F.State == 0)
                        {
                            Friend.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Update, F, State, Index));
                        }
                    }
                    else
                    {
                        Friend.SendPacket(new PROTOCOL_CS_MEMBER_INFO_CHANGE_ACK(Player, State));
                    }
                }
            }
        }
    }
}
