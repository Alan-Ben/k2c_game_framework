using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOActionFinishEvent : _AALSOBaseActionEvent
    {
        public ALSOActionFinishEvent()
            : base()
        {
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALBaseCreatureActionObj _actionObj)
        {
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALBaseCreatureActionObj _actionObj)
        {
            //需要将动作在LATE UPDATE删除时为了避免在UPDATE同时处理多个事件时出现的空指针偶发错误
            if (null == _actionObj.creatureControl)
                return;

            //remove self
            _actionObj.creatureControl.getActionControler().removeAction(_actionObj);
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
