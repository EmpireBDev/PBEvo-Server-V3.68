using System.Collections.Generic;

namespace Plugin.Core.Utility
{
    public class SafeList<T>
    {
        private readonly List<T> Lists = new List<T>();
        private readonly object Sync = new object();
        public SafeList()
        {
        }
        public void Add(T Value)
        {
            lock (Sync)
            {
                Lists.Add(Value);
            }
        }
        public void Clear()
        {
            lock (Sync)
            {
                Lists.Clear();
            }
        }
        public bool Contains(T Value)
        {
            lock (Sync)
            {
                return Lists.Contains(Value);
            }
        }
        public int Count()
        {
            lock (Sync)
            {
                return Lists.Count;
            }
        }
        public bool Remove(T Value)
        {
            lock (Sync)
            {
                return Lists.Remove(Value);
            }
        }
    }
}
