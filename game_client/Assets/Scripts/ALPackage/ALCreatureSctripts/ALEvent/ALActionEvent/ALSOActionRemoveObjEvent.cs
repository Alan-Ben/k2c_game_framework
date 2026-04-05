using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOActionRemoveObjEvent : _AALSOBaseActionEvent
    {
        /** 删除Operation队列 */
        public List<ALSOBaseAnimationInfo> removeAnimationList;
        /** 删除物件队列 */
        public List<_AALSOBasicAdditionObjInfo> removeAdditionObjInfoList;

        public ALSOActionRemoveObjEvent()
            : base()
        {
            removeAnimationList = new List<ALSOBaseAnimationInfo>();
            removeAdditionObjInfoList = new List<_AALSOBasicAdditionObjInfo>();
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALBaseCreatureActionObj _actionObj)
        {
            //remove addition object
            for (int i = 0; i < removeAdditionObjInfoList.Count; i++)
            {
                _actionObj.removeChildAdditionObj(removeAdditionObjInfoList[i]);
            }
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALBaseCreatureActionObj _actionObj)
        {
            //需要将动作在LATE UPDATE删除时为了避免在UPDATE同时处理多个事件时出现的空指针偶发错误
            //remove operation
            for (int i = 0; i < removeAnimationList.Count; i++)
            {
                _actionObj.removeAnimation(removeAnimationList[i]);
            }
        }

        /***********************
         * 需要Update后处理么
         **/
        public override bool needLaterActive()
        {
            if (removeAnimationList.Count > 0)
                return true;

            return false;
        }
    }
}
#endif
