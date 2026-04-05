using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
/**************************
 * 为动作添加物件的事件
 **/

namespace ALPackage
{
    [System.Serializable]
    public class ALSOAnimationAdditionAddEvent : _AALSOBaseEvent
    {
        /** 添加物件的添加信息 */
        public List<_AALSOBasicAdditionObjInfo> additionObjInfoList;
        /** 增加事件队列 */
        public List<ALEventInfoObj> eventList;

        public ALSOAnimationAdditionAddEvent()
            : base()
        {
            additionObjInfoList = new List<_AALSOBasicAdditionObjInfo>();
            eventList = new List<ALEventInfoObj>();
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
            for (int i = 0; i < additionObjInfoList.Count; i++)
            {
                //处理物件添加操作
                if (null != additionObjInfoList[i])
                {
                    _parentSession.addAdditionObj(additionObjInfoList[i]);
                }
            }

            //增加事件的处理
            for (int i = 0; i < eventList.Count; i++)
            {
                _parentSession.addEvent(eventList[i]);
            }
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
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
