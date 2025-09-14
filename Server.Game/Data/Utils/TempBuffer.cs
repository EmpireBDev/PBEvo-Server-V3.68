using Server.Game.Data.Models;

namespace Server.Game.Network
{
    public static class TempBuffer
    {
        private static Account PlayerInstance;
        public static void Set(Account player)
        {
            if (player != null)
            {
                PlayerInstance = player;
            }
        }
        public static Account Get()
        {
            if (PlayerInstance != null)
            {
                return PlayerInstance;
            }
            else
            {
                return null;
            }
        }
        public static void Remove()
        {
            PlayerInstance = null;
        }
        public static bool Contains()
        {
            return PlayerInstance != null;
        }
    }
}
