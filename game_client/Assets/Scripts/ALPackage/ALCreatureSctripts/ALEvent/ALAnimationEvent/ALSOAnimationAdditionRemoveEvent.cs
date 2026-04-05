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
    public class ALSOAnimationAdditionRemoveEvent : _AALSOBaseEvent
    {
        /** 添加物件的添加信息 */
        public List<_AALSOBasicAdditionObjInfo> additionObjInfoList;

        public ALSOAnimationAdditionRemoveEvent()
            : base()
        {
            additionObjInfoList = new List<_AALSOBasicAdditionObjInfo>();
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
                    _parentSession.removeChildAdditionObj(additionObjInfoList[i]);
                }
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
