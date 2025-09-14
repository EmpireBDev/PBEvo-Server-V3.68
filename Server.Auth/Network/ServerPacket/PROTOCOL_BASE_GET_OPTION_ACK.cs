using Plugin.Core.Models;
using Plugin.Core.Utility;

namespace Server.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_OPTION_ACK : AuthServerPacket
    {
        private readonly int Error;
        private readonly PlayerConfig Config;
        public PROTOCOL_BASE_GET_OPTION_ACK(int Error, PlayerConfig Config)
        {
            this.Error = Error;
            this.Config = Config;
        }
        public override void Write()
        {
            WriteH(529);
            WriteH(0);
            WriteD(Error);
            if (Error == 0)
            {
                WriteH((ushort)Config.Nations);
                WriteH((ushort)(Config.Macro5.Length));
                WriteN(Config.Macro5, Config.Macro5.Length, "UTF-16LE");
                WriteH((ushort)(Config.Macro4.Length));
                WriteN(Config.Macro4, Config.Macro4.Length, "UTF-16LE");
                WriteH((ushort)(Config.Macro3.Length));
                WriteN(Config.Macro3, Config.Macro3.Length, "UTF-16LE");
                WriteH((ushort)(Config.Macro2.Length));
                WriteN(Config.Macro2, Config.Macro2.Length, "UTF-16LE");
                WriteH((ushort)(Config.Macro1.Length));
                WriteN(Config.Macro1, Config.Macro1.Length, "UTF-16LE");
                WriteH(49);
                WriteB(Bitwise.HexStringToByteArray("39 F8 10 00"));
                WriteB(Config.KeyboardKeys);
                WriteH((short)Config.ShowBlood);
                WriteC((byte)Config.Crosshair);
                WriteC((byte)Config.HandPosition);
                WriteD(Config.Config);
                WriteD(Config.AudioEnable);
                WriteH(0);
                WriteC((byte)Config.AudioSFX);
                WriteC((byte)Config.AudioBGM);
                WriteC((byte)Config.PointOfView);
                WriteC(0);
                WriteC((byte)Config.Sensitivity);
                WriteC((byte)Config.InvertMouse);
                WriteH(0);
                WriteC((byte)Config.EnableInviteMsg);
                WriteC((byte)Config.EnableWhisperMsg);
                WriteD(Config.Macro);
            }
        }
    }
}