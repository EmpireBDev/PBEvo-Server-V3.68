using Plugin.Core;
using Plugin.Core.Enums;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_LEAVE_REQ : GameClientPacket
    {
        private uint Error;
        public PROTOCOL_LOBBY_LEAVE_REQ(GameClient client, byte[] data)
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
                ChannelModel Channel = Player.GetChannel();
                if (Player.Room != null || Player.Match != null)
                {
                    return;
                }
                if (Channel == null || Player.Session == null || !Channel.RemovePlayer(Player))
                {
                    Error = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_LOBBY_LEAVE_ACK(Error));
                if (Error == 0)
                {
                    Player.ResetPages();
                    Player.Status.UpdateChannel(255);
                    AllUtils.SyncPlayerToFriends(Player, false);
                    AllUtils.SyncPlayerToClanMembers(Player);
                }
                else
                {
                    Client.Close(1000, true);
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_LOBBY_LEAVE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}