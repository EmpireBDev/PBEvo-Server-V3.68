using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;
using Server.Game.Data.Sync.Server;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_FRIEND_DELETE_REQ : GameClientPacket
    {
        private int index;
        private uint erro;
        public PROTOCOL_AUTH_FRIEND_DELETE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            index = ReadC();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                FriendModel f = Player.Friend.GetFriend(index);
                if (f != null)
                {
                    DaoManagerSQL.DeletePlayerFriend(f.PlayerId, Player.PlayerId);
                    Account Member = AccountManager.GetAccount(f.PlayerId, 287);
                    if (Member != null)
                    {
                        int idx = -1;
                        FriendModel FriendUser = Member.Friend.GetFriend(Player.PlayerId, out idx);
                        if (FriendUser != null)
                        {
                            FriendUser.Removed = true;
                            DaoManagerSQL.UpdatePlayerFriendBlock(Member.PlayerId, FriendUser);
                            SendFriendInfo.Load(Member, FriendUser, 2);
                            Member.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Update, FriendUser, idx), false);
                        }
                    }
                    Player.Friend.RemoveFriend(f);
                    Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Delete, null, 0, index));
                }
                else
                {
                    erro = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_AUTH_FRIEND_DELETE_ACK(erro));
                Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_ACK(Player.Friend.Friends));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_FRIEND_DELETE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
