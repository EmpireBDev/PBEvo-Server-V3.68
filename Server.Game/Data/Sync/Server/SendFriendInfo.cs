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
    public class SendFriendInfo
    {
        public static void Load(Account Player, FriendModel Friend, int Type)
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
                    S.WriteH(17);
                    S.WriteQ(Player.PlayerId);
                    S.WriteC((byte)Type);
                    S.WriteQ(Friend.PlayerId);
                    if (Type != 2)
                    {
                        S.WriteC((byte)Friend.State);
                        S.WriteC((byte)(Friend.Removed ? 1 : 0));
                    }
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