using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Data.Commands
{
    public class HelpCommand : ICommand
    {
        readonly int MAX_COMMANDS_PER_PAGE = 5;
        public string Command => "help";
        public string Description => "Show available commands";
        public string Permission => "helpcommand";
        public string Args => "%page% (optional)";
        public string Execute(string Command, string[] Args, Account Player)
        {
            int Page = 1;
            if (Args.Length > 0)
            {
                if (int.TryParse(Args[0], out int CurrentPage))
                {
                    if (CurrentPage == 0)
                    {
                        CurrentPage = 1;
                    }
                    Page = CurrentPage;
                }
                else
                {
                    Page = 1;
                }
            }
            IEnumerable<ICommand> Commands = CommandManager.GetCommandsForPlayer(Player);
            int Pages = (Commands.Count() + MAX_COMMANDS_PER_PAGE - 1) / MAX_COMMANDS_PER_PAGE; // (records + recordsPerPage - 1) / recordsPerPage;
            if (Page > Pages)
            {
                return $"Please insert a valid page. Pages: {Pages}";
            }
            IEnumerable<ICommand> PageCommands = Commands.Split(MAX_COMMANDS_PER_PAGE).ToArray()[Page - 1];
            string Result = $"Commands ({Page}/{Pages}):\n";
            foreach(ICommand CMD in PageCommands)
            {
                Result += $":{CMD.Command} {CMD.Args} -> {CMD.Description}\n";
            }
            Player.Connection.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Result));
            return $"Displayed commands page '{Page}' of '{Pages}'";
        }
    }
}
