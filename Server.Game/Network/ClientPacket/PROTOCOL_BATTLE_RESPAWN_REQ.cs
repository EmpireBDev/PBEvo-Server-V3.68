using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_RESPAWN_REQ : GameClientPacket
    {
        private int[] ItemIds;
        private int WeaponsFlag;
        public PROTOCOL_BATTLE_RESPAWN_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            ItemIds = new int[16];
            ItemIds[0] = ReadD();
            ReadUD();
            ItemIds[1] = ReadD();
            ReadUD();
            ItemIds[2] = ReadD();
            ReadUD();
            ItemIds[3] = ReadD();
            ReadUD();
            ItemIds[4] = ReadD();
            ReadUD();
            ItemIds[5] = ReadD();
            ReadUD();
            ItemIds[6] = ReadD();
            ReadUD();
            ItemIds[7] = ReadD();
            ReadUD();
            ItemIds[8] = ReadD();
            ReadUD();
            ItemIds[9] = ReadD();
            ReadUD();
            ItemIds[10] = ReadD();
            ReadUD();
            ItemIds[11] = ReadD();
            ReadUD();
            ItemIds[12] = ReadD();
            ReadUD();
            ItemIds[13] = ReadD();
            ReadUD();
            ItemIds[14] = ReadD();
            ReadUD();
            WeaponsFlag = ReadH();
            ItemIds[15] = ReadD();
            ReadUD();
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
                RoomModel Room = Player.Room;
                if (Room != null && Room.State == RoomState.Battle)
                {
                    SlotModel Slot = Room.GetSlot(Player.SlotId);
                    if (Slot != null && Slot.State == SlotState.BATTLE)
                    {
                        if (Slot.DeathState.HasFlag(DeadEnum.Dead) || Slot.DeathState.HasFlag(DeadEnum.UseChat))
                        {
                            Slot.DeathState = DeadEnum.Alive;
                        }
                        PlayerEquipment Equip = AllUtils.ValidateRespawnEQ(Player, Room, Slot, ItemIds);
                        if (Equip != null)
                        {
                            ComDiv.CheckEquipedItems(Equip, Player.Inventory.Items, true);
                            AllUtils.CheckEquipment(Room, Equip);
                            Slot.Equipment = Equip;
                            if ((WeaponsFlag & 8) > 0)
                            {
                                AllUtils.InsertItem(Equip.WeaponPrimary, Slot);
                            }
                            if ((WeaponsFlag & 4) > 0)
                            {
                                AllUtils.InsertItem(Equip.WeaponSecondary, Slot);
                            }
                            if ((WeaponsFlag & 2) > 0)
                            {
                                AllUtils.InsertItem(Equip.WeaponMelee, Slot);
                            }
                            if ((WeaponsFlag & 1) > 0)
                            {
                                AllUtils.InsertItem(Equip.WeaponExplosive, Slot);
                            }
                            AllUtils.InsertItem(Equip.WeaponSpecial, Slot);
                            AllUtils.InsertItem(Slot.Team == TeamEnum.FR_TEAM ? Equip.CharaRedId : Equip.CharaBlueId, Slot);
                            AllUtils.InsertItem(Equip.PartHead, Slot);
                            AllUtils.InsertItem(Equip.PartFace, Slot);
                            AllUtils.InsertItem(Equip.BeretItem, Slot);
                            AllUtils.InsertItem(Equip.DinoItem, Slot);
                            AllUtils.InsertItem(Equip.AccessoryId, Slot);
                        }
                        using (PROTOCOL_BATTLE_RESPAWN_ACK packet = new PROTOCOL_BATTLE_RESPAWN_ACK(Room, Slot))
                        {
                            Room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                        }
                        if (Slot.FirstRespawn)
                        {
                            Slot.FirstRespawn = false;
                            EquipmentSync.SendUDPPlayerSync(Room, Slot, Player.Effects, 0);
                        }
                        else
                        {
                            EquipmentSync.SendUDPPlayerSync(Room, Slot, Player.Effects, 2);
                        }
                        if (Room.WeaponsFlag != (RoomWeaponsFlag)WeaponsFlag)
                        {
                            CLogger.Print($"Player '{Player.Nickname}' Weapon Flags Doesn't Match! (Room: {(int)Room.WeaponsFlag}; Player: {WeaponsFlag})", LoggerType.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_RESPAWN_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}