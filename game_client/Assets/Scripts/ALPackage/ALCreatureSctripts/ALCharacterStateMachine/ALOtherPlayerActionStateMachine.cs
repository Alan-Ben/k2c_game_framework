using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALOtherPlayerActionStateMachine
                    : _AALBasicActionStateMachine
    {
        public ALOtherPlayerActionStateMachine(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
        }

        /****************
         * 判断当前状态是否允许移动，OPC永远都属于可移动状态
         **/
        public override bool canMove() { return true; }

        /******************
         * 尝试转化为指定的状态
         * 
         * 返回是否成功进入状态
         **/
        protected override _AALBaseCharacterActionState _tryChgToState(_AALBaseCharacterActionState _newState)
        {
            if (_m_bcsCurState.getState() == ALCharacterActionStateType.DEAD)
                return _m_bcsCurState.changeToState(_newState);

            return _newState;
        }
    }
}
#endif
