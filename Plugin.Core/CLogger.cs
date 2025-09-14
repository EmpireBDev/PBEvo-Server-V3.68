using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Plugin.Core.Colorful;
using Plugin.Core.Enums;
using Plugin.Core.Utility;
using Console = Plugin.Core.Colorful.Console;

namespace Plugin.Core
{
    public static class CLogger
    {
        private readonly static string Date = DateTimeUtil.LogNow("yyyy-MM-dd--HH-mm-ss");
        private readonly static string CommandPath = $"Logs/{LoggerType.Command}";
        private readonly static string ConsolePath = $"Logs/{LoggerType.Console}";
        private readonly static string DebugPath = $"Logs/{LoggerType.Debug}";
        private readonly static string ErrorPath = $"Logs/{LoggerType.Error}";
        private readonly static string HackPath = $"Logs/{LoggerType.Hack}";
        private readonly static string OpcodePath = $"Logs/{LoggerType.Opcode}";
        private readonly static object Sync = new object();
        public static string StartedFor = "None";
        public static void CheckDirectorys()
        {
            try
            {
                if (StartedFor.Equals("Server"))
                {
                    if (!Directory.Exists(CommandPath))
                    {
                        Directory.CreateDirectory(CommandPath);
                    }
                    if (!Directory.Exists(ConsolePath))
                    {
                        Directory.CreateDirectory(ConsolePath);
                    }
                    if (!Directory.Exists(DebugPath))
                    {
                        Directory.CreateDirectory(DebugPath);
                    }
                    if (!Directory.Exists(ErrorPath))
                    {
                        Directory.CreateDirectory(ErrorPath);
                    }
                    if (!Directory.Exists(HackPath))
                    {
                        Directory.CreateDirectory(HackPath);
                    }
                    if (!Directory.Exists(OpcodePath))
                    {
                        Directory.CreateDirectory(OpcodePath);
                    }
                }
                else
                {
                    Print("Basic Log Directories Weren't Created!", LoggerType.Warning);
                }
            }
            catch (Exception ex)
            {
                Print(ex.Message, LoggerType.Error, ex);
            }
        }
        public static void Print(string Text, LoggerType Type, Exception Ex = null)
        {
            switch (Type)
            {
                case LoggerType.Info: Execute("{0}", Text, Ex, Type); break; 
                case LoggerType.Warning: Execute("{1}", Text, Ex, Type); break; 
                case LoggerType.Debug: Execute("{2}", Text, Ex, Type); break; 
                case LoggerType.Error: Execute("{3}", Text, Ex, Type); break; 
                case LoggerType.Hack: Execute("{4}", Text, Ex, Type); break; 
                case LoggerType.Command: Execute("{5}", Text, Ex, Type); break; 
                case LoggerType.Console: Execute("{5}", Text, Ex, Type); break; 
                case LoggerType.Opcode: Execute("{-}", Text, Ex, Type); break; 
                default: break;
            }
        }
        private static string[] StackTraces(Exception Ex)
        {
            string[] Traces = new string[3] { "", "", "" };
            try
            {
                StackTrace Trace = new StackTrace(Ex, true);
                if (Trace != null)
                {
                    Traces[0] = Trace.GetFrame(0).GetMethod().ReflectedType.Name;
                    Traces[1] = Trace.GetFrame(0).GetFileLineNumber().ToString();
                    Traces[2] = Trace.GetFrame(0).GetFileColumnNumber().ToString();
                }
            }
            catch
            {
            }
            return Traces;
        }
        private static void Execute(string Code, string Text, Exception Ex, LoggerType PathGroup)
        {
            try
            {
                lock (Sync)
                {
                    if (!PathGroup.Equals(LoggerType.Opcode))
                    {
                        Formatter[] TitleFormat = new Formatter[]
                        {
                            new Formatter("[I]", ColorUtil.White),
                            new Formatter("[W]", ColorUtil.Yellow),
                            new Formatter("[D]", ColorUtil.Cyan),
                            new Formatter("[E]", ColorUtil.Red),
                            new Formatter("[H]", ColorUtil.Red),
                            new Formatter("[C]", ColorUtil.Red)
                        };
                        Console.WriteLineFormatted($"{DateTimeUtil.Now("HH:mm:ss")} {Code} {Text}", ColorUtil.LightGrey, TitleFormat);
                    }
                    else
                    {
                        Console.WriteLine(Text, ColorUtil.Blue);
                    }
                    string FinalPath = ((PathGroup.Equals(LoggerType.Info) || PathGroup.Equals(LoggerType.Warning)) ? $"Logs/{Date}.log" : PathGroup.Equals(LoggerType.Error) ? $"Logs/{PathGroup}/{Date}-{(Ex != null ? StackTraces(Ex)[0] : "NULL")}.log" : $"Logs/{PathGroup}/{Date}.log");
                    LOG(Text, Ex, FinalPath);
                }
            }
            catch
            {
            }
        }
        private static void LOG(string Text, Exception Ex, string FinalPath)
        {
            using (FileStream File = new FileStream(FinalPath, FileMode.Append))
            using (StreamWriter Stream = new StreamWriter(File, Encoding.UTF8))
            {
                try
                {
                    string TextFinal = (Ex != null ? $"{Text} \n{Ex}" : Text);
                    Stream.WriteLine(TextFinal);
                }
                catch
                {
                }
                finally
                {
                    Stream.Flush();
                    Stream.Close();
                    File.Flush();
                    File.Close();
                }
            }
        }
    }
}
