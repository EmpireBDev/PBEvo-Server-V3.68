using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Managers;
using Server.Game.Network.ServerPacket;
using System;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Server.Game.Data.Models;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CLIENT_ENTER_REQ : GameClientPacket
    {
        private int ClanId;
        public PROTOCOL_CS_CLIENT_ENTER_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
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
                if (Room != null)
                {
                    Room.ChangeSlotState(Player.SlotId, SlotState.CLAN, false);
                    Room.StopCountDown(Player.SlotId);
                    Room.UpdateSlotsInfo();
                }
                ClanModel Clan = ClanManager.GetClan(Player.ClanId);
                if (Player.ClanId == 0 && Player.Nickname.Length > 0)
                {
                    ClanId = DaoManagerSQL.GetRequestClanId(Player.PlayerId);
                }
                Client.SendPacket(new PROTOCOL_CS_CLIENT_ENTER_ACK(ClanId > 0 ? ClanId : Clan.Id, Player.ClanAccess));
                if (Clan.Id > 0 && ClanId == 0)
                {
                    Client.SendPacket(new PROTOCOL_CS_DETAIL_INFO_ACK(0, Clan));
                }
                Client.SendPacket(new PROTOCOL_CS_MEDAL_INFO_ACK());
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_CS_CLIENT_ENTER_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}