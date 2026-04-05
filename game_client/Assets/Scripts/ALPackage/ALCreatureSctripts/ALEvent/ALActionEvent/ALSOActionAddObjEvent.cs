using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOActionAddObjEvent : _AALSOBaseActionEvent
    {
        /** 增加Operation队列 */
        public List<ALSOBaseAnimationInfo> addAnimationList;
        /** 增加物件队列 */
        public List<_AALSOBasicAdditionObjInfo> additionObjList;
        /** 增加事件队列 */
        public List<ALActionEventInfoObj> eventList;

        public ALSOActionAddObjEvent()
            : base()
        {
            addAnimationList = new List<ALSOBaseAnimationInfo>();
            additionObjList = new List<_AALSOBasicAdditionObjInfo>();
            eventList = new List<ALActionEventInfoObj>();
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALBaseCreatureActionObj _actionObj)
        {
            //add operation
            for (int i = 0; i < addAnimationList.Count; i++)
            {
                _actionObj.addAnimation(addAnimationList[i]);
            }

            //add addition object
            for (int i = 0; i < additionObjList.Count; i++)
            {
                _actionObj.addAdditionObj(additionObjList[i]);
            }

            //add event
            for (int i = 0; i < eventList.Count; i++)
            {
                _actionObj.addEvent(eventList[i]);
            }
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALBaseCreatureActionObj _actionObj)
        {
        }

        /***********************
         * 需要Update后处理么
         **/
        public override bool needLaterActive()
        {
            return false;
        }
    }
}
#endif
