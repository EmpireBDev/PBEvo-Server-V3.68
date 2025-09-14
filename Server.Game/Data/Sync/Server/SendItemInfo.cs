using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using System;
using System.Net;

namespace Server.Game.Data.Sync.Server
{
    public class SendItemInfo
    {
        public static void LoadItem(Account Player, ItemsModel Item)
        {
            try
            {
                if (Player == null || Player.Status.ServerId == 0)
                {
                    return;
                }
                SChannelModel Server = GameXender.Sync.GetServer(Player.Status);
                if (Server == null)
                {
                    return;
                }
                IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(18);
                    S.WriteQ(Player.PlayerId);
                    S.WriteQ(Item.ObjectId);
                    S.WriteD(Item.Id);
                    S.WriteC((byte)Item.Equip);
                    S.WriteC((byte)Item.Category);
                    S.WriteD(Item.Count);
                    S.WriteC((byte)Item.Name.Length);
                    S.WriteS(Item.Name, Item.Name.Length);
                    GameXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void LoadGoldCash(Account Player)
        {
            try
            {
                if (Player == null)
                {
                    return;
                }
                SChannelModel Server = GameXender.Sync.GetServer(Player.Status);
                if (Server == null)
                {
                    return;
                }
                IPEndPoint Sync = SynchronizeXML.GetServer(Server.Port).Connection;
                using (SyncServerPacket S = new SyncServerPacket())
                {
                    S.WriteH(19);
                    S.WriteQ(Player.PlayerId);
                    S.WriteC(0);
                    S.WriteC((byte)Player.Rank);
                    S.WriteD(Player.Gold);
                    S.WriteD(Player.Cash);
                    S.WriteD(Player.Tags);
                    GameXender.Sync.SendPacket(S.ToArray(), Sync);
                }
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
    }
}