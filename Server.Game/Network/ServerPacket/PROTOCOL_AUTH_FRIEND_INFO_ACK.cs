using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_FRIEND_INFO_ACK : GameServerPacket
    {
        private readonly List<FriendModel> Friends;
        public PROTOCOL_AUTH_FRIEND_INFO_ACK(List<FriendModel> Friends)
        {
            this.Friends = Friends;
        }
        public override void Write()
        {
            WriteH(786);
            WriteC((byte)Friends.Count);
            foreach (FriendModel Friend in Friends)
            {
                PlayerInfo Info = Friend.Info;
                if (Info == null)
                {
                    WriteB(new byte[24]);
                }
                else
                {
                    WriteC((byte)(Info.Nickname.Length + 1));
                    WriteN(Info.Nickname, Info.Nickname.Length + 2, "UTF-16LE");
                    WriteQ(Friend.PlayerId);
                    WriteD(ComDiv.GetFriendStatus(Friend));
                    WriteD(uint.MaxValue);
                    WriteC((byte)Info.Rank);
                    WriteB(new byte[6]);
                }
            }
        }
    }
}