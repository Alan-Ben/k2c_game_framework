using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
/*****************************
 * 角色动作状态对象，此对象存储了角色动作状态的相关信息
 **/

namespace ALPackage
{
    public abstract class _AALBaseActionState
        : _AALBaseCharacterActionState, _IALCreatureActionResponse
    {
        public _AALBaseActionState(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
        }
        public _AALBaseActionState(_AALBasicCreatureControl _creature, float _actionSpeed, ALSOCreatureActionInfo _relateAction)
            : base(_creature, _actionSpeed, _relateAction)
        {
        }
        public _AALBaseActionState(_AALBasicCreatureControl _creature, float _actionSpeed, List<ALSOCreatureActionInfo> _relateActionList)
            : base(_creature, _actionSpeed, _relateActionList)
        {
        }

        /****************
         * 返回检测后的实际状态，返回null为无新状态
         **/
        public override _AALBaseCharacterActionState checkState()
        {
            ////判断当前行为队列是否为空
            if (_getCurActionCount() > 0)
                return null;

            return getCreatureControl().getActionStateMachine().getDefaultState();
        }

        /********************
         * 尝试切换到新的状态，返回新的状态，当无法切换时，返回null
         * 部分状态，如打断状态，会将新的打断附加到原先存在的打断状态中
         */
        public override _AALBaseCharacterActionState changeToState(_AALBaseCharacterActionState _newState)
        {
            ALCharacterActionStateType newState = _newState.getState();
            if (newState == ALCharacterActionStateType.INTERRUPT || newState == ALCharacterActionStateType.DEAD)
                return _newState;

            if (canBlend)
                return _newState;

            return null;
        }

        /****************
         * 获取当前状态类型
         **/
        public override ALCharacterActionStateType getState()
        {
            return ALCharacterActionStateType.ACTION;
        }
    }
}
#endif
