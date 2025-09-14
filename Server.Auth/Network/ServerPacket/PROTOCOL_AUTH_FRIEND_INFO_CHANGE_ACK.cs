using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK : AuthServerPacket
    {
        private readonly FriendModel Friend;
        private readonly int Index;
        private readonly FriendState State;
        private readonly FriendChangeState Type;
        public PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState Type, FriendModel Friend, FriendState State, int Index)
        {
            this.State = State;
            this.Friend = Friend;
            this.Type = Type;
            this.Index = Index;
        }
        public override void Write()
        {
            WriteH(791);
            WriteC((byte)Type);
            WriteC((byte)Index);
            if (Type == FriendChangeState.Insert || Type == FriendChangeState.Update)
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
                    WriteD(ComDiv.GetFriendStatus(Friend, State));
                    WriteD(uint.MaxValue);
                    WriteC((byte)Info.Rank);
                    WriteB(new byte[6]);
                }
            }
            else
            {
                WriteB(new byte[24]);
            }
        }
    }
}