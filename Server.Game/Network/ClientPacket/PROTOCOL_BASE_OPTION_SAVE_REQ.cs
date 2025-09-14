using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;

namespace Server.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_OPTION_SAVE_REQ : GameClientPacket
    {
        private byte[] KeyboardKeys;
        private string Macro1, Macro2, Macro3, Macro4, Macro5;
        private int Type, Value, ShowBlood, Crosshair, HandPosition, ConfigVal, AudioEnable, AudioSFX, AudioBGM, PointOfView, Sensitivity, InvertMouse, EnableInviteMsg, EnableWhisperMsg, Macro, Nations;
        public PROTOCOL_BASE_OPTION_SAVE_REQ(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }
        public override void Read()
        {
            Type = ReadC();
            Value = ReadC();
            ReadH();
            if ((Type & 1) == 1)
            {
                ShowBlood = ReadH();
                Crosshair = ReadC();
                HandPosition = ReadC();
                ConfigVal = ReadD();
                AudioEnable = ReadC();
                ReadB(5);
                AudioSFX = ReadC();
                AudioBGM = ReadC();
                PointOfView = ReadH();
                Sensitivity = ReadC();
                InvertMouse = ReadC();
                ReadC();
                ReadC();
                EnableInviteMsg = ReadC();
                EnableWhisperMsg = ReadC();
                Macro = ReadC();
                ReadC();
                ReadC();
                ReadC();
            }
            if ((Type & 2) == 2)
            {
                ReadB(5);
                KeyboardKeys = ReadB(240);
            }
            if ((Type & 4) == 4)
            {
                Macro1 = ReadU(ReadC() * 2);
                Macro2 = ReadU(ReadC() * 2);
                Macro3 = ReadU(ReadC() * 2);
                Macro4 = ReadU(ReadC() * 2);
                Macro5 = ReadU(ReadC() * 2);
            }
            if ((Type & 8) == 8)
            {
                Nations = ReadH();
            }
        }
        public override void Run()
        {
            try
            {
                Account Player = Client.Player;
                if (Player == null)
                {
                    return;
                }
                DBQuery Query = new DBQuery();
                PlayerConfig Config = Player.Config;
                if (Config != null)
                {
                    if ((Type & 1) == 1)
                    {
                        UpdatePlayerConfigs(Query, Config);
                    }
                    if ((Type & 2) == 2)
                    {
                        if (Config.KeyboardKeys != KeyboardKeys)
                        {
                            Config.KeyboardKeys = KeyboardKeys;
                            Query.AddQuery("keyboard_keys", Config.KeyboardKeys);
                        }
                    }
                    if ((Type & 4) == 4)
                    {
                        UpdatePlayerConfigMacros(Query, Config);
                    }
                    if ((Type & 8) == 8)
                    {
                        if (Config.Nations != Nations)
                        {
                            Config.Nations = Nations;
                            Query.AddQuery("nations", Config.Nations);
                        }
                    }
                    ComDiv.UpdateDB("player_configs", "owner_id", Player.PlayerId, Query.GetTables(), Query.GetValues());
                }
                Client.SendPacket(new PROTOCOL_BASE_OPTION_SAVE_ACK());
            }
            catch (Exception Ex)
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        private void UpdatePlayerConfigs(DBQuery Query, PlayerConfig Config)
        {
            if (Config.ShowBlood != ShowBlood)
            {
                Config.ShowBlood = ShowBlood;
                Query.AddQuery("show_blood", Config.ShowBlood);
            }
            if (Config.Crosshair != Crosshair)
            {
                Config.Crosshair = Crosshair;
                Query.AddQuery("crosshair", Config.Crosshair);
            }
            if (Config.HandPosition != HandPosition)
            {
                Config.HandPosition = HandPosition;
                Query.AddQuery("hand_pos", Config.HandPosition);
            }
            if (Config.Config != ConfigVal)
            {
                Config.Config = ConfigVal;
                Query.AddQuery("configs", Config.Config);
            }
            if (Config.AudioEnable != AudioEnable)
            {
                Config.AudioEnable = AudioEnable;
                Query.AddQuery("audio_enable", Config.AudioEnable);
            }
            if (Config.AudioSFX != AudioSFX)
            {
                Config.AudioSFX = AudioSFX;
                Query.AddQuery("audio_sfx", Config.AudioSFX);
            }
            if (Config.AudioBGM != AudioBGM)
            {
                Config.AudioBGM = AudioBGM;
                Query.AddQuery("audio_bgm", Config.AudioBGM);
            }
            if (Config.PointOfView != PointOfView)
            {
                Config.PointOfView = PointOfView;
                Query.AddQuery("pov_size", Config.PointOfView);
            }
            if (Config.Sensitivity != Sensitivity)
            {
                Config.Sensitivity = Sensitivity;
                Query.AddQuery("sensitivity", Config.Sensitivity);
            }
            if (Config.InvertMouse != InvertMouse)
            {
                Config.InvertMouse = InvertMouse;
                Query.AddQuery("invert_mouse", Config.InvertMouse);
            }
            if (Config.EnableInviteMsg != EnableInviteMsg)
            {
                Config.EnableInviteMsg = EnableInviteMsg;
                Query.AddQuery("enable_invite", Config.EnableInviteMsg);
            }
            if (Config.EnableWhisperMsg != EnableWhisperMsg)
            {
                Config.EnableWhisperMsg = EnableWhisperMsg;
                Query.AddQuery("enable_whisper", Config.EnableWhisperMsg);
            }
            if (Config.Macro != Macro)
            {
                Config.Macro = Macro;
                Query.AddQuery("macro_enable", Config.Macro);
            }
        }
        private void UpdatePlayerConfigMacros(DBQuery Query, PlayerConfig Config)
        {
            if ((Value & 1) == 1)
            {
                if (Config.Macro1 != Macro1)
                {
                    Config.Macro1 = Macro1;
                    Query.AddQuery("macro1", Config.Macro1);
                }
            }
            if ((Value & 2) == 2)
            {
                if (Config.Macro2 != Macro2)
                {
                    Config.Macro2 = Macro2;
                    Query.AddQuery("macro1", Config.Macro1);
                }
            }
            if ((Value & 4) == 4)
            {
                if (Config.Macro3 != Macro3)
                {
                    Config.Macro3 = Macro3;
                    Query.AddQuery("macro1", Config.Macro1);
                }
            }
            if ((Value & 8) == 8)
            {
                if (Config.Macro4 != Macro4)
                {
                    Config.Macro4 = Macro4;
                    Query.AddQuery("macro1", Config.Macro1);
                }
            }
            if ((Value & 16) == 16)
            {
                if (Config.Macro5 != Macro5)
                {
                    Config.Macro5 = Macro5;
                    Query.AddQuery("macro1", Config.Macro1);
                }
            }
        }
    }
}