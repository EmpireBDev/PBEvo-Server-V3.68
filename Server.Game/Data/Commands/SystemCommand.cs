using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Plugin.Core.XML;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;

namespace Server.Game.Data.Commands
{
    public class SystemCommand : ICommand
    {
        public string Command => "sys";
        public string Description => "change server settings";
        public string Permission => "developercommand";
        public string Args => "%options% %value%";
        public string Execute(string Command, string[] Args, Account Player)
        {
            string Options = Args[0].ToLower(), Value = Args[1].ToLower(), Result = "";
            if (Options.Equals("udp"))
            {
                int CurrentUDP = (int)ConfigLoader.UdpType, ChangeValue = int.Parse(Value);
                if (ChangeValue.Equals(CurrentUDP))
                {
                    return $"UDP State Already Changed To: {CurrentUDP}";
                }
                else if (ChangeValue < 1 || ChangeValue > 4)
                {
                    return $"Cannot Change UDP State To: {ChangeValue}";
                }
                switch (ChangeValue)
                {
                    case 1:
                    {
                        ConfigLoader.UdpType = (UdpState)ChangeValue;
                        Result = $"State Changed ({ChangeValue} - {ConfigLoader.UdpType})";
                        break;
                    }
                    case 2:
                    {
                        ConfigLoader.UdpType = (UdpState)ChangeValue;
                        Result = $"State Changed ({ChangeValue} - {ConfigLoader.UdpType})";
                        break;
                    }
                    case 3:
                    {
                        ConfigLoader.UdpType = (UdpState)ChangeValue;
                        Result = $"State Changed ({ChangeValue} - {ConfigLoader.UdpType})";
                        break;
                    }
                    case 4:
                    {
                        ConfigLoader.UdpType = (UdpState)ChangeValue;
                        Result = $"State Changed ({ChangeValue} - {ConfigLoader.UdpType})";
                        break;
                    }
                    default:
                    {
                        ConfigLoader.UdpType = UdpState.RELAY;
                        Result = $"State Changed (3 - {ConfigLoader.UdpType}). Wrong Value";
                        break;
                    }
                }
            }
            else if (Options.Equals("debug"))
            {
                bool ChangeValue = int.Parse(Value).Equals(1);
                if (ChangeValue.Equals(ConfigLoader.DebugMode))
                {
                    return $"Debug Mode Already Changed To: {ChangeValue}";
                }
                switch (ChangeValue)
                {
                    case true:
                    {
                        ConfigLoader.DebugMode = ChangeValue;
                        Result = $"Mode '{(ChangeValue ? "Enabled" : "Disabled")}'";
                        break;
                    }
                    case false:
                    {
                        ConfigLoader.DebugMode = ChangeValue;
                        Result = $"Mode '{(ChangeValue ? "Enabled" : "Disabled")}'";
                        break;
                    }
                    default:
                    {
                        Result = $"Mode Unchanged Due To Wrong Value";
                        break;
                    }
                }
            }
            else if (Options.Equals("test"))
            {
                bool ChangeValue = int.Parse(Value).Equals(1);
                if (ChangeValue.Equals(ConfigLoader.IsTestMode))
                {
                    return $"Test Mode Already Changed To: {ChangeValue}";
                }
                switch (ChangeValue)
                {
                    case true:
                    {
                        ConfigLoader.IsTestMode = ChangeValue;
                        Result = $"Mode '{(ChangeValue ? "Enabled" : "Disabled")}'";
                        break;
                    }
                    case false:
                    {
                        ConfigLoader.IsTestMode = ChangeValue;
                        Result = $"Mode '{(ChangeValue ? "Enabled" : "Disabled")}'";
                        break;
                    }
                    default:
                    {
                        Result = $"Mode Unchanged Due To Wrong Value";
                        break;
                    }
                }
            }
            else if (Options.Equals("ping"))
            {
                bool ChangeValue = int.Parse(Value).Equals(1);
                if (ChangeValue.Equals(ConfigLoader.IsDebugPing))
                {
                    return $"Ping Mode Already Changed To: {ChangeValue}";
                }
                switch (ChangeValue)
                {
                    case true:
                    {
                        ConfigLoader.IsDebugPing = ChangeValue;
                        Result = $"Mode '{(ChangeValue ? "Enabled" : "Disabled")}'";
                        break;
                    }
                    case false:
                    {
                        ConfigLoader.IsDebugPing = ChangeValue;
                        Result = $"Mode '{(ChangeValue ? "Enabled" : "Disabled")}'";
                        break;
                    }
                    default:
                    {
                        Result = $"Mode Unchanged Due To Wrong Value";
                        break;
                    }
                }
            }
            else if (Options.Equals("title"))
            {
                if (Value.Equals("all"))
                {
                    if (Player.Title.OwnerId == 0)
                    {
                        DaoManagerSQL.CreatePlayerTitlesDB(Player.PlayerId);
                        Player.Title = new PlayerTitles()
                        {
                            OwnerId = Player.PlayerId
                        };
                    }
                    PlayerTitles Titles = Player.Title;
                    int Count = 0;
                    for (int i = 1; i <= 44; i++)
                    {
                        TitleModel Title = TitleSystemXML.GetTitle(i, true);
                        if (Title != null && !Titles.Contains(Title.Flag))
                        {
                            Count++;
                            Titles.Add(Title.Flag);
                            if (Titles.Slots < Title.Slot)
                            {
                                Titles.Slots = Title.Slot;
                            }
                        }
                    }
                    if (Count > 0)
                    {
                        ComDiv.UpdateDB("player_titles", "slots", Titles.Slots, "owner_id", Player.PlayerId);
                        DaoManagerSQL.UpdatePlayerTitlesFlags(Player.PlayerId, Titles.Flags);
                        Player.SendPacket(new PROTOCOL_BASE_USER_TITLE_INFO_ACK(Player));
                    }
                    Result = "Successfully Opened!";
                }
                else
                {
                    Result = "Arguments was not valid!";
                }
            }
            return $"{(Options.Equals("udp") ? "UDP" : ComDiv.ToTitleCase(Options))} {Result}";
        }
    }
}
