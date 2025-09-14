using Plugin.Core.Enums;
using Plugin.Core.JSON;
using Plugin.Core.Models;
using Plugin.Core.Utility;
using Server.Game.Data.Models;

namespace Server.Game.Data.Commands
{
    public class HostCommand : ICommand
    {
        public string Command => "host";
        public string Description => "Change room options (AI Only)";
        public string Permission => "hostcommand";
        public string Args => "%options% %value%";
        public string Execute(string Command, string[] Args, Account Player)
        {
            RoomModel Room = Player.Room;
            if (Room == null)
            {
                return $"A room is required to execute the command.";
            }
            else if (!(Room.GetLeader(out Account Host) && Host == Player))
            {
                return $"This Command Is Only Valid For Host (Room Leader).";
            }
            else if (!Room.IsBotMode())
            {
                return $"This Command Is Only Valid For Bot (Practice) Mode.";
            }
            string Options = Args[0].ToLower(), Value = Args[1].ToLower(), Result = "";
            if (Options.Equals("wpn"))
            {
                CommandHelper Helper = CommandHelperJSON.GetTag("WeaponsFlag");
                switch (Value)
                {
                    case "ar":
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.AssaultRifle;
                        Room.UpdateRoomInfo();
                        Result = "Assault Rifle (Only)"; 
                        break;
                    }
                    case "smg":
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.SubMachineGun;
                        Room.UpdateRoomInfo();
                        Result = "Sub Machine Gun (Only)";
                        break;
                    }
                    case "sr":
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.SniperRifle;
                        Room.UpdateRoomInfo();
                        Result = "Sniper Rifle (Only)";
                        break;
                    }
                    case "sg":
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.ShotGun;
                        Room.UpdateRoomInfo();
                        Result = "Shot Gun (Only)";
                        break;
                    }
                    case "mg":
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.MachineGun;
                        Room.UpdateRoomInfo();
                        Result = "Machine Gun (Only)";
                        break;
                    }
                    case "rpg":
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.RPG7;
                        Room.UpdateRoomInfo();
                        Result = "RPG-7 (Only) - Hot Glitch";
                        break;
                    }
                    case "all":
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.AllWeapons;
                        Room.UpdateRoomInfo();
                        Result = "All (AR, SMG, SR, SG, MG)";
                        break;
                    }
                    default:
                    {
                        Room.WeaponsFlag = (RoomWeaponsFlag)Helper.AllWeapons;
                        Room.UpdateRoomInfo();
                        Result = "Default (Value Not Founded)"; 
                        break;
                    }
                }
            }
            else if (Options.Equals("time"))
            {
                CommandHelper Helper = CommandHelperJSON.GetTag("PlayTime");
                switch (int.Parse(Value))
                {
                    case 5:
                    {
                        Room.KillTime = Helper.Minutes05;
                        Room.UpdateRoomInfo();
                        Result = $"5 Minutes";
                        break;
                    }
                    case 10:
                    {
                        Room.KillTime = Helper.Minutes10;
                        Room.UpdateRoomInfo();
                        Result = $"10 Minutes";
                        break;
                    }
                    case 15:
                    {
                        Room.KillTime = Helper.Minutes15;
                        Room.UpdateRoomInfo();
                        Result = $"15 Minutes";
                        break;
                    }
                    case 20:
                    {
                        Room.KillTime = Helper.Minutes20;
                        Room.UpdateRoomInfo();
                        Result = $"20 Minutes";
                        break;
                    }
                    case 25:
                    {
                        Room.KillTime = Helper.Minutes25;
                        Room.UpdateRoomInfo();
                        Result = $"25 Minutes";
                        break;
                    }
                    case 30:
                    {
                        Room.KillTime = Helper.Minutes30;
                        Room.UpdateRoomInfo();
                        Result = $"30 Minutes";
                        break;
                    }
                    default:
                    {
                        Result = "None! (Wrong Value)";
                        break;
                    }
                }
            }
            return $"{(Options.Equals("wpn") ? "Weapon" : ComDiv.ToTitleCase(Options))} Changed To {Result}.";
        }
    }
}
