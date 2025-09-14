using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ : GameClientPacket
    {
        private readonly List<int> Messages = new List<int>();
        public PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            int Count = ReadC();
            for (int i = 0; i < Count; i++)
            {
                Messages.Add(ReadD());
            }
        }
        public override void Run()
        {
            try
            {
                if (ComDiv.UpdateDB("player_messages", "object_id", Messages.ToArray(), "owner_id", Client.PlayerId, new string[] { "expire_date", "state" }, long.Parse(DateTimeUtil.Now().AddDays(7).ToString("yyMMddHHmm")), 0))
                {
                    Client.SendPacket(new PROTOCOL_MESSENGER_NOTE_CHECK_READED_ACK(Messages));
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_MESSENGER_NOTE_CHECK_READED_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}