using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using System.Net;

namespace Server.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_PRESTARTBATTLE_ACK : GameServerPacket
    {
        private readonly Account Player;
        private readonly RoomModel Room;
        private readonly bool IsPreparing, LoadHits;
        private readonly uint UniqueRoomId, RoomSeed;
        private readonly SChannelModel MatchPoint;
        public PROTOCOL_BATTLE_PRESTARTBATTLE_ACK(Account Player, bool LoadHits)
        {
            this.Player = Player;
            this.LoadHits = LoadHits;
            Room = Player.Room;
            if (Room != null)
            {
                IsPreparing = Room.IsPreparing();
                MatchPoint = SChannelXML.GetServer(0);
                UniqueRoomId = Room.UniqueRoomId;
                RoomSeed = Room.Seed;
            }
        }
        public PROTOCOL_BATTLE_PRESTARTBATTLE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(4106);
            WriteD(IsPreparing ? 1 : 0);
            if (IsPreparing)
            {
                WriteD(Player.SlotId);
                WriteC((byte)(Room.RoomType == RoomCondition.Tutorial ? UdpState.RENDEZVOUS : ConfigLoader.UdpType));
                if (ConfigLoader.AESProxy == true)
                {
                    WriteB(ComDiv.AddressBytes(ConfigLoader.MatchPoint));
                    WriteB(ComDiv.AddressBytes(ConfigLoader.MatchPoint));
                }
                else
                {
                    WriteB(ComDiv.AddressBytes(MatchPoint.Host));
                    WriteB(ComDiv.AddressBytes(MatchPoint.Host));
                }
                WriteH((ushort)ConfigLoader.DEFAULT_PORT[2]);
                WriteD(UniqueRoomId);
                WriteD(RoomSeed);
                if (LoadHits)
                {
                    WriteB(new byte[35]
                    { 
                        0x15, 0x0A, 0x0B, 0x0C, 0x0D, 
                        0x0E, 0x06, 0x10, 0x11, 0x12, 
                        0x0f, 0x14, 0x21, 0x16, 0x03, 
                        0x13, 0x19, 0x1A, 0x1B, 0x1C, 
                        0x1D, 0x1E, 0x1F, 0x20, 0x09, 
                        0x22, 0x00, 0x01, 0x02, 0x17, 
                        0x04, 0x05, 0x18, 0x07, 0x08,
                    });
                }
            }
        }
    }
}
