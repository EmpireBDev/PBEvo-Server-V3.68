using Plugin.Core.Network;
using Server.Auth.Data.Managers;
using Server.Auth.Data.Models;
using Server.Auth.Data.Sync.Update;

namespace Server.Auth.Data.Sync.Client
{
    public static class ClanSync
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ(), MemberId;
            int Type = C.ReadC();
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player == null)
            {
                return;
            }
            if (Type == 0)
            {
                ClanInfo.ClearList(Player);
            }
            else if (Type == 1)
            {
                MemberId = C.ReadQ();
                string Name = C.ReadS(C.ReadC());
                byte[] Status = C.ReadB(4);
                byte Rank = C.ReadC();
                Account Member = new Account()
                {
                    PlayerId = MemberId,
                    Nickname = Name,
                    Rank = Rank
                };
                Member.Status.SetData(Status, MemberId);
                ClanInfo.AddMember(Player, Member);
            }
            else if (Type == 2)
            {
                MemberId = C.ReadQ();
                ClanInfo.RemoveMember(Player, MemberId);
            }
            else if (Type == 3)
            {
                int ClanId = C.ReadD();
                int ClanAccess = C.ReadC();
                Player.ClanId = ClanId;
                Player.ClanAccess = ClanAccess;
            }
        }
    }
}