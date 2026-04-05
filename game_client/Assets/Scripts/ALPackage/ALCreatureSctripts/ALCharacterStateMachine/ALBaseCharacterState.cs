using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
/*****************************
 * 角色状态对象，此对象存储了角色状态的相关信息
 **/

namespace ALPackage
{
    public abstract class _AALBaseCharacterState
    {
        /** 状态所属的角色对象 */
        protected _AALBasicCreatureControl _m_cCharacterControl;
        /** 当前状态是否允许过渡到下一个状态 */
        protected bool _m_bCanBlend;

        public _AALBaseCharacterState(_AALBasicCreatureControl _creature)
        {
            _m_cCharacterControl = _creature;
            _m_bCanBlend = false;
        }

        public _AALBasicCreatureControl getCreatureControl() { return _m_cCharacterControl; }
        public bool canBlend { get { return _m_bCanBlend; } }

        public void setCanBlend(bool _canBlend) { _m_bCanBlend = _canBlend; }

        /****************
         * 进入当前状态
         **/
        public abstract void onEnterState();

        /****************
         * 离开当前状态
         **/
        public abstract void onLeaveState(_AALBaseCharacterState _newState);
    }
}
#endif
