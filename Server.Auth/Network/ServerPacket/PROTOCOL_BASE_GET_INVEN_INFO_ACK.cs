using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.XML;
using Server.Auth.Data.Utils;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_INVEN_INFO_ACK : AuthServerPacket
    {
        private readonly uint Error;
        private readonly int Total;
        private readonly List<ItemsModel> Charas, Weapons, Coupons;
        public PROTOCOL_BASE_GET_INVEN_INFO_ACK(uint Error, List<ItemsModel> Items)
        {
            this.Error = Error;
            Total = AllUtils.DataFromItems(Items, out Charas, out Weapons, out Coupons);
        }
        public override void Write()
        {
            WriteH(527);
            WriteH(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteH((ushort)Total);
                WriteB(InventoryData(Charas));
                WriteB(InventoryData(Weapons));
                WriteB(InventoryData(Coupons));
                WriteH((ushort)(TemplatePackXML.Basics.Count + TemplatePackXML.CafePCs.Count));
                WriteH((ushort)Total);
                WriteH((ushort)Total);
                WriteH((ushort)Total);
                WriteH(0);
            }
        }
        private byte[] InventoryData(List<ItemsModel> Items)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                foreach (ItemsModel Item in Items)
                {
                    S.WriteD((uint)Item.ObjectId);
                    S.WriteD(Item.Id);
                    S.WriteC((byte)Item.Equip);
                    S.WriteD(Item.Count);
                }
                return S.ToArray();
            }
        }
    }
}
