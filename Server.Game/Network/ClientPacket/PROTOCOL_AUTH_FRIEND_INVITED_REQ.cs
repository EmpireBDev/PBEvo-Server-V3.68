using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_FRIEND_INVITED_REQ : GameClientPacket
    {
        private string Nickname;
        public PROTOCOL_AUTH_FRIEND_INVITED_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Nickname = ReadU(66);
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
                if (Player.Nickname.Length == 0 || Player.Nickname == Nickname)
                {
                    Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INVITED_ACK(0x80001037));
                }
                else if (Player.Friend.Friends.Count >= 50)
                {
                    Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INVITED_ACK(0x80001038));
                }
                else
                {
                    Account Friend = AccountManager.GetAccount(Nickname, 1, 287);
                    if (Friend != null)
                    {
                        if (Player.Friend.GetFriendIdx(Friend.PlayerId) == -1)
                        {
                            if (Friend.Friend.Friends.Count >= 50)
                            {
                                Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INVITED_ACK(0x80001038));
                            }
                            else
                            {
                                int Error = AllUtils.AddFriend(Friend, Player, 2);
                                if (AllUtils.AddFriend(Player, Friend, (Error == 1 ? 0 : 1)) == -1 || Error == -1)
                                {
                                    Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INVITED_ACK(0x80001039));
                                    return;
                                }
                                FriendModel FriendUser = Friend.Friend.GetFriend(Player.PlayerId, out int idx2);
                                if (FriendUser != null)
                                {
                                    Friend.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(Error == 0 ? FriendChangeState.Insert : FriendChangeState.Update, FriendUser, idx2), false);
                                }
                                FriendModel MemberUser = Player.Friend.GetFriend(Friend.PlayerId, out int idx1);
                                if (MemberUser != null)
                                {
                                    Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState.Insert, MemberUser, idx1));
                                }
                            }
                        }
                        else
                        {
                            Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INVITED_ACK(0x80001041));
                        }
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_AUTH_FRIEND_INVITED_ACK(0x80001042));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_AUTH_FRIEND_INVITED_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}