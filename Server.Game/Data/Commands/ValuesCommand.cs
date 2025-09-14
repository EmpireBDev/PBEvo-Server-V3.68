using Plugin.Core.Enums;
using Plugin.Core.Managers;
using Plugin.Core.Models;
using Plugin.Core.SQL;
using Plugin.Core.Utility;
using Server.Game.Data.Managers;
using Server.Game.Data.Models;
using Server.Game.Data.Sync.Server;
using Server.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace Server.Game.Data.Commands
{
    public class ValuesCommand : ICommand
    {
        public string Command => "player";
        public string Description => "modify value of player";
        public string Permission => "gamemastercommand";
        public string Args => "%options1% $options2% %value% %uid%";
        public string Execute(string Command, string[] Args, Account Player)
        {
            string Options = Args[0].ToLower(), Options2 = Args[1].ToLower(), Result = "";
            int Value = int.Parse(Args[2]);
            bool IsUIDONN = long.TryParse(Args[3], out long UID);
            if (Options.Equals("gift"))
            {
                Account PT = IsUIDONN ? AccountManager.GetAccount(UID, 0) : AccountManager.GetAccount(Args[3], 1, 0);
                switch (Options2)
                {
                    case "gold":
                    {
                        if (PT == null)
                        {
                            return $"Player with {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} doesn't Exist!";
                        }
                        if (DaoManagerSQL.UpdateAccountGold(PT.PlayerId, (PT.Gold + Value)))
                        {
                            PT.Gold += Value;
                            PT.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, PT));
                            SendItemInfo.LoadGoldCash(PT);
                            Result = $"{Value} Amount Of {ComDiv.ToTitleCase(Options2)} To UID: {PT.PlayerId} ({PT.Nickname})";
                        }
                        break;
                    }
                    case "cash":
                    {
                        if (PT == null)
                        {
                            return $"Player with {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} doesn't Exist!";
                        }
                        if (DaoManagerSQL.UpdateAccountCash(PT.PlayerId, (PT.Cash + Value)))
                        {
                            PT.Cash += Value;
                            PT.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, PT));
                            SendItemInfo.LoadGoldCash(PT);
                            Result = $"{Value} Amount Of {ComDiv.ToTitleCase(Options2)} To UID: {PT.PlayerId} ({PT.Nickname})";
                        }
                        break;
                    }
                    case "item": //Simplify
                    {
                        if (PT == null)
                        {
                            return $"Player with {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} doesn't Exist!";
                        }
                        GoodsItem Good = ShopManager.GetGood(Value);
                        if (Good == null)
                        {
                            return $"Goods Id: {Value} not founded!";
                        }
                        else
                        {
                            List<ItemsModel> Items = new List<ItemsModel>();
                            ItemsModel Item = new ItemsModel(Good.Item);
                            if (Item == null)
                            {
                                return $"Goods Id: {Value} was error!";
                            }
                            else
                            {
                                Items.Add(Item);
                            }
                            if (Items.Count > 0)
                            {
                                PT.SendPacket(new PROTOCOL_BASE_NEW_REWARD_POPUP_ACK(Items));
                            }
                            PT.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(1, PT, Item));
                            Result = $"{Item.Name} To UID: {PT.PlayerId} ({PT.Nickname})";
                        }
                        break;
                    }
                }
            }
            else if (Options.Equals("kick"))
            {
                switch (Options2)
                {
                    case "uid":
                    {
                        Account PT = AccountManager.GetAccount(UID, 0);
                        if (PT == null)
                        {
                            return $"Player with UID: {UID} doesn't Exist!";
                        }
                        else if (PT.PlayerId == Player.PlayerId)
                        {
                            return $"Player by UID: {UID} failed! (Can't Kick Yourself)";
                        }
                        else if (PT.Access > Player.Access)
                        {
                            return $"Player by UID: {UID} failed! (Can't Kick Higher Access Level Than Yours)";
                        }
                        PT.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                        PT.Close(TimeSpan.FromSeconds(Value).Milliseconds, true);
                        Result = $"Player by UID: {UID} Executed in {Value} Seconds!";
                        break;
                    }
                    case "nick":
                    {
                        Account PT = AccountManager.GetAccount(Args[3], 1, 0);
                        if (PT == null)
                        {
                            return $"Player with Nickname: {Args[3]} doesn't Exist!";
                        }
                        else if (PT.Nickname == Player.Nickname)
                        {
                            return $"Player by Nickname: {Args[3]} failed! (Can't Kick Yourself)";
                        }
                        else if (PT.Access > Player.Access)
                        {
                            return $"Player by Nickname: {Args[3]} failed! (Can't Kick Higher Access Level Than Yours)";
                        }
                        PT.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                        PT.Close(TimeSpan.FromSeconds(Value).Milliseconds, true);
                        Result = $"Player by Nickname: {Args[3]} Executed in {Value} Seconds!";
                        break;
                    }
                }
            }
            else if (Options.Equals("ban"))
            {
                Account PT = IsUIDONN ? AccountManager.GetAccount(UID, 0) : AccountManager.GetAccount(Args[3], 1, 0);
                switch (Options2)
                {
                    case "normal":
                    {
                        if (PT == null)
                        {
                            return $"Player with {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} doesn't Exist!";
                        }
                        else if (PT.PlayerId == Player.PlayerId)
                        {
                            return $"Player by {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} failed! (Can't Kick Yourself)";
                        }
                        else if (PT.Access > Player.Access)
                        {
                            return $"Player by {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} failed! (Can't Kick Higher Access Level Than Yours)";
                        }
                        double Days = Convert.ToDouble(Value);
                        BanHistory Ban = DaoManagerSQL.SaveBanHistory(Player.PlayerId, "DURATION", $"{PT.PlayerId}", DateTimeUtil.Now().AddDays(Days));
                        if (Ban != null)
                        {
                            string Message = $"Player '{PT.Nickname}' has been banned for {Days} Day(s)!";
                            using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Message))
                            {
                                GameXender.Client.SendPacketToAllClients(Packet);
                            }
                            PT.BanObjectId = Ban.ObjectId;
                            PT.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                            PT.Close(1000, true);
                            Result = $"{(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} Success for {Days} Day(s)";
                        }
                        break;
                    }
                    case "permanent":
                    {
                        if (PT == null)
                        {
                            return $"Player with {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} doesn't Exist!";
                        }
                        else if (PT.PlayerId == Player.PlayerId)
                        {
                            return $"Player by {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} failed! (Can't Kick Yourself)";
                        }
                        else if (PT.Access > Player.Access)
                        {
                            return $"Player by {(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} failed! (Can't Kick Higher Access Level Than Yours)";
                        }
                        AccessLevel Banned = AccessLevel.BANNED;
                        if (ComDiv.UpdateDB("accounts", "access_level", -1, "player_id", PT.PlayerId))
                        {
                            string Message = $"Player '{PT.Nickname}' has been banned for '10' Years (PERMANENTLY)!";
                            using (PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK Packet = new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Message))
                            {
                                GameXender.Client.SendPacketToAllClients(Packet);
                            }
                            PT.Access = Banned;
                            PT.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                            PT.Close(1000, true);
                            Result = $"{(IsUIDONN ? $"UID: {UID}" : $"Nickname: {Args[3]}")} Success! (Permanent)";
                        }
                        break;
                    }
                }
            }
            return $"{ComDiv.ToTitleCase(Options)} {Result}";
        }
    }
}
