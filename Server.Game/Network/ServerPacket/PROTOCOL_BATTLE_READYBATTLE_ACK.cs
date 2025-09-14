using Plugin.Core.Network;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_READYBATTLE_ACK : GameServerPacket
    {
        private readonly uint Error;
        public PROTOCOL_BATTLE_READYBATTLE_ACK(uint Error)
        {
            this.Error = Error;
        }
        public override void Write()
        {
            WriteH(4100);
            WriteC(0);
            WriteH(0);
            WriteD(Error);
            /*
             * 0x80001008 STBL_IDX_EP_ROOM_NO_REAL_IP_S
             * 0x80001009 STBL_IDX_EP_ROOM_NO_READY_TEAM_S
             * 0x80001012 STBL_IDX_EP_ROOM_NO_START_FOR_UNDER_NAT
             * 0x80001071 STBL_IDX_EP_ROOM_NO_START_FOR_NO_CLAN_TEAM
             * 0x80001072 STBL_IDX_EP_ROOM_NO_START_FOR_TEAM_NOTFULL
             * 0x80001098 STBL_IDX_EP_ROOM_NO_START_FOR_NOT_ALL_READY
             * 0x800010AB STBL_IDX_EP_ROOM_ERROR_READY_WEAPON_EQUIP
             */
        }
    }
}