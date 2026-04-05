using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public abstract class _AALSOBaseActionEvent : _AALSOBaseEvent
    {
        public _AALSOBaseActionEvent()
            : base()
        {
        }

        /***********************
         * 处理动作事件函数，转换成本对象面向对象的事件处理
         **/
        public override void activeEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
            //类型不匹配，直接不处理
            if (!(_parentSession is ALCreatureActionAnimationSession))
                return;

            ALCreatureActionAnimationSession actionAnimationSession = _parentSession as ALCreatureActionAnimationSession;
            if (null == actionAnimationSession.parentActionObj)
                return;

            activeEvent(actionAnimationSession.parentActionObj);
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
            //类型不匹配，直接不处理
            if (!(_parentSession is ALCreatureActionAnimationSession))
                return;

            ALCreatureActionAnimationSession actionAnimationSession = _parentSession as ALCreatureActionAnimationSession;
            if (null == actionAnimationSession.parentActionObj)
                return;

            lateActiveEvent(actionAnimationSession.parentActionObj);
        }

        /***********************
         * 处理事件的函数
         **/
        public abstract void activeEvent(ALBaseCreatureActionObj _actionObj);

        /***********************
         * 处理事件的函数
         **/
        public abstract void lateActiveEvent(ALBaseCreatureActionObj _actionObj);

        /************************
         * 延迟处理的时间，用于预留调整的接口
         **/
        public virtual float getActionEventDelayTime(_AALBasicCreatureControl _creatureControl) { return 0; }
    }
}
#endif
