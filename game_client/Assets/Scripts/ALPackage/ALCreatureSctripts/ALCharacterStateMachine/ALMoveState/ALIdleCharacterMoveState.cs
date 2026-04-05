using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

#if AL_CREATURE_SYS
/*****************************
 * 角色状态对象，此对象存储了角色状态的相关信息
 **/

namespace ALPackage
{
    public class ALIdleCharacterMoveState : _AALBaseCharacterMoveState
    {
        public ALIdleCharacterMoveState(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
        }

        /********************
         * 返回状态对象的枚举类型
         * @return
         */
        public override ALCharacterMoveStateType getState()
        {
            return ALCharacterMoveStateType.IDLE;
        }

        /********************
         * 检查本状态是否有效
         * @return
         */
        public override _AALBaseCharacterMoveState checkState()
        {
            return null;
        }

        /********************
         * change into the new state and return the new state
         * if null then can not change to new state
         * @param newState
         * @return
         */
        public override _AALBaseCharacterMoveState changeToState(_AALBaseCharacterMoveState _newState)
        {
            return _newState;
        }

        /**********************
         * 在角色进行每帧处理时处理的函数
         **/
        public override void onUpdate()
        {
            //无移动时暂时不做高度判断，以及其他操作
        }

        /**********************
         * 进入本状态的子类处理函数
         **/
        protected override void _onEnterState()
        {
        }

        /**********************
         * 离开本状态的子类处理函数
         **/
        protected override void _onLeaveState()
        {
        }

        /**********************
         * 在行动状态改变时，调用的事件函数，用于刷新动作
         **/
        protected override void _onMoveModeChg()
        {
        }
    }
}
#endif
