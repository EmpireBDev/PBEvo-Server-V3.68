using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class PlayerQuickstart
    {
        public long OwnerId;
        public List<QuickstartModel> Quickjoins = new List<QuickstartModel>();
        public PlayerQuickstart()
        {
        }
        public QuickstartModel GetMapList(byte MapId)
        {
            lock (Quickjoins)
            {
                foreach(QuickstartModel Quick in Quickjoins)
                {
                    if (Quick.MapId == MapId)
                    {
                        return Quick;
                    }
                }
            }
            return null;
        }
    }
}
