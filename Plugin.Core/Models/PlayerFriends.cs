using System.Collections.Generic;

namespace Plugin.Core.Models
{
    public class PlayerFriends
    {
        public List<FriendModel> Friends = new List<FriendModel>();
        public bool MemoryCleaned;
        public PlayerFriends()
        {
        }
        public void CleanList()
        {
            lock (Friends)
            {
                foreach (FriendModel Friend in Friends)
                {
                    Friend.Info = null;
                }
            }
            MemoryCleaned = true;
        }
        public void AddFriend(FriendModel friend)
        {
            lock (Friends)
            {
                Friends.Add(friend);
            }
        }
        public bool RemoveFriend(FriendModel friend)
        {
            lock (Friends)
            {
                return Friends.Remove(friend);
            }
        }
        public void RemoveFriend(int index)
        {
            lock (Friends)
            {
                Friends.RemoveAt(index);
            }
        }
        public void RemoveFriend(long id)
        {
            lock (Friends)
            {
                for (int i = 0; i < Friends.Count; i++)
                {
                    FriendModel f = Friends[i];
                    if (f.PlayerId == id)
                    {
                        Friends.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        public int GetFriendIdx(long id)
        {
            lock (Friends)
            {
                for (int i = 0; i < Friends.Count; i++)
                {
                    FriendModel f = Friends[i];
                    if (f.PlayerId == id)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        public FriendModel GetFriend(int idx)
        {
            lock (Friends)
            {
                try
                {
                    return Friends[idx];
                }
                catch
                {
                    return null;
                }
            }
        }
        public FriendModel GetFriend(long id)
        {
            lock (Friends)
            {
                for (int i = 0; i < Friends.Count; i++)
                {
                    FriendModel f = Friends[i];
                    if (f.PlayerId == id)
                    {
                        return f;
                    }
                }
            }
            return null;
        }
        public FriendModel GetFriend(long id, out int index)
        {
            lock (Friends)
            {
                for (int i = 0; i < Friends.Count; i++)
                {
                    FriendModel f = Friends[i];
                    if (f.PlayerId == id)
                    {
                        index = i;
                        return f;
                    }
                }
            }
            index = -1;
            return null;
        }
    }
}