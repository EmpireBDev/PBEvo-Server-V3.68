using Plugin.Core.Enums;

namespace Plugin.Core.Models
{
    public class MissionCardModel
    {
        public ClassType WeaponReq;
        public MissionType MissionType;
        public int MissionId, MapId, WeaponReqId, MissionLimit, MissionBasicId, CardBasicId, ArrayIdx, Flag;
        public MissionCardModel(int CardBasicId, int MissionBasicId)
        {
            this.CardBasicId = CardBasicId;
            this.MissionBasicId = MissionBasicId;
            ArrayIdx = (CardBasicId * 4) + MissionBasicId;
            Flag = (15 << 4 * MissionBasicId);
        }
    }
}
