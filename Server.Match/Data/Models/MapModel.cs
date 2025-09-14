using System.Collections.Generic;

namespace Server.Match.Data.Models
{
    public class MapModel
    {
        public int Id;
        public List<ObjectModel> Objects = new List<ObjectModel>();
        public List<BombPosition> Bombs = new List<BombPosition>();
        public BombPosition GetBomb(int BombId)
        {
            try
            {
                return Bombs[BombId];
            }
            catch
            {
                return null;
            }
        }
    }
}
