using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Managers.Events;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_PLAYTIME_REWARD_REQ : GameClientPacket
    {
        private int goodId;
        public PROTOCOL_BASE_PLAYTIME_REWARD_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            goodId = ReadD();
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
                PlayerEvent PEV = Player.Event;
                GoodsItem Good = ShopManager.GetGood(goodId);
                if (Good == null || PEV == null)
                {
                    return;
                }
                EventPlaytimeModel EventPt = EventPlaytimeSync.GetRunningEvent();
                if (EventPt != null)
                {
                    if (PEV.LastPlaytimeFinish == 1 && ComDiv.UpdateDB("player_events", "last_playtime_finish", 2, "owner_id", Client.PlayerId))
                    {
                        PEV.LastPlaytimeFinish = 2;
                        Client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, Player, new ItemsModel(Good.Item)));
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BASE_PLAYTIME_REWARD_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}