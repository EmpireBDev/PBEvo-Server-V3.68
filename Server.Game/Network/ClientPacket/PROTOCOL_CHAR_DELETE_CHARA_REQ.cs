using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CHAR_DELETE_CHARA_REQ : GameClientPacket
    {
        private int Slot;
        public PROTOCOL_CHAR_DELETE_CHARA_REQ(GameClient Client, byte[] Buffer)
        {
            Makeme(Client, Buffer);
        }
        public override void Read()
        {
            Slot = ReadC();
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
                CharacterModel Character = Player.Character.GetCharacterSlot(Slot);
                if (Character != null)
                {
                    ItemsModel Item = Player.Inventory.GetItem(Character.Id);
                    if (Item != null)
                    {
                        int Index = 0;
                        foreach (CharacterModel Chara in Player.Character.Characters)
                        {
                            if (Chara.Slot != Character.Slot)
                            {
                                Chara.Slot = Index;
                                DaoManagerSQL.UpdatePlayerCharacter(Index, Chara.ObjectId, Player.PlayerId);
                                Index++;
                            }
                        }
                        Client.SendPacket(new PROTOCOL_CHAR_DELETE_CHARA_ACK(0, Slot, Player, Item));
                        if (DaoManagerSQL.DeletePlayerCharacter(Character.ObjectId, Player.PlayerId))
                        {
                            Player.Character.RemoveCharacter(Character);
                        }
                        if (DaoManagerSQL.DeletePlayerInventoryItem(Item.ObjectId, Player.PlayerId))
                        {
                            Player.Inventory.RemoveItem(Item);
                        }
                    }
                    else
                    {
                        Client.SendPacket(new PROTOCOL_CHAR_DELETE_CHARA_ACK(0x800010A7, -1, null, null));
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_CHAR_DELETE_CHARA_ACK(0x800010A7, -1, null, null));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CHAR_DELETE_CHARA_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}
