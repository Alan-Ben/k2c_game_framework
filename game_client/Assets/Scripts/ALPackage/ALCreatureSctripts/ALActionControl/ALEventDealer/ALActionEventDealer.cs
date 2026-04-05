using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /**************************
     * 对象进行一般事件处理的相关信息存储对象
     **/
    public class ALActionEventDealer
        : _IALFrameLateChecker
    {
        protected _AALSOBaseActionEvent eventObj;
        protected ALBaseCreatureActionObj actionObj;
        /** 本事件触发的实际触发时间点 */
        protected float realActiveTime;

        public ALActionEventDealer(ALActionEventInfoObj _eventInfo, ALBaseCreatureActionObj _actionObj)
        {
            eventObj = _eventInfo.eventObj;
            actionObj = _actionObj;
            realActiveTime = _eventInfo.activeTime + _eventInfo.eventObj.getActionEventDelayTime(_actionObj.creatureControl);
        }
        public ALActionEventDealer(ALActionEventInfoObj _eventInfo, ALBaseCreatureActionObj _actionObj, float _addTimeTag)
        {
            eventObj = _eventInfo.eventObj;
            actionObj = _actionObj;
            realActiveTime = _addTimeTag + _eventInfo.activeTime + _eventInfo.eventObj.getActionEventDelayTime(_actionObj.creatureControl);
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
            eventObj.activeEvent(actionObj);

            //judge if we need late deal this event
            if (eventObj.needLaterActive())
            {
                actionObj.creatureControl.getFrameCheckerMgr().regLaterChecker(this);
            }
        }

        /***************
         * 每帧处理时，在LateUpdate调用的
         **/
        public void lateUpdate(_AALBasicCreatureControl _creature)
        {
            eventObj.lateActiveEvent(actionObj);
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
        public static int CompareEventTime(ALActionEventDealer _event1, ALActionEventDealer _event2)
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
