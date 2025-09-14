using Plugin.Core.Managers.Events;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using System;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_ACK : GameServerPacket
    {
        private readonly Account Player;
        public PROTOCOL_ROOM_GET_ACEMODE_PLAYERINFO_ACK(Account Player)
        {
            this.Player = Player;
        }
        public override void Write()
        {
            ClanModel Clan = ClanManager.GetClan(Player.ClanId);
            WriteH(3937);
            WriteH(0);
            WriteD((uint)Player.PlayerId);

            //ACE statistics
            WriteD(102); //fights
            WriteD(50); //wins
            WriteD(50); //loses
            WriteD(50); //kills
            WriteD(50); //deaths
            WriteD(50); //headshots
            WriteD(0); //unk (in theory this is assists, but in ace mode it not used)
            WriteD(2); //escapes
            WriteD(50); //several wins in a row

            WriteB(new byte[122]);

            WriteU(Player.Nickname, 66);
            WriteD(Player.Rank);
            WriteD(Player.GetRank());
            WriteD(0); //unk
            WriteD(0); //unk
            WriteD(Player.Gold);
            WriteD(Player.Exp);
            WriteD(Player.Tags);
            WriteC(0); //unk
            WriteD(0); //unk
            WriteQ(0); //unk
            WriteC(0); //unk
            WriteC(0); //unk
            WriteD(Player.Cash);
            WriteD(Player.ClanId);
            WriteD(Player.ClanAccess);
            WriteQ(0); //unk
            WriteC((byte)Player.CafePC);
            WriteC((byte)Player.TourneyLevel);
            WriteU(Clan.Name, 34);
            WriteC((byte)Clan.Rank);
            WriteC((byte)Clan.GetClanUnit());
            WriteD(Clan.Logo);
            WriteC(0); //unk
            WriteC((byte)Clan.Effect);
            WriteC((byte)Player.Age);
        }
    }
}
