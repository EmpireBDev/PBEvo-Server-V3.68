using Plugin.Core.Utility;

namespace Plugin.Core.Models
{
    public class PlayerInfo
    {
        public int Rank, NickColor;
        public long PlayerId;
        public string Nickname;
        public bool IsOnline;
        public AccountStatus Status;
        public PlayerInfo(long PlayerId)
        {
            this.PlayerId = PlayerId;
            Status = new AccountStatus();
        }
        public PlayerInfo(long PlayerId, int Rank, int NickColor, string Nickname, bool IsOnline, AccountStatus Status)
        {
            this.PlayerId = PlayerId;
            SetInfo(Rank, NickColor, Nickname, IsOnline, Status);
        }
        public void SetOnlineStatus(bool state)
        {
            if (IsOnline != state && ComDiv.UpdateDB("accounts", "online", state, "player_id", PlayerId))
            {
                IsOnline = state;
            }
        }
        public void SetInfo(int Rank, int NickColor, string Nickname, bool IsOnline, AccountStatus Status)
        {
            this.Rank = Rank;
            this.NickColor = NickColor;
            this.Nickname = Nickname;
            this.IsOnline = IsOnline;
            this.Status = Status;
        }
    }
}