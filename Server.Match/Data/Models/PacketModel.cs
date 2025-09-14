using System;

namespace Server.Match.Data.Models
{
    public class PacketModel
    {
        public int Opcode;
        public int Slot;
        public int Round;
        public int Length;
        public int AccountId;
        public int Respawn;
        public int RoundNumber;
        public int Unk;
        public int Unk2;
        public float Time;
        public byte[] Data;
        public byte[] WithEndData;
        public byte[] WithoutEndData;
        public DateTime ReceiveDate;
    }
}