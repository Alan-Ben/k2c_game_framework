using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALAnimationEventDealer
        : _IALFrameLateChecker
    {
        private _AALSOBaseEvent eventObj;
        private ALCreatureAnimationSession animationObj;
        /** 本事件触发的实际触发时间点 */
        protected float realActiveTime;

        public ALAnimationEventDealer(ALEventInfoObj _eventInfo, ALCreatureAnimationSession _animationObj)
        {
            eventObj = _eventInfo.eventObj;
            animationObj = _animationObj;
            realActiveTime = _eventInfo.activeTime;
        }
        public ALAnimationEventDealer(ALEventInfoObj _eventInfo, ALCreatureAnimationSession _animationObj, float _addTimeTag)
        {
            eventObj = _eventInfo.eventObj;
            animationObj = _animationObj;
            realActiveTime = _addTimeTag + _eventInfo.activeTime;
        }

        public float activeTime
        {
            get
            {
                return realActiveTime;
            }
        }

        public void dealEvent()
        {
            if (null == animationObj.creatureControl)
                return;

            eventObj.activeEvent(animationObj, animationObj.creatureControl);

            //judge if we need late deal this event
            if (eventObj.needLaterActive())
            {
                animationObj.creatureControl.getFrameCheckerMgr().regLaterChecker(this);
            }
        }

        /***************
         * 每帧处理时，在LateUpdate调用的
         **/
        public void lateUpdate(_AALBasicCreatureControl _creature)
        {
            eventObj.lateActiveEvent(animationObj, animationObj.creatureControl);
        }

        /*****************
         * 检测对象是否有效
         **/
        public bool laterCheckerEnable()
        {
            return false;
        }

        /**********************
         * judge the action time
         **/
        public static int CompareEventTime(ALAnimationEventDealer _event1, ALAnimationEventDealer _event2)
        {
            if (_event1.activeTime > _event2.activeTime)
            {
                return 1;
            }
            else if (_event1.activeTime < _event2.activeTime)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
#endif
