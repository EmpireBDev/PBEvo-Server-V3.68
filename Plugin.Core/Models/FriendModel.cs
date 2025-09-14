namespace Plugin.Core.Models
{
    public class FriendModel
    {
        public long ObjectId;
        public long OwnerId;
        public long PlayerId;
        public int State;
        public bool Removed;
        public PlayerInfo Info;
        public FriendModel(long PlayerId)
        {
            this.PlayerId = PlayerId;
            Info = new PlayerInfo(PlayerId);
        }
        public FriendModel(long PlayerId, int Rank, int NickColor, string Nickname, bool IsOnline, AccountStatus Status)
        {
            this.PlayerId = PlayerId;
            SetModel(PlayerId, Rank, NickColor, Nickname, IsOnline, Status);
        }
        public void SetModel(long PlayerId, int Rank, int NickColor, string Nickname, bool IsOnline, AccountStatus Status)
        {
            Info = new PlayerInfo(PlayerId, Rank, NickColor, Nickname, IsOnline, Status);
        }
    }
}