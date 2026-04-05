using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /***************
     * 角色状态类型
     **/
    public enum ALCharacterMoveStateType
    {
        NONE,
        IDLE,           //空闲状态
        MOVE,           //移动状态，移动状态需要根据速度设置对应的移动动画
        JUMP,           //跳跃状态
        MOVEMENT,       //位移行为状态，位移行为状态不播放特殊动作，只操作位置移动
    }

    public class ALCharacterMoveStateMachine
    {
        /** 状态机所属的角色对象 */
        private _AALBasicCreatureControl _m_cParent;
        /** 当前此状态采用的移动动作模式 */
        private _ATALObjResObj<ALSOMoveAnimationModeObj> _m_mMoveAnimationModeObj;
        private ALSOMoveAnimationModeObj _m_moMoveModeObj;
        /** 当前状态对象 */
        private _AALBaseCharacterMoveState _m_msCurState;

        public ALCharacterMoveStateMachine(_AALBasicCreatureControl _parent)
        {
            _m_cParent = _parent;

            _m_mMoveAnimationModeObj = null;
            _m_moMoveModeObj = null;

            _m_msCurState = new ALIdleCharacterMoveState(_parent);
        }

        public _AALBasicCreatureControl creatureControl { get { return _m_cParent; } }

        /****************
         * 设置当前是否移动状态
         **/
        public bool IsMoving
        {
            get
            {
                return (getCurState().getState() != ALCharacterMoveStateType.IDLE);
            }
            set
            {
                //一致则不处理
                if ((value && getCurState().getState() != ALCharacterMoveStateType.IDLE)
                    || (!value && getCurState().getState() == ALCharacterMoveStateType.IDLE))
                {
                    //目标状态与当前状态匹配
                    return;
                }

                //尝试重置状态对象
                if (value)
                {
                    tryChgToState(new ALMoveCharacterMoveState(_m_cParent));
                }
                else
                {
                    tryChgToState(new ALIdleCharacterMoveState(_m_cParent));
                }
            }
        }

        /***************
         * 获取当前状态对象
         **/
        public _AALBaseCharacterMoveState getCurState() { return _m_msCurState; }
        public ALSOMoveAnimationModeObj getMoveAnimationModeObj() { return _m_moMoveModeObj; }

        /*****************
         * 初始化状态
         **/
        public void init()
        {
            tryChgToState(new ALIdleCharacterMoveState(_m_cParent));
        }

        /***************
         * 检测当前状态是否合法，并设置为合法状态
         **/
        public void checkCurState()
        {
            //检查是否需要进入新状态
            _AALBaseCharacterMoveState newState = _m_msCurState.checkState();
            if (null != newState)
            {
                __setState(newState);
            }
        }

        /***************
         * 每帧执行的操作函数
         **/
        public void onUpdate()
        {
            //检查是否需要进入新状态
            checkCurState();

            //执行状态的处理
            _m_msCurState.onUpdate();
        }

        /******************
         * 尝试转化为指定的状态
         * 
         * 返回是否成功进入状态
         **/
        public bool tryChgToState(_AALBaseCharacterMoveState _newState)
        {
            //尝试切换到新状态
            _AALBaseCharacterMoveState newState = _tryChgToState(_newState);

            if (null != newState)
            {
                if (newState != _m_msCurState)
                {
                    //设置为新状态
                    __setState(newState);
                }

                return true;
            }

            return false;
        }

        /************************
         * 强制重置状态
         **/
        public void resetState()
        {
            __setState(new ALIdleCharacterMoveState(_m_cParent));
        }
        public void resetState(_AALBaseCharacterMoveState _state)
        {
            __setState(_state);
        }

        /************************
         * 进入位移行为状态
         **/
        public ALMovementCharacterMoveState enterMovementState(ActionMovementInfo _movementInfo)
        {
            //新状态
            ALMovementCharacterMoveState targetState = new ALMovementCharacterMoveState(_m_cParent, _movementInfo);

            //尝试切换到新状态
            if (tryChgToState(targetState))
                return targetState;
            else
                return null;
        }

        /********************
         * 根据新的移动模式名，设置移动状态
         **/
        public void setMoveAnimationMode(_ATALObjResObj<ALSOMoveAnimationModeObj> _moveAnimationModeObj, _AALResObjContainer _resContainer)
        {
            if (null == _moveAnimationModeObj)
            {
                UnityEngine.Debug.LogError("Set a nll Move Animation Mode Object!");
                return;
            }

            _ATALObjResObj<ALSOMoveAnimationModeObj> preResObj = _m_mMoveAnimationModeObj;
            ALSOMoveAnimationModeObj preObj = _m_moMoveModeObj;
            //设置新值
            _m_mMoveAnimationModeObj = _moveAnimationModeObj;
            if (null != _m_mMoveAnimationModeObj)
                _m_moMoveModeObj = _m_mMoveAnimationModeObj.createObj(_resContainer);

            //调用事件函数
            _m_msCurState.onMoveModeChg();

            //删除旧资源
            if (null != preResObj)
                preResObj.discard();
            else if (null != preObj)
                ALUnityCommon.releaseGameObj(preObj);
        }
        public void setMoveAnimationMode(ALSOMoveAnimationModeObj _moveAnimationModeObj)
        {
            if (null == _moveAnimationModeObj)
            {
                UnityEngine.Debug.LogError("Set a nll Move Animation Mode Object!");
                return;
            }

            _ATALObjResObj<ALSOMoveAnimationModeObj> preResObj = _m_mMoveAnimationModeObj;
            ALSOMoveAnimationModeObj preObj = _m_moMoveModeObj;
            //设置新值
            _m_mMoveAnimationModeObj = null;
            _m_moMoveModeObj = _moveAnimationModeObj;

            //调用事件函数
            _m_msCurState.onMoveModeChg();

            //删除旧资源
            if (null != preResObj)
                preResObj.discard();
            else if (null != preObj)
                ALUnityCommon.releaseGameObj(preObj);
        }


        /**************
         * 设置当前的状态对象为新的状态对象
         * @param _newState
         * @return
         */
        protected bool __setState(_AALBaseCharacterMoveState _newState)
        {
            if (null == _newState)
                return false;

            if (null != _m_msCurState && _newState != _m_msCurState)
                _m_msCurState.onLeaveState(_newState);

            if (null != _newState && _newState != _m_msCurState)
                _newState.onEnterState();

            _m_msCurState = _newState;

            return true;
        }

        /******************
         * 尝试转化为指定的状态
         * 
         * 返回是否成功进入状态
         **/
        protected _AALBaseCharacterMoveState _tryChgToState(_AALBaseCharacterMoveState _newState)
        {
            if (null == _m_msCurState)
                return null;

            return _m_msCurState.changeToState(_newState);
        }
    }
}
#endif
