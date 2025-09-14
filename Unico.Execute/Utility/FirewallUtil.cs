using NetFwTypeLib;
using Plugin.Core;
using Plugin.Core.Enums;
using System;
using System.IO;

namespace Executable.Utility
{
    public static class FirewallUtil
    {
        private static void FWRule(string FPath, NET_FW_RULE_DIRECTION_ Direction, NET_FW_ACTION_ Action, int Key)
        {
            try
            {
                INetFwRule FirewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                FirewallRule.Action = Action;
                FirewallRule.Enabled = true;
                FirewallRule.InterfaceTypes = "All";
                FirewallRule.ApplicationName = FPath;
                FirewallRule.Name = $"Server: {Path.GetFileName(FPath)}";
                INetFwPolicy2 FirewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                FirewallRule.Direction = Direction;
                if (Key == 1)
                {
                    FirewallPolicy.Rules.Add(FirewallRule);
                }
                else
                {
                    FirewallPolicy.Rules.Remove(FirewallRule.Name);
                }
            }
            catch (Exception Ex) 
            {
                CLogger.Print(Ex.Message, LoggerType.Error, Ex);
            }
        }
        public static void AddFirewallRule(string FullPath) => FWRule(FullPath, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT, NET_FW_ACTION_.NET_FW_ACTION_ALLOW, 1);
        public static void RemoveFirewallRule(string FullPath) => FWRule(FullPath, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT, NET_FW_ACTION_.NET_FW_ACTION_BLOCK, 0);
    }
}
