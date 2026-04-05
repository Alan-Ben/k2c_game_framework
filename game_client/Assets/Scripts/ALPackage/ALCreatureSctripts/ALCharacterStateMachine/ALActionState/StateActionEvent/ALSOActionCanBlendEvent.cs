using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOActionCanBlendEvent : _AALSOBaseActionEvent
    {
        public bool canBlend;

        public ALSOActionCanBlendEvent()
            : base()
        {
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALBaseCreatureActionObj _actionObj)
        {
            //判断动作是否对应状态下的动作
            if (!(_actionObj is ALCreatureStateActionObj))
                return;

            ALCreatureStateActionObj stateActionObj = (ALCreatureStateActionObj)_actionObj;
            stateActionObj.getParentActionStateObj().setCanBlend(canBlend);
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
