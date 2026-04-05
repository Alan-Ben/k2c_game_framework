using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOAnimationFinishEvent : _AALSOBaseEvent
    {
        public ALSOAnimationFinishEvent()
            : base()
        {
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
            if (null == _creatureControl)
                return;

            _creatureControl.getAnimationControler().removeAnimation(_parentSession);
        }

        /***********************
         * 需要Update后处理么
         **/
        public override bool needLaterActive()
        {
            return true;
        }
    }
}

#endif
