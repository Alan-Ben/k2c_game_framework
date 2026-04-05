using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
/*****************************
 * 角色动作状态对象，此对象存储了角色动作状态的相关信息
 **/

namespace ALPackage
{
    public class ALInterruptActionState
        : _AALBaseCharacterActionState
    {
        /** 获取对应打断状态的函数类型 */
        public delegate bool judgeInterruptState(_AALBaseInterruptState _state);

        /** 同时存在的多个状态类型队列 */
        private List<_AALBaseInterruptState> _m_lInterruptStateList;

        public ALInterruptActionState(_AALBasicCreatureControl _creature, _AALBaseInterruptState _initInterruptState)
            : base(_creature)
        {
            _m_lInterruptStateList = new List<_AALBaseInterruptState>();

            _m_lInterruptStateList.Add(_initInterruptState);
        }
        public ALInterruptActionState(_AALBasicCreatureControl _creature, List<_AALBaseInterruptState> _initInterruptStateList)
            : base(_creature)
        {
            _m_lInterruptStateList = new List<_AALBaseInterruptState>();

            for (int i = 0; i < _initInterruptStateList.Count; i++)
            {
                _m_lInterruptStateList.Add(_initInterruptStateList[i]);
            }
        }

        /****************
         * 返回检测后的实际状态，返回null为无新状态
         **/
        public override _AALBaseCharacterActionState checkState()
        {
            //遍历所有内部打断状态，判断是否有无效
            int i = 0;
            while (i < _m_lInterruptStateList.Count)
            {
                _AALBaseInterruptState interruptState = _m_lInterruptStateList[i];

                if (!interruptState.checkState())
                {
                    //状态无效，删除状态
                    _removeInterruptState(interruptState);
                }
                else
                {
                    //检查的下标递增
                    i++;
                }
            }

            //状态队列不为空表示不更改状态
            if (_m_lInterruptStateList.Count > 0)
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
            if (newState == ALCharacterActionStateType.DEAD)
            {
                return _newState;
            }
            else if (ALCharacterActionStateType.INTERRUPT == newState)
            {
                ALInterruptActionState interruptState = (ALInterruptActionState)_newState;

                for (int i = 0; i < interruptState._m_lInterruptStateList.Count; i++)
                {
                    //添加新状态,返回本对象表明列表进行了更新
                    _addInterruptState(interruptState._m_lInterruptStateList[i]);
                }

                return this;
            }
            else
            {
                return null;
            }
        }

        /********************
         * 增加打断状态对象
         * 
         * @author alzq.z
         * @time   May 16, 2013 12:07:15 AM
         */
        public void addInterruptState(_AALBaseInterruptState _stateObj)
        {
            if (null == _stateObj)
            {
                return;
            }

            //尝试添加新的打断状态信息对象
            _addInterruptState(_stateObj);
        }

        /**********************
         * 通过带入的判断函数获取对应的打断状态对象
         **/
        public _AALBaseInterruptState getState(judgeInterruptState _judgeFunc)
        {
            if (null == _judgeFunc)
                return null;

            //返回第一个通过判断的对象
            for (int i = 0; i < _m_lInterruptStateList.Count; i++)
            {
                if (_judgeFunc(_m_lInterruptStateList[i]))
                    return _m_lInterruptStateList[i];
            }

            return null;
        }

        /****************
         * 删除并返回符合条件的第一个状态对象
         **/
        public _AALBaseInterruptState removeState(judgeInterruptState _judgeFunc)
        {
            if (null == _judgeFunc)
                return null;

            //返回第一个通过判断的对象
            for (int i = 0; i < _m_lInterruptStateList.Count; i++)
            {
                _AALBaseInterruptState interruptState = _m_lInterruptStateList[i];

                if (_judgeFunc(interruptState))
                {
                    //删除状态
                    _removeInterruptState(interruptState);

                    return interruptState;
                }
            }

            return null;
        }

        /****************
         * 获取当前状态类型
         **/
        public override ALCharacterActionStateType getState()
        {
            return ALCharacterActionStateType.INTERRUPT;
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
            //处理所有初始化状态的进入函数
            for (int i = 0; i < _m_lInterruptStateList.Count; i++)
            {
                _m_lInterruptStateList[i].setParent(this);
                _m_lInterruptStateList[i].onAdded(_m_cCharacterControl);
            }
        }

        /****************
         * 离开当前状态
         **/
        public override void _onLeaveState(_AALBaseCharacterState _newState)
        {
            //删除所有状态
            while (_m_lInterruptStateList.Count > 0)
            {
                _removeInterruptState(_m_lInterruptStateList[0]);
            }
        }

        /************
         * 刷新本状态的动作显示
         **/
        public void refreshAction()
        {
            //判断父亲对象当前状态是否还是本状态，不是则不继续处理
            if (getCreatureControl().getActionStateMachine().getCurState() != this)
                return;

            _AALBaseInterruptState tmpState = null;
            ALSOCreatureActionInfo tmpActionInfo = null;
            int usePriority = int.MinValue;
            ALSOCreatureActionInfo useActionInfo = null;

            for(int i = 0; i < _m_lInterruptStateList.Count; i++)
            {
                tmpState = _m_lInterruptStateList[i];
                if (null == tmpState)
                    continue;

                //判断优先级是否有效
                if(tmpState.stateActionPriority > usePriority)
                {
                    //尝试使用新的动作
                    tmpActionInfo = tmpState.actionInfo;
                    if (null == tmpActionInfo)
                        continue;

                    //设置优先级与动作
                    usePriority = tmpState.stateActionPriority;
                    useActionInfo = tmpActionInfo;
                }
            }

            //使用最终的动作
            swapAction(useActionInfo);
        }

        /********************
         * 添加对应状态对象
         * 
         * 客户端部分听从服务端操作，直接进行添加
         **/
        protected void _addInterruptState(_AALBaseInterruptState _state)
        {
            _m_lInterruptStateList.Add(_state);

            //调用事件函数
            _state.setParent(this);
            _state.onAdded(_m_cCharacterControl);
        }

        /****************
         * 从打断状态队列中删除某一状态
         **/
        protected void _removeInterruptState(_AALBaseInterruptState _state)
        {
            _m_lInterruptStateList.Remove(_state);

            //调用事件函数
            _state.onRemoved(_m_cCharacterControl);
        }
    }
}
#endif
