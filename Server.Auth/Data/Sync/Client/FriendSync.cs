using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Models;

namespace Server.Auth.Data.Sync.Client
{
    public static class FriendSync
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ();
            int Type = C.ReadC();
            long FriendId = C.ReadQ();
            int State;
            bool Removed;
            FriendModel FM = null;
            if (Type <= 1)
            {
                State = C.ReadC();
                Removed = C.ReadC() == 1;
                FriendModel Friend = new FriendModel(FriendId) 
                { 
                    State = State, 
                    Removed = Removed 
                };
                FM = Friend;
            }
            if (FM == null && Type <= 1)
            {
                return;
            }
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player != null)
            {
                if (Type <= 1)
                {
                    FM.Info.Nickname = Player.Nickname;
                    FM.Info.Rank = Player.Rank;
                    FM.Info.IsOnline = Player.IsOnline;
                    FM.Info.Status = Player.Status;
                }
                if (Type == 0)
                {
                    Player.Friend.AddFriend(FM);
                }
                else if (Type == 1)
                {
                    FriendModel MyFriend = Player.Friend.GetFriend(FriendId);
                    if (MyFriend != null)
                    {
                        MyFriend = FM;
                    }
                }
                else if (Type == 2)
                {
                    Player.Friend.RemoveFriend(FriendId);
                }
            }
        }
    }
}