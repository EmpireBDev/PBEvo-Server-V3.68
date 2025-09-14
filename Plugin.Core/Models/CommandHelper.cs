namespace Plugin.Core.Models
{
    public class CommandHelper
    {
        public readonly string Tag;
        public int AllWeapons;
        public int AssaultRifle;
        public int SubMachineGun;
        public int SniperRifle;
        public int ShotGun;
        public int MachineGun;
        public int Secondary;
        public int Melee;
        public int Knuckle;
        public int RPG7;
        public int Minutes05;
        public int Minutes10;
        public int Minutes15;
        public int Minutes20;
        public int Minutes25;
        public int Minutes30;
        public CommandHelper(string Tag)
        {
            this.Tag = Tag;
        }
    }
}
