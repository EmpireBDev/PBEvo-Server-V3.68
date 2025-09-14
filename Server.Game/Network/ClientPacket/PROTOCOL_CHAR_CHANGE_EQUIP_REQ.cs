using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CHAR_CHANGE_EQUIP_REQ : GameClientPacket
    {
        private readonly int[] EquipmentList = new int[14];
        private int AccessoryId, CharaPos, FR_SLOT, CT_SLOT;
        private bool AccesoryChange, CouponChange, EquipmentChange, ItemChange;
        private readonly SortedList<int, int> Coupons = new SortedList<int, int>(), Items = new SortedList<int, int>();
        public PROTOCOL_CHAR_CHANGE_EQUIP_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            AccessoryId = ReadD();
            ReadUD();
            AccesoryChange = (ReadC() == 1);
            ReadC();
            ReadH();
            int CouponCount = ReadC();
            for (int i = 0; i < CouponCount; i++)
            {
                int CouponId = ReadD();
                Coupons.Add(i, CouponId);
            }
            CouponChange = (ReadC() == 1);
            ReadH();
            EquipmentList[0] = ReadD();
            ReadUD();
            EquipmentList[1] = ReadD();
            ReadUD();
            EquipmentList[2] = ReadD();
            ReadUD();
            EquipmentList[3] = ReadD();
            ReadUD();
            EquipmentList[4] = ReadD();
            ReadUD();
            CharaPos = ReadD();
            ReadUD();
            EquipmentList[5] = ReadD();
            ReadUD();
            EquipmentList[6] = ReadD();
            ReadUD();
            EquipmentList[7] = ReadD();
            ReadUD();
            EquipmentList[8] = ReadD();
            ReadUD();
            EquipmentList[9] = ReadD();
            ReadUD();
            EquipmentList[10] = ReadD();
            ReadUD();
            EquipmentList[11] = ReadD();
            ReadUD();
            EquipmentList[12] = ReadD();
            ReadUD();
            EquipmentList[13] = ReadD();
            ReadUD();
            EquipmentChange = (ReadC() == 1);
            int ItemCount = ReadC();
            for (int i = 0; i < ItemCount; i++)
            {
                int ItemId = ReadD();
                ReadUD();
                Items.Add(i, ItemId);
            }
            ItemChange = (ReadC() == 1);
            ReadC();
            FR_SLOT = ReadC();
            CT_SLOT = ReadC();
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
                int CharacterCount = Player.Character.Characters.Count;
                if (CharacterCount > 0)
                {
                    if (AccesoryChange)
                    {
                        AllUtils.ValidateAccesoryEquipment(Player, AccessoryId);
                    }
                    if (EquipmentChange)
                    {
                        AllUtils.ValidateCharacterEquipment(Player, Player.Equipment, EquipmentList, CharaPos, FR_SLOT, CT_SLOT);
                    }
                    if (CouponChange)
                    {
                        AllUtils.ValidateCouponEquipment(Player, Coupons);
                    }
                    if (ItemChange)
                    {
                        AllUtils.ValidateItemEquipment(Player, Items);
                    }
                    AllUtils.ValidateCharacterSlot(Player, Player.Equipment, FR_SLOT, CT_SLOT);
                }
                RoomModel Room = Player.Room;
                if (Room != null)
                {
                    AllUtils.UpdateSlotEquips(Player, Room);
                }
                Client.SendPacket(new PROTOCOL_CHAR_CHANGE_EQUIP_ACK(0));
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}
