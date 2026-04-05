using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public abstract class _AALBaseInterruptState
    {
        /** 父对象节点 */
        private ALInterruptActionState _m_asParentState;

        /*******************
         * 打断状态在正常情况下是听取服务端回应进行处理的，所以大部分情况下，时间都是不进行统计的
         **/
        /** 开始时间 */
        private float _m_fStartTime;
        /** 持续时间 */
        private float _m_fProgressTime;
        /** 结束时间 */
        private float _m_fEndTime;

        public _AALBaseInterruptState()
        {
            _m_asParentState = null;

            _m_fStartTime = Time.time;
            _m_fProgressTime = -1;
            _m_fEndTime = -1;
        }
        public _AALBaseInterruptState(float _progressTime)
        {
            _m_asParentState = null;

            _m_fStartTime = Time.time;
            _m_fProgressTime = _progressTime;

            if (_m_fProgressTime < 0)
                _m_fEndTime = -1;
            else
                _m_fEndTime = _m_fStartTime + _m_fProgressTime;
        }

        public ALInterruptActionState parentState { get { return _m_asParentState; } }
        public float startTime { get { return _m_fStartTime; } }
        public float progressTime { get { return _m_fProgressTime; } }
        public float endTime { get { return _m_fEndTime; } }

        /*************************
         * 设置父节点对象
         **/
        public void setParent(ALInterruptActionState _parent)
        {
            _m_asParentState = _parent;
        }

        /****************
         * 判断状态是否有效
         **/
        public bool checkState()
        {
            if (_m_fEndTime < 0)
                return true;

            if (Time.time <= _m_fEndTime)
                return true;

            return false;
        }

        /*************
         * 本打断状态的动作优先级，优先级高的将会显示，低的会在高的显示时暂时被屏蔽
         **/
        public abstract int stateActionPriority { get; }
        /*******************
         * 本打断状态的动作对象，为null表示无效，暂时不显示
         **/
        public abstract ALSOCreatureActionInfo actionInfo { get; }
        /************
         * 添加本状态对象的事件函数
         **/
        public abstract void onAdded(_AALBasicCreatureControl _creature);

        /************
         * 删除本状态对象的事件函数
         **/
        public abstract void onRemoved(_AALBasicCreatureControl _creature);
    }
}
#endif
