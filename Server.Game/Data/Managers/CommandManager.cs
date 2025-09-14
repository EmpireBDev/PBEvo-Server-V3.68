using Server.Game.Data.Commands;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game.Data.Managers
{
    public static class CommandManager
    {
        private static readonly Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>();
        public static void Load()
        {
            Type InterfaceType = typeof(ICommand);
            IEnumerable<object> commands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => InterfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(x => Activator.CreateInstance(x));
            foreach (ICommand command in commands)
            {
                Commands.Add(command.Command, command);
            }
        }
        public static bool TryParse(string Text, Account Player)
        {
            Text = Text.Trim();
            if (Text.Length == 0 || Player == null)
            {
                return false;
            }
            if (Text.StartsWith(":"))
            {
                string Command = Text.Substring(1);
                string[] Args = new string[0];
                if (Command.Contains(" "))
                {
                    string[] Split = Command.Split(new string[] { " " }, StringSplitOptions.None);
                    Command = Split[0];
                    Args = Split.Skip(1).ToArray();
                }
                return ExecuteCommand(Player, Commands, Command, Args);
            }
            return false;
        }
        public static IEnumerable<ICommand> GetCommandsForPlayer(Account Player)
        {
            if (Player == null)
            {
                return Enumerable.Empty<ICommand>();
            }
            return Commands.Values.Where(X => Player.HavePermission(X.Permission));
        }
        private static bool ExecuteCommand(Account Player, Dictionary<string, ICommand> Commands, string Command, string[] Args)
        {
            if (Commands.ContainsKey(Command))
            {
                ICommand CommandParser = Commands[Command];
                if (CommandParser != null)
                {
                    if (Player.HavePermission(CommandParser.Permission))
                    {
                        if (Player.Connection != null)
                        {
                            string Result = CommandParser.Execute(Command, Args, Player);
                            Player.Connection.SendPacket(new PROTOCOL_LOBBY_CHATTING_ACK("Server", 0, 5, false, Result));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int limit)
        {
            return list.Select((item, inx) => new { item, inx }).GroupBy(x => x.inx / limit).Select(g => g.Select(x => x.item));
        }
    }
}
