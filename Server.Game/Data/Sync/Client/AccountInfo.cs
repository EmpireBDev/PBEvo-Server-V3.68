using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;

namespace Server.Game.Data.Sync.Client
{
    public class AccountInfo
    {
        public static void Load(SyncClientPacket C)
        {
            long PlayerId = C.ReadQ();
            int Type = C.ReadC();
            string PacketName = C.ReadS(C.ReadC());
            byte[] Data = C.ReadB(C.ReadUH());
            Account Player = AccountManager.GetAccount(PlayerId, true);
            if (Player != null)
            {
                if (Type == 0)
                {
                    Player.SendPacket(Data, PacketName);
                }
                else
                {
                    Player.SendCompletePacket(Data, PacketName);
                }
            }
        }
    }
}
