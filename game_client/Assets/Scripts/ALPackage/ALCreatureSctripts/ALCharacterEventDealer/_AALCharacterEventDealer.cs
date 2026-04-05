using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public abstract class _AALCharacterEventDealer
    {
        protected _AALBasicCreatureControl _m_ccParent;

        public _AALCharacterEventDealer(_AALBasicCreatureControl _parentCreature)
        {
            _m_ccParent = _parentCreature;
        }

        public _AALBasicCreatureControl getParent() { return _m_ccParent; }

        /**********************
         * 每帧执行的相关操作
         **/
        public abstract void onUpdate();

        /**********************
         * 开始和停止移动的事件函数
         **/
        public abstract void onStartMove();
        public abstract void onStopMove();
        /**********************
         * 跳跃时触发的函数
         **/
        public abstract void onEnterJump(Vector3 _jumpDirection);
        /**********************
         * 跳跃结束时触发的函数
         **/
        public abstract void onJumpDone();
        /**********************
         * 角色悬空时执行的操作
         **/
        public abstract void onDrop();
        /**********************
         * 进入掉落状态时触发的事件函数
         **/
        public abstract void onEnterDrop(Vector3 _dropDirection);
        /**********************
         * 掉落落地时触发的事件函数
         **/
        public abstract void onDropDone();
        /**********************
         * 位移操作完成时的处理
         **/
        public abstract void onMovementDone(ActionMovementInfo _movementInfo);
    }
}
#endif
