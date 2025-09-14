namespace Plugin.Core.Models
{
    public class RecordInfo
    {
        public long PlayerId;
        public int RecordValue;
        public RecordInfo(string[] Split)
        {
            PlayerId = GetPlayerId(Split);
            RecordValue = GetPlayerValue(Split);
        }
        public long GetPlayerId(string[] Split)
        {
            try
            {
                return long.Parse(Split[0]);
            }
            catch
            {
                return 0;
            }
        }
        public int GetPlayerValue(string[] Split)
        {
            try
            {
                return int.Parse(Split[1]);
            }
            catch
            {
                return 0;
            }
        }
        public string GetSplit()
        {
            return PlayerId + "-" + RecordValue;
        }
    }
}
