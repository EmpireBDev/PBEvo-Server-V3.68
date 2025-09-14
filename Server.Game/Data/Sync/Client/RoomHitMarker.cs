using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Game.Network.ServerPacket;
using Server.Game.Data.XML;
using Server.Game.Data.Models;

namespace Server.Game.Data.Sync.Client
{
    public static class RoomHitMarker
    {
        public static void Load(SyncClientPacket C)
        {
            int RoomId = C.ReadH();
            int ChannelId = C.ReadH();
            int ServerId = C.ReadH();
            byte KillerIdx = C.ReadC();
            byte DeathType = C.ReadC();
            byte HitEnum = C.ReadC();
            int Damage = C.ReadD();
            if (C.ToArray().Length > 15)
            {
                CLogger.Print($"Invalid Hit (Length > 15): {C.ToArray().Length}", LoggerType.Warning);
            }
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel == null)
            {
                return;
            }
            RoomModel Room = Channel.GetRoom(RoomId);
            if (Room != null && Room.State == RoomState.Battle)
            {
                Account Player = Room.GetPlayerBySlot(KillerIdx);
                if (Player != null)
                {
                    string warn = "";
                    if (DeathType == 10)
                    {
                        warn = Translation.GetLabel("LifeRestored", Damage);
                    }
                    else if (HitEnum == 0)
                    {
                        warn = Translation.GetLabel("HitMarker1", Damage);
                    }
                    else if (HitEnum == 1)
                    {
                        warn = Translation.GetLabel("HitMarker2", Damage);
                    }
                    else if (HitEnum == 2)
                    {
                        warn = Translation.GetLabel("HitMarker3");
                    }
                    else if (HitEnum == 3)
                    {
                        warn = Translation.GetLabel("HitMarker4");
                    }
                    Player.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK(Translation.GetLabel("HitMarkerName"), Player.GetSessionId(), 0, true, warn));
                }
            }
        }
    }
}