using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.SQL;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_MESSENGER_NOTE_DELETE_REQ : GameClientPacket
    {
        private uint Error;
        private List<object> Objects = new List<object>();
        public PROTOCOL_MESSENGER_NOTE_DELETE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            int Count = ReadC();
            for (int i = 0; i < Count; i++)
            {
                long ObjectId = ReadUD();
                Objects.Add(ObjectId);
            }
        }
        public override void Run()
        {
            try
            {
                if (!DaoManagerSQL.DeleteMessages(Objects, Client.PlayerId))
                {
                    Error = 0x80000000;
                }
                Client.SendPacket(new PROTOCOL_MESSENGER_NOTE_DELETE_ACK(Error, Objects));
                Objects = null;
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_MESSENGER_NOTE_DELETE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}