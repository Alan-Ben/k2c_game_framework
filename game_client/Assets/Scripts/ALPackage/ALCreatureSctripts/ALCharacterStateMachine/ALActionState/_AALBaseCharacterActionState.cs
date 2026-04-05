using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
/*****************************
 * 角色动作状态对象，此对象存储了角色动作状态的相关信息
 **/

namespace ALPackage
{
    public abstract class _AALBaseCharacterActionState
        : _AALBaseCharacterState, _IALCreatureActionResponse
    {
        private static int g_serialize = 1;

        /** 当前状态的序列化ID */
        protected int _m_iCharacterStateSerialize;
        /** 状态开始的时间 */
        protected float _m_fStateStartTime;

        /** 本状态关联的的Action信息队列 */
        protected List<ALSOCreatureActionInfo> _m_lRelateActionInfoList;
        /** 本状态相关动作队列 */
        protected List<ALCreatureStateActionObj> _m_lActionList;
        /** 本状态内所有行为的播放速度 */
        protected float actionSpeed;

        public _AALBaseCharacterActionState(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
            _m_iCharacterStateSerialize = g_serialize++;

            _m_fStateStartTime = 0;

            _m_lRelateActionInfoList = new List<ALSOCreatureActionInfo>();

            _m_lActionList = new List<ALCreatureStateActionObj>();
            actionSpeed = 1f;
        }
        public _AALBaseCharacterActionState(_AALBasicCreatureControl _creature, float _actionSpeed, ALSOCreatureActionInfo _relateAction)
            : base(_creature)
        {
            _m_iCharacterStateSerialize = g_serialize++;

            _m_fStateStartTime = 0;

            _m_lRelateActionInfoList = new List<ALSOCreatureActionInfo>();
            _m_lRelateActionInfoList.Add(_relateAction);

            _m_lActionList = new List<ALCreatureStateActionObj>();
            actionSpeed = _actionSpeed;
        }
        public _AALBaseCharacterActionState(_AALBasicCreatureControl _creature, float _actionSpeed, List<ALSOCreatureActionInfo> _relateActionList)
            : base(_creature)
        {
            _m_iCharacterStateSerialize = g_serialize++;

            _m_fStateStartTime = 0;

            _m_lRelateActionInfoList = new List<ALSOCreatureActionInfo>();
            _m_lRelateActionInfoList.AddRange(_relateActionList);

            _m_lActionList = new List<ALCreatureStateActionObj>();
            actionSpeed = _actionSpeed;
        }

        public int serialize { get { return _m_iCharacterStateSerialize; } }
        public float stateStartTime { get { return _m_fStateStartTime; } }

        /****************
         * 进入当前状态
         **/
        public override void onEnterState()
        {
            _m_fStateStartTime = Time.time;

            //初始化所有行为队列
            for (int i = 0; i < _m_lRelateActionInfoList.Count; i++)
            {
                _addAction(_m_lRelateActionInfoList[i]);
            }

            //检测当前移动状态机
            getCreatureControl().getMoveStateMachine().checkCurState();

            //调用事件函数
            _onEnterState();
        }

        /****************
         * 离开当前状态
         **/
        public override void onLeaveState(_AALBaseCharacterState _newState)
        {
            //clear all action
            clearAction();

            //调用事件函数
            _onLeaveState(_newState);
        }

        /** 在Action结束时处理 */
        public void onActionEnd(ALBaseCreatureActionObj _actionObj)
        {
            if (!(_actionObj is ALCreatureStateActionObj))
                return;

            ALCreatureStateActionObj stateActionObj = (ALCreatureStateActionObj)_actionObj;

            _m_lActionList.Remove(stateActionObj);
        }

        /***************
         * 设置动作速度
         **/
        public void setActionSpeed(float _actionSpeed)
        {
            actionSpeed = _actionSpeed;

            //clear all action
            for (int i = 0; i < _m_lActionList.Count; i++)
            {
                ALBaseCreatureActionObj actionObj = _m_lActionList[i];
                if (null == actionObj)
                    continue;

                actionObj.setActionTimeScale(actionSpeed);
            }
        }

        /****************
         * 清空所有行为动作
         **/
        public void clearAction()
        {
            //clear all action
            for (int i = 0; i < _m_lActionList.Count; i++)
            {
                ALBaseCreatureActionObj actionObj = _m_lActionList[i];
                if (null == actionObj)
                    continue;

                _m_cCharacterControl.getActionControler().removeAction(actionObj);
            }

            _m_lActionList.Clear();
        }

        /****************
         * 切换为指定动作
         **/
        public void swapAction(ALSOCreatureActionInfo _newAction)
        {
            if (null == _newAction)
                return;

            //clear all action
            clearAction();

            //添加新动作
            _addAction(_newAction);
        }

        /**************
         * 获取当前有效行为个数
         **/
        protected int _getCurActionCount()
        {
            return _m_lActionList.Count;
        }

        /********************
         * 为本状态添加一个动作行为
         **/
        protected void _addAction(ALSOCreatureActionInfo _actionInfo)
        {
            if (null == _actionInfo)
                return;

            ALCreatureStateActionObj actionObj = _m_cCharacterControl.getActionControler().addAction(this, _actionInfo, actionSpeed);

            if (null == actionObj)
                return;

            //add into list
            _m_lActionList.Add(actionObj);
            //reg response deal
            actionObj.regResponseObj(this);
        }

        /****************
         * 返回检测后的实际状态，返回null为无新状态
         **/
        public abstract _AALBaseCharacterActionState checkState();

        /********************
         * 尝试切换到新的状态，返回新的状态，当无法切换时，返回null
         * 部分状态，如打断状态，会将新的打断附加到原先存在的打断状态中
         */
        public abstract _AALBaseCharacterActionState changeToState(_AALBaseCharacterActionState _newState);

        /****************
         * 获取当前状态类型
         **/
        public abstract ALCharacterActionStateType getState();

        /****************
         * 返回是否允许移动
         **/
        public abstract bool canMove();

        /****************
         * 返回是否允许移动
         **/
        public abstract void _onEnterState();

        /****************
         * 返回是否允许移动
         **/
        public abstract void _onLeaveState(_AALBaseCharacterState _newState);
    }
}
#endif
