using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Plugin.Core.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK : GameServerPacket
    {
        private readonly FriendModel Friend;
        private readonly int Index;
        private readonly FriendState State;
        private readonly FriendChangeState Type;
        public PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState Type, FriendModel Friend, int Index)
        {
            this.Type = Type;
            State = (FriendState)Friend.State;
            this.Friend = Friend;
            this.Index = Index;
        }
        public PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState Type, FriendModel Friend, int State, int Index)
        {
            this.Type = Type;
            this.State = (FriendState)State;
            this.Friend = Friend;
            this.Index = Index;
        }
        public PROTOCOL_AUTH_FRIEND_INFO_CHANGE_ACK(FriendChangeState Type, FriendModel Friend, FriendState State, int Index)
        {
            this.Type = Type;
            this.State = State;
            this.Friend = Friend;
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
