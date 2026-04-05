using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if AL_CREATURE_SYS
/*****************************
 * 角色状态对象，此对象存储了角色状态的相关信息
 **/

namespace ALPackage
{
    public abstract class _AALBaseCharacterMoveState : _AALBaseCharacterState
    {
        private ALCharacterMoveStateMachine _m_smParent;

        /** 动画片段对象 */
        private ALCreatureMoveStateAnimationSession _m_asAnimationSession;

        public _AALBaseCharacterMoveState(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
            _m_smParent = _creature.getMoveStateMachine();
        }

        public ALCharacterMoveStateMachine getParent() { return _m_smParent; }

        /********************
         * 刷新动画片段
         **/
        public void refreshAnimationSession()
        {
            if (null == _m_asAnimationSession)
                return;

            _m_asAnimationSession.refreshAnimationList();
        }

        /****************
         * 进入当前状态
         **/
        public override void onEnterState()
        {
            if (null != _m_smParent.getMoveAnimationModeObj())
            {
                //add animation
                _m_asAnimationSession = getCreatureControl().getAnimationControler().addMoveStateAnimation(this, _m_smParent.getMoveAnimationModeObj().getAnimationInfo(getState()), 1.0f);
            }

            //调用子类重载的事件函数
            _onEnterState();
        }

        /****************
         * 离开当前状态
         **/
        public override void onLeaveState(_AALBaseCharacterState _newState)
        {
            //remove session
            getCreatureControl().getAnimationControler().removeAnimation(_m_asAnimationSession);

            _m_asAnimationSession = null;

            //调用子类重载的事件函数
            _onLeaveState();
        }

        /**********************
         * 在行动状态改变时，调用的事件函数，用于刷新动作
         **/
        public void onMoveModeChg()
        {
            //remove session
            getCreatureControl().getAnimationControler().removeAnimation(_m_asAnimationSession);

            _m_asAnimationSession = null;

            //add animation
            _m_asAnimationSession = getCreatureControl().getAnimationControler().addMoveStateAnimation(this, _m_smParent.getMoveAnimationModeObj().getAnimationInfo(getState()), 1.0f);

            //调用事件函数
            _onMoveModeChg();
        }

        /********************
         * 返回状态对象的枚举类型
         * @return
         */
        public abstract ALCharacterMoveStateType getState();

        /********************
         * 检查本状态是否有效，有效则返回null
         * @return
         */
        public abstract _AALBaseCharacterMoveState checkState();

        /********************
         * 尝试跳转到对应的状态对象，返回跳转的新状态对象
         * 返回null表示不跳转
         * @param newState
         * @return
         */
        public abstract _AALBaseCharacterMoveState changeToState(_AALBaseCharacterMoveState _newState);
        /**********************
         * 在角色进行每帧处理时处理的函数
         **/
        public abstract void onUpdate();

        /**********************
         * 进入本状态的子类处理函数
         **/
        protected abstract void _onEnterState();

        /**********************
         * 离开本状态的子类处理函数
         **/
        protected abstract void _onLeaveState();

        /**********************
         * 在行动状态改变时，调用的事件函数，用于刷新动作
         **/
        protected abstract void _onMoveModeChg();
    }
}
#endif
