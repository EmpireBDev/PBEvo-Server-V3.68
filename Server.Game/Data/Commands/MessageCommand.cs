using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Network.ServerPacket;
using System.Collections.Generic;

namespace Server.Game.Data.Commands
{
    public class MessageCommand : ICommand
    {
        public string Command => "sendmsg";
        public string Description => "Send messages";
        public string Permission => "moderatorcommand";
        public string Args => "%options% %text%";
        public string Execute(string Command, string[] Args, Account Player)
        {
            string Options = Args[0].ToLower(), Value = string.Join(" ", Args, 1, Args.Length - 1), Result = "";
            if (Options.Equals("room"))
            {
                RoomModel Room = Player.Room;
                if (Room == null)
                {
                    return $"A room is required to execute the command.";
                }
                using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Value))
                {
                    Room.SendPacketToPlayers(Packet);
                }
                Result = $"current Room Id: {Room.RoomId}";
            }
            else if (Options.Equals("channel"))
            {
                int Count = 0;
                ChannelModel Channel = Player.GetChannel();
                if (Channel == null)
                {
                    return $"Please run the command in the lobby.";
                }
                using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Value))
                {
                    Channel.SendPacketToWaitPlayers(Packet);
                    Count = Channel.GetWaitPlayers().Count;
                }
                Result = $"{Count} total of player(s)";
            }
            else if (Options.Equals("player"))
            {
                int Count = 0;
                using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Value))
                {
                    SortedList<long, Account> Players = AccountManager.Accounts;
                    if (Players.Count == 0)
                    {
                        Count = 0;
                    }
                    byte[] Data = Packet.GetCompleteBytes("Player.MessageCommands");
                    foreach (Account Member in Players.Values)
                    {
                        Member.SendCompletePacket(Data, Packet.GetType().Name);
                        Count++;
                    }
                }
                Result = $"{Count} total of player(s)";
            }
            return $"Message sended to {Result}";
        }
    }
}
