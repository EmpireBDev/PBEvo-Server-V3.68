using Server.Game.Data.Models;

namespace Server.Game.Data.Commands
{
    public interface ICommand
    {
        string Command { get; }
        string Description { get; }
        string Permission { get; }
        string Args { get; }
        string Execute(string Command, string[] Args, Account Player);
    }
}
