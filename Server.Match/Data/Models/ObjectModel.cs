using Plugin.Core.Utility;
using System;
using System.Collections.Generic;

namespace Server.Match.Data.Models
{
    public class ObjectModel
    {
        public int Id, Life, Animation, UltraSync, UpdateId = 1;
        public bool NeedSync, Destroyable, NoInstaSync;
        public List<AnimModel> Animations;
        public List<DeffectModel> Effects;
        public ObjectModel(bool NeedSync)
        {
            this.NeedSync = NeedSync;
            if (NeedSync)
            {
                Animations = new List<AnimModel>();
            }
            Effects = new List<DeffectModel>();
        }
        public int CheckDestroyState(int Life)
        {
            for (int i = Effects.Count - 1; i > -1; i--)
            {
                DeffectModel Deffect = Effects[i];
                if (Deffect.Life >= Life)
                {
                    return Deffect.Id;
                }
            }
            return 0;
        }
        public int GetRandomAnimation(RoomModel Room, ObjectInfo Obj)
        {
            if (Animations != null && Animations.Count > 0)
            {
                int Idx = new Random().Next(Animations.Count);
                AnimModel Animation = Animations[Idx];
                Obj.Animation = Animation;
                Obj.UseDate = DateTimeUtil.Now();
                if (Animation.OtherObj > 0)
                {
                    ObjectInfo Obj2 = Room.Objects[Animation.OtherObj];
                    GetAnim(Animation.OtherAnim, 0, 0, Obj2);
                }
                return Animation.Id;
            }
            Obj.Animation = null;
            return 255;
        }
        public void GetAnim(int AnimId, float Time, float Duration, ObjectInfo Obj)
        {
            if (AnimId == 255 || Obj == null || Obj.Model == null || Obj.Model.Animations == null || Obj.Model.Animations.Count == 0)
            {
                return;
            }
            foreach (AnimModel Anim in Obj.Model.Animations)
            {
                if (Anim.Id == AnimId)
                {
                    Obj.Animation = Anim;
                    Time -= Duration;
                    Obj.UseDate = DateTimeUtil.Now().AddSeconds(Time * -1);
                    break;
                }
            }
        }
    }
}
