using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public abstract class _AALSOBaseEvent : ScriptableObject
    {
        /***********************
         * 处理事件的函数
         **/
        public abstract void activeEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl);

        /***********************
         * 处理事件的函数
         **/
        public abstract void lateActiveEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl);

        /***********************
         * 是否需要Update后处理
         **/
        public abstract bool needLaterActive();
    }
}
#endif
