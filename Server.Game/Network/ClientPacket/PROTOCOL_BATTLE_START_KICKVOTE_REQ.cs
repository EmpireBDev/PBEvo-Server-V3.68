using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_START_KICKVOTE_REQ : GameClientPacket
    {
        private int Reason, TargetSlot;
        private uint Error;
        public PROTOCOL_BATTLE_START_KICKVOTE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            TargetSlot = ReadC();
            Reason = ReadC(); //Motive 0 = No Manner | 1 = Illegal Program | 2 = Abuse | 3 = ETC
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
                if (Room != null && Room.State == RoomState.Battle && Player.SlotId != TargetSlot)
                {
                    SlotModel Slot = Room.GetSlot(Player.SlotId);
                    if (Slot != null && Slot.State == SlotState.BATTLE && Room.GetSlot(TargetSlot).State == SlotState.BATTLE)
                    {
                        Room.GetPlayingPlayers(true, out int RedPlayers, out int BluePlayers);
                        //if (redPlayers < 3 && bluePlayers == 1 ||
                        //bluePlayers < 3 && redPlayers == 1) erro = 0x800010E2;
                        if (Player.Rank < ConfigLoader.MinRankVote && !Player.HaveGMLevel())
                        {
                            Error = 0x800010E4;
                        }
                        else if (Room.VoteTime.Timer != null)
                        {
                            Error = 0x800010E0;
                        }
                        else if (Slot.NextVoteDate > DateTimeUtil.Now())
                        {
                            Error = 0x800010E1;
                        }
                        Client.SendPacket(new PROTOCOL_BATTLE_SUGGEST_KICKVOTE_ACK(Error));
                        if (Error > 0)
                        {
                            return;
                        }
                        Slot.NextVoteDate = DateTimeUtil.Now().AddMinutes(1);
                        VoteKickModel VoteKick = new VoteKickModel(Slot.Id, TargetSlot) 
                        { 
                            Motive = Reason 
                        };
                        Room.votekick = VoteKick;
                        for (int i = 0; i < 16; i++)
                        {
                            Room.votekick.TotalArray[i] = (Room.Slots[i].State == SlotState.BATTLE);
                        }
                        using (PROTOCOL_BATTLE_START_KICKVOTE_ACK packet = new PROTOCOL_BATTLE_START_KICKVOTE_ACK(Room.votekick))
                        {
                            Room.SendPacketToPlayers(packet, SlotState.BATTLE, 0, Player.SlotId, TargetSlot);
                        }
                        //AllUtils.LogVotekickStart(room, p, slot);
                        Room.StartVote();
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_START_KICKVOTE_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}