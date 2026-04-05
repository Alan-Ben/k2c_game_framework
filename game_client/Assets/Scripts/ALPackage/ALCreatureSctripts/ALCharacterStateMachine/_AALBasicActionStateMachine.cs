using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /***************
     * 角色状态类型
     **/
    public enum ALCharacterActionStateType
    {
        NONE,
        IDLE,           //空闲状态  - 客户端/服务端驱动
        LOCK,           //锁定状态  - 客户端驱动（可被服务端解除）
        ACTION,         //操作状态  - 客户端驱动
        INTERRUPT,      //打断状态  - 服务端驱动
        DEAD,           //死亡状态  - 服务端驱动
    }

    /** 获取某一行为状态的函数指针 */
    public delegate _AALBaseCharacterActionState _DelegateGetState();

    public abstract class _AALBasicActionStateMachine
    {
        /** 状态机所属的角色对象 */
        protected _AALBasicCreatureControl _m_cCharacterControl;
        /** 当前状态对象 */
        protected _AALBaseCharacterActionState _m_bcsCurState;
        /** 获取默认行为状态的回调函数 */
        protected _DelegateGetState _m_dGetDefaultStateDelegate;

        public _AALBasicActionStateMachine(_AALBasicCreatureControl _creature)
        {
            _m_cCharacterControl = _creature;

            _m_dGetDefaultStateDelegate = null;

            //设置初始状态
            __setState(getDefaultState());
        }
        public _AALBasicActionStateMachine(_AALBasicCreatureControl _creature, _DelegateGetState _defaultStateDelegate)
        {
            _m_cCharacterControl = _creature;

            _m_dGetDefaultStateDelegate = _defaultStateDelegate;

            //设置初始状态
            __setState(getDefaultState());
        }

        public _AALBasicCreatureControl creatureControl { get { return _m_cCharacterControl; } }

        /**********************
         * 设置默认的状态获取回调函数
         **/
        public void setDefaultStateDelegate(_DelegateGetState _delegate)
        {
            _m_dGetDefaultStateDelegate = _delegate;
        }

        /***************
         * 检查当前状态是否有效，无效则重置状态
         **/
        public void checkCurState()
        {
            //检查是否需要进入新状态
            _AALBaseCharacterActionState newState = _m_bcsCurState.checkState();

            if (null == newState)
                return;

            //direct set the new state
            __setState(newState);
        }

        /***************
         * 获取当前状态对象
         **/
        public _AALBaseCharacterActionState getCurState() { return _m_bcsCurState; }

        /******************
         * 尝试转化为指定的状态
         * 
         * 返回是否成功进入状态
         **/
        public void chgToState(_AALBaseCharacterActionState _newState)
        {
            //尝试切换到新状态
            _AALBaseCharacterActionState newState = _tryChgToState(_newState);
            __setState(newState);
        }
        public bool tryChgToState(_AALBaseCharacterActionState _newState)
        {
            //尝试切换到新状态
            _AALBaseCharacterActionState newState = _tryChgToState(_newState);

            if (null != newState)
            {
                if (newState != _m_bcsCurState)
                {
                    //设置为新状态
                    __setState(newState);
                }

                return true;
            }

            return false;
        }

        /*******************
         * 添加某一打断状态
         * 
         * @author alzq.z
         * @time   May 16, 2013 12:11:12 AM
         */
        public void interrupt(_AALBaseInterruptState _interruptStateObj)
        {
            if (null == _interruptStateObj)
                return;

            //获取当前真实状态
            _AALBaseCharacterActionState curState = getCurState();

            //判断当前状态是否打断状态
            if (curState.getState() == ALCharacterActionStateType.INTERRUPT)
            {
                //强制转化为打断状态信息对象
                ALInterruptActionState interruptState = (ALInterruptActionState)curState;

                //执行打断状态的添加状态函数
                interruptState.addInterruptState(_interruptStateObj);
            }
            else
            {
                //执行正常的尝试进入状态函数
                chgToState(new ALInterruptActionState(creatureControl, _interruptStateObj));
            }
        }

        /******************
         * 进入死亡状态
         **/
        public int death()
        {
            return death(1f, null);
        }
        public int death(float _actionSpeed, ALSOCreatureActionInfo _actionInfo)
        {
            //创建对象
            ALDeadActionState deathState = new ALDeadActionState(creatureControl, _actionSpeed, _actionInfo);

            //切换状态
            chgToState(deathState);

            //返回序列号
            return deathState.serialize;
        }

        /******************
         * 进入复活状态
         **/
        public void resurrect()
        {
            //获取当前状态是否死亡状态，非死亡状态则不处理
            _AALBaseCharacterActionState curState = getCurState();

            //判断当前状态是否打断状态
            if (curState.getState() != ALCharacterActionStateType.DEAD)
                return ;

            //重置动作状态
            resetState();
        }

        /************************
         * 强制重置状态
         **/
        public void resetState()
        {
            //设置初始状态
            __setState(getDefaultState());
        }
        public void resetState(_AALBaseCharacterActionState _state)
        {
            __setState(_state);
        }


        /**************
         * 设置当前的状态对象为新的状态对象
         * @param _newState
         * @return
         */
        protected bool __setState(_AALBaseCharacterActionState _newState)
        {
            if (null == _newState)
                return false;

            if (null != _m_bcsCurState && _newState != _m_bcsCurState)
                _m_bcsCurState.onLeaveState(_newState);

            _AALBaseCharacterActionState preState = _m_bcsCurState;
            _m_bcsCurState = _newState;

            if (null != _newState && _newState != preState)
                _newState.onEnterState();

            return true;
        }

        /****************************
         * 获取默认的行为状态对象
         **/
        public _AALBaseCharacterActionState getDefaultState()
        {
            if (null != _m_dGetDefaultStateDelegate)
                return _m_dGetDefaultStateDelegate();

            return new ALIdleActionState(creatureControl);
        }

        /****************
         * 判断当前状态是否允许移动
         **/
        public abstract bool canMove();

        /******************
         * 尝试转化为指定的状态
         * 
         * 返回是否成功进入状态
         **/
        protected abstract _AALBaseCharacterActionState _tryChgToState(_AALBaseCharacterActionState _newState);
    }
}
#endif
