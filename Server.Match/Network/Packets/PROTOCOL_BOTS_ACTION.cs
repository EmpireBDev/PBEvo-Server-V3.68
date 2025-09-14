using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Plugin.Core.Utility;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using System;
using System.IO;

namespace Server.Match.Network.Packets
{
    public class PROTOCOL_BOTS_ACTION
    {
        public static byte[] GET_CODE(byte[] Data, RoomModel Room, bool IsBotData)
        {
            SyncClientPacket C = new SyncClientPacket(Data);
            using (SyncServerPacket S = new SyncServerPacket())
            {
                if (IsBotData)
                {
                    S.WriteT(C.ReadT());
                }
                for (int i = 0; i < 16; i++)
                {
                    ActionModel Action = new ActionModel();
                    try
                    {
                        Action.Length = C.ReadUH(out bool Exception); 
                        if (Exception)
                        {
                            break;
                        }
                        Action.Slot = C.ReadUH();
                        Action.SubHead = (UdpSubHead)C.ReadC();
                        if (Action.SubHead == (UdpSubHead)byte.MaxValue)
                        {
                            break;
                        }
                        S.WriteH(Action.Length);
                        S.WriteH(Action.Slot);
                        S.WriteC((byte)Action.SubHead);
                        if (Action.SubHead == UdpSubHead.Grenade || Action.SubHead == UdpSubHead.DroppedWeapon || Action.SubHead == UdpSubHead.ObjectStatic || Action.SubHead == UdpSubHead.ObjectAnim || Action.SubHead == UdpSubHead.StageInfoObjectStatic || Action.SubHead == UdpSubHead.StageInfoObjectAnim || Action.SubHead == UdpSubHead.StageInfoObjectAnim || Action.SubHead == UdpSubHead.StageInfoObjectControl)
                        {
                            byte[] ActionDataRaw = C.ReadB(Action.Length - 5);
                            S.WriteB(ActionDataRaw);
                        }
                        else if (Action.SubHead == UdpSubHead.User || Action.SubHead == UdpSubHead.StageInfoChara)
                        {
                            Action.Flag = (UdpGameEvent)C.ReadUD();
                            Action.Data = C.ReadB(Action.Length - 9);
                            GetBotActionData(Action, Room, out byte[] Result);
                            S.GoBack(5);
                            S.WriteH((ushort)(Result.Length + 9));
                            S.WriteH(Action.Slot);
                            S.WriteC((byte)Action.SubHead);
                            S.WriteD((uint)Action.Flag);
                            S.WriteB(Result);
                            if (Action.Data.Length == 0 && Action.Length - 9 != 0 && (uint)Action.Flag != 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            CLogger.Print(Bitwise.ToHexData($"UDP SUB HEAD (BOT ACTION): '{Action.SubHead}' or '{(int)Action.SubHead}'", Data), LoggerType.Opcode);
                        }
                    }
                    catch (Exception ex)
                    {
                        CLogger.Print($"BOTS ACTION DATA - Buffer (Length: {Data.Length}): | {ex.Message}", LoggerType.Error, ex);
                        S.SetMStream(new MemoryStream());
                        break;
                    }
                }
                return S.ToArray();
            }
        }
        private static void GetBotActionData(ActionModel Action, RoomModel Room, out byte[] EventsData)
        {
            EventsData = new byte[0];
            if (Room == null)
            {
                return;
            }
            if (Action.Data.Length == 0)
            {
                return;
            }
            byte[] Data = Action.Data;
            SyncClientPacket C = new SyncClientPacket(Data);
            using (SyncServerPacket S = new SyncServerPacket())
            {
                uint Event = 0;
                if (Action.Flag.HasFlag(UdpGameEvent.ActionState) || Action.Flag.HasFlag(UdpGameEvent.Animation) || Action.Flag.HasFlag(UdpGameEvent.PosRotation) || Action.Flag.HasFlag(UdpGameEvent.SoundPosRotation) || Action.Flag.HasFlag(UdpGameEvent.UseObject) || Action.Flag.HasFlag(UdpGameEvent.ActionForObjectSync) || Action.Flag.HasFlag(UdpGameEvent.RadioChat) || Action.Flag.HasFlag(UdpGameEvent.WeaponSync) || Action.Flag.HasFlag(UdpGameEvent.WeaponRecoil) || Action.Flag.HasFlag(UdpGameEvent.HpSync) || Action.Flag.HasFlag(UdpGameEvent.DropWeapon) || Action.Flag.HasFlag(UdpGameEvent.GetWeaponForClient) || Action.Flag.HasFlag(UdpGameEvent.FireData) || Action.Flag.HasFlag(UdpGameEvent.CharaFireNHitData) || Action.Flag.HasFlag(UdpGameEvent.GetWeaponForHost) || Action.Flag.HasFlag(UdpGameEvent.FireDataOnObject) || Action.Flag.HasFlag(UdpGameEvent.FireNHitDataOnObject))
                {
                    Event += (uint)Action.Flag;
                    byte[] ActionFlagRaw = C.ReadB(Action.Length - 5);
                    S.WriteB(ActionFlagRaw);
                }
                EventsData = S.ToArray();
                if (Event != (uint)Action.Flag)
                {
                    CLogger.Print(Bitwise.ToHexData($"UDP EVENT FLAGS: '{(uint)Action.Flag}' | '{((uint)Action.Flag - Event)}'", Data), LoggerType.Opcode);
                }
            }
        }
    }
}
