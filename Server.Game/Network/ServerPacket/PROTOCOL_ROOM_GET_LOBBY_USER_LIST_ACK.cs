using Server.Game.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_LOBBY_USER_LIST_ACK : GameServerPacket
    {
        private readonly List<Account> players;
        private readonly List<int> playersIdxs;
        public PROTOCOL_ROOM_GET_LOBBY_USER_LIST_ACK(ChannelModel ch)
        {
            players = ch.GetWaitPlayers();
            playersIdxs = GetRandomIndexes(players.Count, players.Count >= 8 ? 8 : players.Count);
        }
        public override void Write()
        {
            WriteH(3674);
            WriteD(playersIdxs.Count);
            for (int i = 0; i < playersIdxs.Count; i++)
            {
                Account p = players[playersIdxs[i]];
                WriteD(p.GetSessionId());
                WriteD(p.GetRank());
                WriteC((byte)(p.Nickname.Length + 1));
                WriteN(p.Nickname, p.Nickname.Length + 2, "UTF-16LE");
                WriteC((byte)p.NickColor);
            }
        }
        private List<int> GetRandomIndexes(int total, int count)
        {
            if (total == 0 || count == 0)
            {
                return new List<int>();
            }
            Random rnd = new Random();
            List<int> numeros = new List<int>();
            for (int i = 0; i < total; i++)
            {
                numeros.Add(i);
            }
            for (int i = 0; i < numeros.Count; i++)
            {
                int a = rnd.Next(numeros.Count);
                int temp = numeros[i];
                numeros[i] = numeros[a];
                numeros[a] = temp;
            }
            return numeros.GetRange(0, count);
        }
    }
}
