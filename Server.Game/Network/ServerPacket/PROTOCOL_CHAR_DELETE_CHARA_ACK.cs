using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_CHAR_DELETE_CHARA_ACK : GameServerPacket
    {
        private readonly uint Error;
        private readonly int Slot;
        private readonly ItemsModel Item;
        private readonly CharacterModel Chara;
        public PROTOCOL_CHAR_DELETE_CHARA_ACK(uint Error, int Slot, Account Player, ItemsModel Item)
        {
            this.Error = Error;
            this.Slot = Slot;
            this.Item = Item;
            if (Player != null && Item != null)
            {
                Chara = Player.Character.GetCharacter(Item.Id);
            }
        }
        public override void Write()
        {
            WriteH(6152);
            WriteD(Error);
            if (Error == 0)
            {
                WriteC((byte)Slot);
                WriteD((uint)Item.ObjectId);
                WriteD(Chara.Slot);
            }
        }
    }
}
