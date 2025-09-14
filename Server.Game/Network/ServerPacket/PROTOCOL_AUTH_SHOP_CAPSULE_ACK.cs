using Plugin.Core;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.Network;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using System;
using System.Collections.Generic;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_CAPSULE_ACK : GameServerPacket
    {
        private readonly List<ItemsModel> Rewards;
        private readonly int CouponId, Index;
        public PROTOCOL_AUTH_SHOP_CAPSULE_ACK(List<ItemsModel> Rewards, int CouponId, int Index)
        {
            this.CouponId = CouponId;
            this.Index = Index;
            this.Rewards = Rewards;
        }
        public override void Write()
        {
            WriteH(1064);
            WriteH(0);
            WriteC((byte)Rewards.Count);
            for (int i = 0; i < Rewards.Count; i++)
            {
                ItemsModel Item = Rewards[i];
                WriteD(Item.Id);
                WriteD(Item.Count);
            }
            WriteC((byte)Index);
            WriteD(CouponId);
        }
    }
}