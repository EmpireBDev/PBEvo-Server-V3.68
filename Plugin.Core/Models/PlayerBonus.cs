namespace Plugin.Core.Models
{
    public class PlayerBonus
    {
        public int Bonuses;
        public int CrosshairColor;
        public int MuzzleColor;
        public int FreePass;
        public int FakeRank;
        public string FakeNick;
        public long OwnerId;
        public PlayerBonus()
        {
            CrosshairColor = 4;
            MuzzleColor = 0;
            FakeRank = 55;
            FakeNick = "";
        }
        public bool RemoveBonuses(int ItemId)
        {
            int DBonuses = Bonuses, DFreePass = FreePass;
            switch (ItemId)
            {
                case 1600001:
                {
                    Decrease(1); //Exp 10%
                    break;
                }
                case 1600002:
                {
                    Decrease(2); //Exp 10%
                    break;
                }
                case 1600003:
                {
                    Decrease(4); //Exp 10%
                    break;
                }
                case 1600037:
                {
                    Decrease(8); //Exp 10%
                    break;
                }
                case 1600004:
                {
                    Decrease(16); //Exp 10%
                    break;
                }
                case 1600119:
                {
                    Decrease(32); //Exp 10%
                    break;
                }
                case 1600038:
                {
                    Decrease(64); //Exp 10%
                    break;
                }
                case 1600011:
                {
                    Decrease(128); //Exp 10%
                    break;
                }
            }
            return (Bonuses != DBonuses || FreePass != DFreePass);
        }
        public bool AddBonuses(int ItemId)
        {
            int DBonuses = Bonuses, DFreePass = FreePass;
            switch (ItemId)
            {
                case 1600001:
                {
                    Increase(1); //Exp 10%
                    break;
                }
                case 1600002:
                {
                    Increase(2); //Exp 10%
                    break;
                }
                case 1600003:
                {
                    Increase(4); //Exp 10%
                    break;
                }
                case 1600037:
                {
                    Increase(8); //Exp 10%
                    break;
                }
                case 1600004:
                {
                    Increase(16); //Exp 10%
                    break;
                }
                case 1600119:
                {
                    Increase(32); //Exp 10%
                    break;
                }
                case 1600038:
                {
                    Increase(64); //Exp 10%
                    break;
                }
                case 1600011:
                {
                    Increase(128); //Exp 10%
                    break;
                }
            }
            return (Bonuses != DBonuses || FreePass != DFreePass);
        }
        private void Decrease(int value)
        {
            Bonuses &= ~value;
        }
        private void Increase(int value)
        {
            Bonuses |= value;
        }
    }
}