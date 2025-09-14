using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_USER_TITLE_RELEASE_REQ : GameClientPacket
    {
        private int SlotIdx, TitleId;
        public PROTOCOL_BASE_USER_TITLE_RELEASE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            SlotIdx = ReadC();
            TitleId = ReadC();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || SlotIdx >= 3 || Player.Title == null)
                {
                    return;
                }
                PlayerTitles Title = Player.Title;
                int titleId = Title.GetEquip(SlotIdx);
                if (SlotIdx < 3 && TitleId < 45 && titleId == TitleId && DaoManagerSQL.UpdateEquipedPlayerTitle(Title.OwnerId, SlotIdx, 0))
                {
                    Title.SetEquip(SlotIdx, 0);
                    if (TitleAwardXML.Contains(titleId, Player.Equipment.BeretItem) && ComDiv.UpdateDB("player_equipments", "beret_item_part", 0, "owner_id", Player.PlayerId))
                    {
                        Player.Equipment.BeretItem = 0;
                        RoomModel room = Player.Room;
                        if (room != null)
                        {
                            AllUtils.UpdateSlotEquips(Player, room);
                        }
                    }
                    Client.SendPacket(new PROTOCOL_BASE_USER_TITLE_RELEASE_ACK(0, SlotIdx, TitleId));
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_BASE_USER_TITLE_RELEASE_ACK(0x80000000, -1, -1));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_USER_TITLE_RELEASE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}