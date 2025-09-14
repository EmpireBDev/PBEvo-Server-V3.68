using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Server.Game.Data.Models;
using Server.Game.Data.Utils;

namespace Server.Game.Data.Commands
{
    public class RoomInfoCommand : ICommand
    {
        public string Command => "roominfo";
        public string Description => "Change room options";
        public string Permission => "moderatorcommand";
        public string Args => "%options% %value%";
        public string Execute(string Command, string[] Args, Account Player)
        {
            RoomModel Room = Player.Room;
            if (Room == null)
            {
                return $"A room is required to execute the command.";
            }
            string Options = Args[0].ToLower(), Value = Args[1].ToLower(), Result = "";
            if (Options.Equals("time"))
            {
                int KillTime = int.Parse(Value) * 6;
                if (KillTime < 0)
                {
                    return $"Oops! Map 'index' Isn't Supposed To Be: {Value}. Try an Higher Value.";
                }
                else if (Room.IsPreparing() || AllUtils.PlayerIsBattle(Player))
                {
                    return $"Oops! You Can't Change The 'time' While The Room Has Started Game Match.";
                }
                Room.KillTime = KillTime;
                Room.UpdateRoomInfo();
                Result = $"{Room.GetTimeByMask() % 60} Minutes";
            }
            else if (Options.Equals("flags"))
            {
                RoomWeaponsFlag WeaponsFlag = (RoomWeaponsFlag)int.Parse(Value);
                Room.WeaponsFlag = WeaponsFlag;
                Room.UpdateRoomInfo();
                Result = $"{WeaponsFlag}";
            }
            return $"{ComDiv.ToTitleCase(Options)} Changed To {Result}.";
        }
    }
}
