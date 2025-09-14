using Plugin.Core.Models;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Auth.Data.Models;
using Server.Auth.Data.XML;
using System.Collections.Generic;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GAME_SERVER_STATE_ACK : AuthServerPacket
    {
        public PROTOCOL_BASE_GAME_SERVER_STATE_ACK()
        {
        }
        public override void Write()
        {
            WriteH(608);
            WriteB(ServerData(SChannelXML.Servers));
            WriteC(0);
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
                    S.WriteB(ComDiv.AddressBytes(Server.Host));
                    S.WriteH(Server.Port);
                    S.WriteC((byte)Server.Type);
                    S.WriteH((ushort)Server.MaxPlayers);
                    S.WriteD(Server.LastPlayers);
                    if (Server.Id == 0)
                    {
                        S.WriteB(Bitwise.HexStringToByteArray("01 01 01 01 01 01 01 01 01 01 0E 00 00 00 00"));
                    }
                    else
                    {
                        foreach (ChannelModel Channel in ChannelsXML.GetChannels(Server.Id))
                        {
                            S.WriteC((byte)Channel.Type);
                        }
                        S.WriteC((byte)Server.Type);
                        S.WriteC(0); //Mobile
                        S.WriteH(0);
                    }
                }
                return S.ToArray();
            }
        }
    }
}