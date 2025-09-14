using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Data.XML;
using Server.Game.Network.ServerPacket;

namespace Server.Game.Data.Sync.Client
{
    public static class RoomPing
    {
        public static void Load(SyncClientPacket C)
        {
            int RoomId = C.ReadH();
            int ChannelId = C.ReadH();
            int ServerId = C.ReadH();
            int SlotId = C.ReadC();
            int PingMs = C.ReadC();
            int Latency = C.ReadUH();
            if (C.ToArray().Length > 12)
            {
                CLogger.Print($"Invalid Ping (Length > 12): {C.ToArray().Length}", LoggerType.Warning);
            }
            ChannelModel Channel = ChannelsXML.GetChannel(ServerId, ChannelId);
            if (Channel == null)
            {
                return;
            }
            RoomModel Room = Channel.GetRoom(RoomId);
            if (Room != null && Room.RoundTime.Timer == null && Room.State == RoomState.Battle)
            {
                SlotModel Slot = Room.GetSlot(SlotId);
                if (Slot != null && Slot.State == SlotState.BATTLE)
                {
                    Account Player = Room.GetPlayerBySlot(Slot);
                    if (Room.IsBotMode() || Player == null)
                    {
                        return;
                    }
                    Slot.Latency = Latency;
                    Slot.Ping = PingMs;
                    if (Slot.Latency >= ConfigLoader.MaxLatency)
                    {
                        Slot.FailLatencyTimes++;
                    }
                    else
                    {
                        Slot.FailLatencyTimes = 0;
                    }
                    if (ConfigLoader.IsDebugPing && ComDiv.GetDuration(Player.LastPingDebug) >= ConfigLoader.PingUpdateTime)
                    {
                        Player.LastPingDebug = DateTimeUtil.Now();
                        Player.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK("Server", 0, 5, false, $"{Latency}ms ({PingMs} bar)"));
                    }
                    if (Slot.FailLatencyTimes >= ConfigLoader.MaxRepeatLatency)
                    {
                        CLogger.Print($"Player: '{Player.Nickname}' (Id: {Slot.PlayerId}) kicked due to high latency. ({Slot.Latency}/{ConfigLoader.MaxLatency}ms)", LoggerType.Warning);
                        Player.Connection.Close(500, true);
                        return;
                    }
                    else
                    {
                        AllUtils.RoomPingSync(Room);
                    }
                }
            }
        }
    }
}
