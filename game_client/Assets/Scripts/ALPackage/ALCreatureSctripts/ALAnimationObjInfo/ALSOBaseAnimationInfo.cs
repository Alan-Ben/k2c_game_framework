using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOBaseAnimationInfo : ScriptableObject
    {
        /** 默认播放速度 */
        public float defaultPlaySpeed;
        /** 对应内部的动作对应表，对象ID对应对象 */
        public List<ALSOBaseAnimationInfoObj> animationObjList;
        /** 动画触发的事件队列 */
        public List<ALEventInfoObj> animationEventList;
        /** 动画删除时触发的事件队列 */
        public List<_AALSOBaseEvent> animationFinishEventList;

        public ALSOBaseAnimationInfo()
        {
            defaultPlaySpeed = 1.0f;
            animationObjList = new List<ALSOBaseAnimationInfoObj>();
            animationEventList = new List<ALEventInfoObj>();
            animationFinishEventList = new List<_AALSOBaseEvent>();
        }
    }
}
#endif
