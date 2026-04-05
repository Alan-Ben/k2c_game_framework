using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /*********************
     * 修改动画播放速度的事件对象
     **/
    [System.Serializable]
    public class ALSOAnimationChgTimeScaleEvent : _AALSOBaseEvent
    {
        /** 切换到的目标播放速度 */
        public float timeScale;

        public ALSOAnimationChgTimeScaleEvent()
            : base()
        {
            timeScale = 1f;
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALCreatureAnimationSession _parentSession, _AALBasicCreatureControl _creatureControl)
        {
            //设置播放速度
            //由于播放速度设置完会影响到子节点中动作的时间统计，因此所有时间更改的事件操作统一放到late update中完成
            _parentSession._setAniTimeScale(timeScale);
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
