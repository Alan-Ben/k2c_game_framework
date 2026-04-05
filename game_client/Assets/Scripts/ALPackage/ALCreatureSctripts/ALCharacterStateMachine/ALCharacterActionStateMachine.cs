using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCharacterActionStateMachine
                    : _AALBasicActionStateMachine
    {
        public ALCharacterActionStateMachine(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
        }

        /****************
         * 判断当前状态是否允许移动
         **/
        public override bool canMove() { return _m_bcsCurState.canMove(); }

        /******************
         * 尝试转化为指定的状态
         * 
         * 返回是否成功进入状态
         **/
        protected override _AALBaseCharacterActionState _tryChgToState(_AALBaseCharacterActionState _newState)
        {
            return _m_bcsCurState.changeToState(_newState);
        }
    }
}
#endif
