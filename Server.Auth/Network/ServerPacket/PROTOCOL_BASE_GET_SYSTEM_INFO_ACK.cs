using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Auth.Data.Models;
using Server.Auth.Data.XML;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_SYSTEM_INFO_ACK : AuthServerPacket
    {
        private readonly ServerConfig CFG;
        private readonly List<SChannelModel> Servers;
        private readonly List<RankModel> PlayerRanks;
        public PROTOCOL_BASE_GET_SYSTEM_INFO_ACK(ServerConfig CFG)
        {
            this.CFG = CFG;
            if (CFG != null)
            {
                Servers = SChannelXML.Servers;
                PlayerRanks = PlayerRankXML.Ranks;
            }
        }
        public override void Write()
        {
            WriteH(523);
            WriteH(0);
            WriteH(32);
            WriteD(0);
            WriteD(153699);
            WriteC(0);
            WriteC(1);
            WriteB(new byte[373]); //must be 373
            WriteC(5);
            WriteC(5);
            WriteC(4);
            WriteD(3500);
            WriteD(1450);
            WriteD(49);
            WriteD(1);
            WriteC(7);
            WriteC(1);
            WriteC(0);
            WriteC(23);
            WriteC(21);
            WriteC(15);
            WriteC((byte)CFG.Showroom);
            WriteB(new byte[3]);
            WriteB(new byte[46]);
            WriteC(5);
            WriteC(10);
            WriteB(new byte[229]);//GIFT_BUY_RANKING || CLAN_MATCH_SEASON_EXT
            WriteC(3); // Count Event Play Time // 3 NEWA05
            WriteD(600);
            WriteD(2400);
            WriteD(6000);
            WriteC((byte)(CFG.Missions ? 1 : 0)); //enable flag (can be 0)
            WriteH((ushort)MissionConfigXML.MissionPage1); //81 = E2
            WriteH((ushort)MissionConfigXML.MissionPage2); //92 = 1F >> hex E2 1F (ushort)
            WriteH((ushort)SECURITY_KEY);
            WriteB(ServerData(Servers));
            WriteC(1);
            WriteH((ushort)NATIONS);
            WriteH((short)CFG.ShopURL.Length);
            WriteS(CFG.ShopURL, CFG.ShopURL.Length);
            WriteB(PlayerRanksData(PlayerRanks));
            WriteC(0);
            WriteC(6);
        }
        private byte[] ServerData(List<SChannelModel> Servers)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)Servers.Count);
                foreach (SChannelModel Server in Servers)
                {
                    S.WriteD(Server.State ? 1 : 0);
                    S.WriteB(ComDiv.AddressBytes(Server.Host));
                    S.WriteB(ComDiv.AddressBytes(Server.Host));
                    S.WriteH(Server.Port);
                    S.WriteC((byte)Server.Type);
                    S.WriteH((ushort)Server.MaxPlayers);
                    S.WriteD(Server.LastPlayers);
                    if (Server.Id == 0)
                    {
                        S.WriteB(Bitwise.HexStringToByteArray("01 01 01 01 01 01 01 01 01 01 0E 00 00 00 00"));
                    }
                    else
                    {
                        List<ChannelModel> Channels = ChannelsXML.GetChannels(Server.Id);
                        foreach (ChannelModel Channel in Channels)
                        {
                            S.WriteC((byte)Channel.Type);
                        }
                        S.WriteC((byte)Server.Type);
                        S.WriteC(0); //Mobile
                        S.WriteH(0);
                    }
                }
                return S.ToArray();
            }
        }
        private byte[] PlayerRanksData(List<RankModel> PlayerRanks)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)PlayerRanks.Count);
                foreach (RankModel Rank in PlayerRanks)
                {
                    S.WriteC((byte)Rank.Id);
                    List<ItemsModel> RankRewards = PlayerRankXML.GetRewards(Rank.Id);
                    foreach (ItemsModel Item in RankRewards)
                    {
                        GoodsItem GoodsReward = ShopManager.GetItemId(Item.Id);
                        S.WriteD(GoodsReward == null ? 0 : GoodsReward.Id);
                    }
                    for (int i = RankRewards.Count; (4 - i) > 0; i++)
                    {
                        S.WriteD(0);
                    }
                }
                return S.ToArray();
            }
        }
    }
}
