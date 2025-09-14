using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_USER_TITLE_EQUIP_REQ : GameClientPacket
    {
        private byte SlotIdx, TitleId;
        public PROTOCOL_BASE_USER_TITLE_EQUIP_REQ(GameClient client, byte[] data)
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
                if (Player == null)
                {
                    return;
                }
                PlayerTitles Title = Player.Title;
                TitleModel titleQ = TitleSystemXML.GetTitle(TitleId);
                TitleSystemXML.Get3Titles(Title.Equiped1, Title.Equiped2, Title.Equiped3, out TitleModel eq1, out TitleModel eq2, out TitleModel eq3, false);
                if (SlotIdx >= 3 || TitleId >= 45 || Title == null || titleQ == null || titleQ.ClassId == eq1.ClassId && SlotIdx != 0 || titleQ.ClassId == eq2.ClassId && SlotIdx != 1 || titleQ.ClassId == eq3.ClassId && SlotIdx != 2 || !Title.Contains(titleQ.Flag) || Title.Equiped1 == TitleId || Title.Equiped2 == TitleId || Title.Equiped3 == TitleId)
                {
                    Client.SendPacket(new PROTOCOL_BASE_USER_TITLE_EQUIP_ACK(0x80000000, -1, -1));
                }
                else
                {
                    if (DaoManagerSQL.UpdateEquipedPlayerTitle(Title.OwnerId, SlotIdx, TitleId))
                    {
                        Title.SetEquip(SlotIdx, TitleId);
                        Client.SendPacket(new PROTOCOL_BASE_USER_TITLE_EQUIP_ACK(0, SlotIdx, TitleId));
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_BASE_USER_TITLE_EQUIP_ACK(0x80000000, -1, -1));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_USER_TITLE_EQUIP_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}