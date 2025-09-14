using Plugin.Core.Utility;
using System;

namespace Plugin.Core.Models
{
    public class AccountStatus
    {
        public long PlayerId;
        public byte ChannelId, RoomId, ClanMatchId, ServerId;
        public byte[] Buffer = new byte[4];
        public AccountStatus()
        {
        }
        public void ResetData(long PlayerId)
        {
            if (PlayerId == 0)
            {
                return;
            }
            int Channel = ChannelId, Room = RoomId, ClanMatch = ClanMatchId, Server = ServerId;
            SetData(4294967295, PlayerId);
            if (Channel != ChannelId || Room != RoomId || ClanMatch != ClanMatchId || Server != ServerId)
            {
                ComDiv.UpdateDB("accounts", "status", (long)4294967295, "player_id", PlayerId);
            }
        }
        public void SetData(uint Data, long PlayerId)
        {
            SetData(BitConverter.GetBytes(Data), PlayerId);
        }
        public void SetData(byte[] Buffer, long PlayerId)
        {
            this.PlayerId = PlayerId;
            this.Buffer = Buffer;
            ChannelId = Buffer[0];
            RoomId = Buffer[1];
            ServerId = Buffer[2];
            ClanMatchId = Buffer[3];
        }
        public void UpdateChannel(byte ChannelId)
        {
            this.ChannelId = ChannelId;
            Buffer[0] = ChannelId;
            UpdateDB();
        }
        public void UpdateRoom(byte RoomId)
        {
            this.RoomId = RoomId;
            Buffer[1] = RoomId;
            UpdateDB();
        }
        public void UpdateServer(byte ServerId)
        {
            this.ServerId = ServerId;
            Buffer[2] = ServerId;
            UpdateDB();
        }
        public void UpdateClanMatch(byte ClanMatchId)
        {
            this.ClanMatchId = ClanMatchId;
            Buffer[3] = ClanMatchId;
            UpdateDB();
        }
        private void UpdateDB()
        {
            uint Value = BitConverter.ToUInt32(Buffer, 0);
            ComDiv.UpdateDB("accounts", "status", (long)Value, "player_id", PlayerId);
        }
    }
}