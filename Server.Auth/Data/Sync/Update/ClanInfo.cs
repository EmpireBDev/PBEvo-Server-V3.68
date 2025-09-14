using Server.Auth.Data.Models;

namespace Server.Auth.Data.Sync.Update
{
    public class ClanInfo
    {
        public static void AddMember(Account Player, Account Member)
        {
            lock (Player.ClanPlayers)
            {
                Player.ClanPlayers.Add(Member);
            }
        }
        public static void RemoveMember(Account Player, long PlayerId)
        {
            lock (Player.ClanPlayers)
            {
                for (int i = 0; i < Player.ClanPlayers.Count; i++)
                {
                    Account pC = Player.ClanPlayers[i];
                    if (pC.PlayerId == PlayerId)
                    {
                        Player.ClanPlayers.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public static void ClearList(Account Player)
        {
            lock (Player.ClanPlayers)
            {
                Player.ClanId = 0;
                Player.ClanAccess = 0;
                Player.ClanPlayers.Clear();
            }
        }
    }
}