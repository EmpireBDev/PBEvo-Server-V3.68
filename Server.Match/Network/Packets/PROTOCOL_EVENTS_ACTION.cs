using Plugin.Core.Network;
using Server.Match.Data.Enums;
using Server.Match.Data.Models;
using System.Collections.Generic;

namespace Server.Match.Network.Packets
{
    public class PROTOCOL_EVENTS_ACTION
    {
        public static byte[] GET_CODE(List<ObjectHitInfo> Objs)
        {
            using (SyncServerPacket S = new SyncServerPacket())
            {
                foreach (ObjectHitInfo Info in Objs)
                {
                    if (Info.Type == 1)
                    {
                        if (Info.ObjSyncId == 0)
                        {
                            S.WriteH((ushort)UdpSubHead.StageInfoObjectMove);
                            S.WriteH((ushort)Info.ObjId);
                            S.WriteC((byte)UdpSubHead.ObjectStatic);
                            S.WriteH((ushort)UdpSubHead.ObjectAnim);
                            S.WriteH((ushort)Info.ObjLife);
                            S.WriteC((byte)Info.KillerId);
                        }
                        else
                        {
                            S.WriteH((ushort)UdpSubHead.StageInfoObjectControl);
                            S.WriteH((ushort)Info.ObjId);
                            S.WriteC((byte)UdpSubHead.ObjectStatic);
                            S.WriteH((ushort)Info.ObjLife);
                            S.WriteC((byte)Info.AnimId1);
                            S.WriteC((byte)Info.AnimId2);
                            S.WriteT(Info.SpecialUse);
                        }
                    }
                    else if (Info.Type == 2)
                    {
                        UdpGameEvent Events = UdpGameEvent.HpSync;
                        int SyncId = 11;
                        if (Info.ObjLife == 0)
                        {
                            Events |= UdpGameEvent.GetWeaponForHost;
                            SyncId += 12;
                        }
                        S.WriteH((ushort)SyncId);
                        S.WriteH((ushort)Info.ObjId);
                        S.WriteC((byte)UdpSubHead.User);
                        S.WriteD((uint)Events);
                        S.WriteH((ushort)Info.ObjLife);
                        if (Events.HasFlag(UdpGameEvent.GetWeaponForHost))
                        {
                            S.WriteC((byte)(Info.DeathType + (Info.KillerId * 16)));
                            S.WriteHV(Info.Position);
                            S.WriteD(0);
                            S.WriteC((byte)Info.HitPart);
                        }
                    }
                    else if (Info.Type == 3)
                    {
                        if (Info.ObjSyncId == 0)
                        {
                            S.WriteH((ushort)UdpSubHead.StageInfoChara);
                            S.WriteH((ushort)Info.ObjId);
                            S.WriteC((byte)UdpSubHead.ObjectStatic);
                            S.WriteH((ushort)UdpSubHead.Grenade);
                            S.WriteC((byte)(Info.ObjLife == 0 ? 1 : 0));
                        }
                        else
                        {
                            S.WriteH((ushort)UdpSubHead.StageInfoMission);
                            S.WriteH((ushort)Info.ObjId);
                            S.WriteC((byte)UdpSubHead.ObjectStatic);
                            S.WriteC((byte)Info.DestroyState);
                            S.WriteH((ushort)Info.ObjLife);
                            S.WriteT(Info.SpecialUse);
                            S.WriteC((byte)Info.AnimId1);
                            S.WriteC((byte)Info.AnimId2);
                        }
                    }
                    else if (Info.Type == 4)
                    {
                        S.WriteH((ushort)UdpSubHead.StageInfoObjectDyamic);
                        S.WriteH((ushort)Info.ObjId);
                        S.WriteC((byte)UdpSubHead.StageInfoChara);
                        S.WriteD((uint)UdpGameEvent.HpSync);
                        S.WriteH((ushort)Info.ObjLife);
                    }
                    else if (Info.Type == 5)
                    {
                        S.WriteH((ushort)UdpSubHead.ArtificialIntelligence);
                        S.WriteH((ushort)Info.ObjId);
                        S.WriteC((byte)UdpSubHead.User);
                        S.WriteD((uint)UdpGameEvent.WeaponSync);
                        S.WriteD(Info.WeaponId);
                        S.WriteC(6);
                        S.WriteC(Info.Extensions);
                    }
                    else if (Info.Type == 6)
                    {
                        S.WriteH((ushort)UdpSubHead.StageInfoObjectDyamic);
                        S.WriteH((ushort)Info.ObjId);
                        S.WriteC((byte)UdpSubHead.User);
                        S.WriteD((uint)UdpGameEvent.FireDataOnObject);
                        S.WriteC((byte)(Info.KillerId + ((int)Info.DeathType * 16)));
                        S.WriteC((byte)Info.HitPart);
                    }
                }
                return S.ToArray();
            }
        }
    }
}
