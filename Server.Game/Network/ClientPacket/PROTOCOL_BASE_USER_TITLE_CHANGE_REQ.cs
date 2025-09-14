using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_USER_TITLE_CHANGE_REQ : GameClientPacket
    {
        private int TitleIdx;
        private uint Error;
        public PROTOCOL_BASE_USER_TITLE_CHANGE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            TitleIdx = ReadC();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || TitleIdx >= 45)
                {
                    return;
                }
                if (Player.Title.OwnerId == 0)
                {
                    DaoManagerSQL.CreatePlayerTitlesDB(Player.PlayerId);
                    Player.Title = new PlayerTitles { OwnerId = Player.PlayerId };
                }
                TitleModel t1 = TitleSystemXML.GetTitle(TitleIdx);
                if (t1 != null)
                {
                    TitleSystemXML.Get2Titles(t1.Req1, t1.Req2, out TitleModel tr1, out TitleModel tr2, false);
                    if ((t1.Req1 == 0 || tr1 != null) && (t1.Req2 == 0 || tr2 != null) && Player.Rank >= t1.Rank && Player.Ribbon >= t1.Ribbon && Player.Medal >= t1.Medal && Player.MasterMedal >= t1.MasterMedal && Player.Ensign >= t1.Ensign && !Player.Title.Contains(t1.Flag) && Player.Title.Contains(tr1.Flag) && Player.Title.Contains(tr2.Flag))
                    {
                        Player.Ribbon -= t1.Ribbon;
                        Player.Medal -= t1.Medal;
                        Player.MasterMedal -= t1.MasterMedal;
                        Player.Ensign -= t1.Ensign;
                        long flags = Player.Title.Add(t1.Flag);
                        DaoManagerSQL.UpdatePlayerTitlesFlags(Player.PlayerId, flags);
                        List<ItemsModel> Items = TitleAwardXML.GetAwards(TitleIdx);
                        if (Items.Count > 0)
                        {
                            foreach (ItemsModel Item in Items)
                            {
                                Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, Item));
                            }
                        }
                        Client.SendPacket(new PROTOCOL_BASE_MEDAL_GET_INFO_ACK(Player));
                        DaoManagerSQL.UpdatePlayerTitleRequi(Player.PlayerId, Player.Medal, Player.Ensign, Player.MasterMedal, Player.Ribbon);
                        if (Player.Title.Slots < t1.Slot)
                        {
                            Player.Title.Slots = t1.Slot;
                            ComDiv.UpdateDB("player_titles", "slots", Player.Title.Slots, "owner_id", Player.PlayerId);
                        }
                    }
                    else
                    {
                        Error = 0x80001083;
                    }
                }
                else
                {
                    Error = 0x80001083;
                }
                Client.SendPacket(new PROTOCOL_BASE_USER_TITLE_CHANGE_ACK(Error, Player.Title.Slots));
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_USER_TITLE_CHANGE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}