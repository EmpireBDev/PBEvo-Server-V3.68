using Plugin.Core;
using Plugin.Core.Enums;
using Plugin.Core.Network;
using Server.Match.Data.Models;
using Server.Match.Data.Models.Event;

namespace Server.Match.Network.Actions.Event
{
    public class PosRotation
    {
        public static PosRotationInfo ReadInfo(ActionModel Action, SyncClientPacket C, bool GenLog)
        {
            PosRotationInfo Info = new PosRotationInfo()
            {
                CameraX = C.ReadUH(),
                CameraY = C.ReadUH(),
                Area = C.ReadUH(),
                RotationZ = C.ReadUH(),
                RotationX = C.ReadUH(),
                RotationY = C.ReadUH()
            };
            if (GenLog)
            {
               CLogger.Print($"Slot: {Action.Slot}; Camera: (X: {Info.CameraX}, Y: {Info.CameraY}); Area: {Info.Area}; Rotation: (X: {Info.RotationX}, Y: {Info.RotationY}, Z: {Info.RotationZ})", LoggerType.Warning);
            }
            return Info;
        }
        public static void WriteInfo(SyncServerPacket S, PosRotationInfo Info)
        {
            S.WriteH(Info.CameraX);
            S.WriteH(Info.CameraY);
            S.WriteH(Info.Area);
            S.WriteH(Info.RotationZ);
            S.WriteH(Info.RotationX);
            S.WriteH(Info.RotationY);
        }
    }
}