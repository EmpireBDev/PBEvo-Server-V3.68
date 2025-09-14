namespace Plugin.Core.Models
{
    public class PlayerConfig
    {
        public long OwnerId;
        public int Crosshair, AudioSFX, AudioBGM, Sensitivity, PointOfView, ShowBlood, HandPosition, AudioEnable, Config, InvertMouse, EnableInviteMsg, EnableWhisperMsg, Macro, Nations;
        public string Macro1, Macro2, Macro3, Macro4, Macro5;
        public byte[] KeyboardKeys = new byte[240];
        public PlayerConfig()
        {
            AudioSFX = 100;
            AudioBGM = 60;
            Crosshair = 2;
            Sensitivity = 50;
            PointOfView = 80;
            ShowBlood = 3;
            AudioEnable = 7;
            Config = 55;
            Macro = 31;
            Macro1 = "";
            Macro2 = "";
            Macro3 = "";
            Macro4 = "";
            Macro5 = "";
            Nations = 0;
        }
    }
}