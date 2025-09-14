using System;

namespace Server.Match.Data.Models
{
    public class ObjectInfo
    {
        public int Id, Life = 100, DestroyState;
        public AnimModel Animation;
        public DateTime UseDate;
        public ObjectModel Model;
        public ObjectInfo()
        {
        }
        public ObjectInfo(int Id)
        {
            this.Id = Id;
        }
    }
}