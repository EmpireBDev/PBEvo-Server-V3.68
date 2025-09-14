using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_SELECT_CHANNEL_REQ : GameClientPacket
    {
        private int ChannelId;
        public PROTOCOL_BASE_SELECT_CHANNEL_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            ReadB(4); //?
            ChannelId = ReadH();
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null || Player.ChannelId >= 0)
                {
                    return;
                }
                ChannelModel Channel = ChannelsXML.GetChannel(Client.ServerId, ChannelId);
                if (Channel != null)
                {
                    if (AllUtils.ChannelRequirementCheck(Player, Channel))
                    {
                        Client.SendPacket(new PROTOCOL_BASE_SELECT_CHANNEL_ACK(0x80000202, -1, -1));
                    }
                    else if (Channel.Players.Count >= SChannelXML.GetServer(Client.ServerId).ChannelPlayers)
                    {
                        Client.SendPacket(new PROTOCOL_BASE_SELECT_CHANNEL_ACK(0x80000201, -1, -1));
                    }
                    else
                    {
                        Player.ServerId = Channel.ServerId;
                        Player.ChannelId = Channel.Id;
                        Client.SendPacket(new PROTOCOL_BASE_SELECT_CHANNEL_ACK(0, Player.ServerId, Player.ChannelId));
                        Player.Status.UpdateServer((byte)Player.ServerId);
                        Player.Status.UpdateChannel((byte)Player.ChannelId);
                        Player.UpdateCacheInfo();
                    }
                }
                else
                {
                    Client.SendPacket(new PROTOCOL_BASE_SELECT_CHANNEL_ACK(0x80000000, -1, -1));
                }
                /*
                 * 0x80000201 STBL_IDX_EP_SERVER_USER_FULL_C
                 * 0x80000202 - De acordo com o ChannelType
                 * 0x80000203 STBL_IDX_EP_SERVER_NOT_SATISFY_MTS
                 * 0x80000204 STR_UI_GOTO_GWARNET_CHANNEL_ERROR
                 * 0x80000205 STR_UI_GOTO_AZERBAIJAN_CHANNEL_ERROR
                 * 0x80000206 STR_POPUP_MOBILE_CERTIFICATION_ERROR
                 * 0x80000207 STR_UI_GOTO_TURKISH_CHANNEL_ERROR
                 * 0x80000208 STR_UI_GOTO_MENA_CHANNEL_ERROR
                 */
            }
            catch (Exception Ex)
            {
                CLogger.Print($"PROTOCOL_BASE_SELECT_CHANNEL_REQ: {Ex.Message}", LoggerType.Error, Ex);
            }
        }
    }
}