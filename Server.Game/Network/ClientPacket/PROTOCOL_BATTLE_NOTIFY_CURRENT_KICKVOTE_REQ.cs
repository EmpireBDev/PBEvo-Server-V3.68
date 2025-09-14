using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ : GameClientPacket
    {
        private byte type;
        public PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            type = ReadC();
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
                if (Room == null || Room.State != RoomState.Battle || Room.VoteTime.Timer == null || Room.votekick == null || !Room.GetSlot(Player.SlotId, out SlotModel slot) || slot.State != SlotState.BATTLE)
                {
                    return;
                }
                VoteKickModel vote = Room.votekick;
                if (vote.Votes.Contains(Player.SlotId))
                {
                    Client.SendPacket(new PROTOCOL_BATTLE_VOTE_KICKVOTE_ACK(0x800010F1));
                    return;
                }
                lock (vote.Votes)
                {
                    vote.Votes.Add(slot.Id);
                }
                if (type == 0)
                {
                    vote.Accept++;
                    if (slot.Team == (TeamEnum)(vote.VictimIdx % 2))
                    {
                        vote.Allies++;
                    }
                    else
                    {
                        vote.Enemies++;
                    }
                }
                else
                {
                    vote.Denie++;
                }
                if (vote.Votes.Count >= vote.GetInGamePlayers())
                {
                    Room.VoteTime.Timer = null;
                    AllUtils.VotekickResult(Room);
                }
                else
                {
                    using (PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK packet = new PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_ACK(vote))
                    {
                        Room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_NOTIFY_CURRENT_KICKVOTE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}