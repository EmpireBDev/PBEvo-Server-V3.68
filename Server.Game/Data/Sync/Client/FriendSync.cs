using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Data.Sync.Client
{
    public class FriendSync
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ();
            int Type = C.ReadC();
            long FriendId = C.ReadQ();
            int State;
            bool Removed;
            FriendModel F = null;
            if (Type <= 1)
            {
                State = C.ReadC();
                Removed = C.ReadC() == 1;
                FriendModel FM = new FriendModel(FriendId) 
                { 
                    State = State, 
                    Removed = Removed 
                };
                F = FM;
            }
            if (F == null && Type <= 1)
            {
                return;
            }
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player != null)
            {
                if (Type <= 1)
                {
                    F.Info.Nickname = Player.Nickname;
                    F.Info.Rank = Player.Rank;
                    F.Info.IsOnline = Player.IsOnline;
                    F.Info.Status = Player.Status;
                }
                if (Type == 0)
                {
                    Player.Friend.AddFriend(F);
                }
                else if (Type == 1)
                {
                    FriendModel MF = Player.Friend.GetFriend(FriendId);
                    if (MF != null)
                    {
                        MF = F;
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