using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOMoveStateCanBlendEvent : _AALSOBaseEvent
    {
        public bool canBlend;

        public ALSOMoveStateCanBlendEvent()
            : base()
        {
        }
        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
            if (!(_parentSession is ALCreatureMoveStateAnimationSession))
                return;

            ALCreatureMoveStateAnimationSession animationSession = (ALCreatureMoveStateAnimationSession)_parentSession;
            animationSession.moveState.setCanBlend(canBlend);
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
        }

        /***********************
         * 是否需要Update后处理
         **/
        public override bool needLaterActive()
        {
            return false;
        }
    }
}
#endif
