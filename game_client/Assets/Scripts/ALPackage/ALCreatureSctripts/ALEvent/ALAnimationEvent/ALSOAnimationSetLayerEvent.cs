﻿using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
/**************************
* 为动作添加物件的事件
**/

namespace ALPackage
{
    [System.Serializable]
    public class ALSOAnimationSetLayerEvent : _AALSOBaseEvent
    {
        //重新设置动作层级的操作
        public CreatureActionLayer newLayer;

        public ALSOAnimationSetLayerEvent()
            : base()
        {
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
            //设置层级
            _parentSession.setAnimationLayer(newLayer);
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
