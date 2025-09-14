using Server.Game.Data.Models;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_ACK : GameServerPacket
    {
        private readonly List<Account> Players;
        private readonly List<int> PlayersIdxs;
        public PROTOCOL_ROOM_INVITE_LOBBY_USER_LIST_ACK(ChannelModel Channel)
        {
            Players = Channel.GetWaitPlayers();
            PlayersIdxs = GetRandomIndexes(Players.Count, Players.Count >= 8 ? 8 : Players.Count);
        }
        public override void Write()
        {
            WriteH(3932);
            WriteD(PlayersIdxs.Count);
            foreach (int Index in PlayersIdxs)
            {
                Account Player = Players[Index];
                WriteD(Player.GetSessionId());
                WriteD(Player.GetRank());
                WriteC((byte)(Player.Nickname.Length + 1));
                WriteN(Player.Nickname, Player.Nickname.Length + 2, "UTF-16LE");
                WriteC((byte)Player.NickColor);
            }
        }
        private List<int> GetRandomIndexes(int Total, int Count)
        {
            if (Total == 0 || Count == 0)
            {
                return new List<int>();
            }
            Random RND = new Random();
            List<int> Numbers = new List<int>();
            for (int i = 0; i < Total; i++)
            {
                Numbers.Add(i);
            }
            for (int i = 0; i < Numbers.Count; i++)
            {
                int Index = RND.Next(Numbers.Count);
                int Temp = Numbers[i];
                Numbers[i] = Numbers[Index];
                Numbers[Index] = Temp;
            }
            return Numbers.GetRange(0, Count);
        }
    }
}