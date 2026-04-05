using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
/*****************************
 * 角色动作状态对象，此对象存储了角色动作状态的相关信息
 **/

namespace ALPackage
{
    public class ALLockActionState
        : _AALBaseCharacterActionState
    {
        /** 锁定对象的ID */
        private int _m_iTargetID;

        public ALLockActionState(ALCreatureControl _creature)
            : base(_creature)
        {
            _m_iTargetID = 0;
        }
        public ALLockActionState(ALCreatureControl _creature, float _actionSpeed, ALSOCreatureActionInfo _relateAction)
            : base(_creature, _actionSpeed, _relateAction)
        {
            _m_iTargetID = 0;
        }
        public ALLockActionState(ALCreatureControl _creature, float _actionSpeed, List<ALSOCreatureActionInfo> _relateActionList, int _targetID)
            : base(_creature, _actionSpeed, _relateActionList)
        {
            _m_iTargetID = _targetID;
        }

        /** 获取闲置状态类型 */
        public int getLockTargetID() { return _m_iTargetID; }

        /****************
         * 返回检测后的实际状态，返回null为无新状态
         **/
        public override _AALBaseCharacterActionState checkState()
        {
            return null;
        }

        /********************
         * 尝试切换到新的状态，返回新的状态，当无法切换时，返回null
         * 部分状态，如打断状态，会将新的打断附加到原先存在的打断状态中
         */
        public override _AALBaseCharacterActionState changeToState(_AALBaseCharacterActionState _newState)
        {
            ALCharacterActionStateType newState = _newState.getState();
            if (newState == ALCharacterActionStateType.LOCK)
                return null;

            return _newState;
        }

        /****************
         * 获取当前状态类型
         **/
        public override ALCharacterActionStateType getState()
        {
            return ALCharacterActionStateType.LOCK;
        }

        /****************
         * 返回是否允许移动
         **/
        public override bool canMove()
        {
            return false;
        }

        /****************
         * 进入当前状态
         **/
        public override void _onEnterState()
        {
        }

        /****************
         * 离开当前状态
         **/
        public override void _onLeaveState(_AALBaseCharacterState _newState)
        {
        }
    }
}
#endif
