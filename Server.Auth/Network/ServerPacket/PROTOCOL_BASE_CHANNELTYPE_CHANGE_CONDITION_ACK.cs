using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_CHANNELTYPE_CHANGE_CONDITION_ACK : AuthServerPacket
    {
        public PROTOCOL_BASE_CHANNELTYPE_CHANGE_CONDITION_ACK()
        {
        }
        public override void Write()
        {
            WriteH(698);
            WriteB(ServerData(SChannelXML.Servers));
        }
        private byte[] ServerData(List<SChannelModel> Servers)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                S.WriteC((byte)Servers.Count);
                foreach (SChannelModel Server in Servers)
                {
                    S.WriteD(Server.State ? 1 : 0);
                    S.WriteB(ComDiv.AddressBytes(Server.Host));
                    S.WriteH(Server.Port);
                    S.WriteC((byte)Server.Type);
                    S.WriteH((ushort)Server.MaxPlayers);
                    S.WriteD(Server.LastPlayers);
                }
                S.WriteC(0);
                return S.ToArray();
            }
        }
    }
}