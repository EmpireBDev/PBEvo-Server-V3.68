using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Models;
using Server.Auth.Network.ServerPacket;

namespace Server.Auth.Data.Sync.Client
{
    public class FriendInfo
    {
        public static void Load(SyncClientPacket C)
        {
            int Type = C.ReadC();
            int IsConnect = C.ReadC();
            long PlayerId = C.ReadQ();
            long FriendId = C.ReadQ();
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player != null)
            {
                Account Friend = AccountManager.GetAccount(FriendId, true);
                if (Friend != null)
                {
                    FriendState State = IsConnect == 1 ? FriendState.Online : FriendState.Offline;
                    if (Type == 0)
                    {
                        int Index = -1;
                        FriendModel FM = Friend.Friend.GetFriend(Player.PlayerId, out Index);
                        if (Index != -1 && FM != null)
                        {
                            Friend.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Update, FM, State, Index));
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
