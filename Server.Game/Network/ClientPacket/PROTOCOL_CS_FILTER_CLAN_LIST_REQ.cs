using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_FILTER_CLAN_LIST_REQ : GameClientPacket
    {
        private uint Page;
        public PROTOCOL_CS_FILTER_CLAN_LIST_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Page = ReadUD();
        }
        public override void Run()
        {
            try
            {
                int Count = 0;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    lock (ClanManager.Clans)
                    {
                        for (int i = (int)Page; i < ClanManager.Clans.Count; i++)
                        {
                            ClanModel Clan = ClanManager.Clans[i];
                            WriteData(Clan, S);
                            if (++Count == 15)
                            {
                                break;
                            }
                        }
                    }
                    Client.SendPacket(new PROTOCOL_CS_FILTER_CLAN_LIST_ACK(Page, Count, S.ToArray()));
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void WriteData(ClanModel Clan, SyncServerPacket S)
        {
            S.WriteD(Clan.Id);
            S.WriteC((byte)Clan.Name.Length);
            S.WriteU(Clan.Name, Clan.Name.Length * 2);
            S.WriteD(Clan.Logo);
            S.WriteH((ushort)Clan.Info.Length);
            S.WriteU(Clan.Info, Clan.Info.Length * 2);
            S.WriteC((byte)Clan.Rank);
        }
    }
}