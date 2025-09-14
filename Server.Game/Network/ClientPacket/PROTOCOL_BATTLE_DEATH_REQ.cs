using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Client;
using Server.Game.Data.Utils;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_DEATH_REQ : GameClientPacket
    {
        private FragInfos Kill;
        private bool isSuicide;
        public PROTOCOL_BATTLE_DEATH_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Kill = new FragInfos();
            Kill.KillingType = (CharaKillType)ReadC();
            Kill.KillsCount = ReadC();
            Kill.KillerIdx = ReadC();
            Kill.Weapon = ReadD();
            Kill.X = ReadT();
            Kill.Y = ReadT();
            Kill.Z = ReadT();
            Kill.Flag = ReadC();
            Kill.Unk = ReadC();
            for (int i = 0; i < Kill.KillsCount; i++)
            {
                FragModel Frag = new FragModel()
                {
                    VictimWeaponClass = ReadC()
                };
                Frag.SetHitspotInfo(ReadC());
                Frag.KillFlag = (KillingMessage)ReadH();
                Frag.Flag = ReadC();
                Frag.X = ReadT();
                Frag.Y = ReadT();
                Frag.Z = ReadT();
                Frag.AssistSlot = ReadC();
                Frag.Unk = ReadB(8);
                Kill.Frags.Add(Frag);
                if (Frag.VictimSlot == Kill.KillerIdx)
                {
                    isSuicide = true;
                }
            }
        }
        public override void Run()
        {
            try
            {
                Account player = Client.Player;
                if (player == null)
                {
                    return;
                }
                RoomModel Room = player.Room;
                if (Room == null || Room.RoundTime.Timer != null || Room.State < RoomState.Battle)
                {
                    return;
                }
                bool isBotMode = Room.IsBotMode();
                SlotModel Killer = Room.GetSlot(Kill.KillerIdx);
                if (Killer == null || !isBotMode && (Killer.State < SlotState.BATTLE || Killer.Id != player.SlotId))
                {
                    return;
                }
                RoomDeath.RegistryFragInfos(Room, Killer, out int score, isBotMode, isSuicide, Kill);
                if (isBotMode)
                {
                    Killer.Score += Killer.KillsOnLife + Room.IngameAiLevel + score;
                    if (Killer.Score > 65535)
                    {
                        Killer.Score = 65535;
                        AllUtils.ValidateBanPlayer(player, $"AI Score Cheating! ({Killer.Score})");
                    }
                    Kill.Score = Killer.Score;
                }
                else
                {
                    Killer.Score += score;
                    AllUtils.CompleteMission(Room, player, Killer, Kill, MissionType.NA, 0);
                    Kill.Score = score;
                }
                using (PROTOCOL_BATTLE_DEATH_ACK Packet = new PROTOCOL_BATTLE_DEATH_ACK(Room, Kill, Killer))
                {
                    Room.SendPacketToPlayers(Packet, SlotState.BATTLE, 0);
                }
                RoomDeath.EndBattleByDeath(Room, Killer, isBotMode, isSuicide);
            }
            catch (Exception ex)
            {
                CLogger.Print($"PROTOCOL_BATTLE_DEATH_REQ: {ex.Message}", LoggerType.Error, ex);
            }
        }
    }
}