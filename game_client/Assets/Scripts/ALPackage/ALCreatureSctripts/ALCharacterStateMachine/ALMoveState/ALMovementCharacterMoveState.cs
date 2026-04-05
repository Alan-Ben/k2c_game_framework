using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

#if AL_CREATURE_SYS
/*****************************
 * 角色状态对象，此对象存储了角色状态的相关信息
 **/

namespace ALPackage
{
    public class ALMovementCharacterMoveState : _AALBaseCharacterMoveState
    {
        /** 对象当前位置 */
        private Vector3 _m_vInitPos;
        /** 时间标记 */
        private float _m_fStartMoveTime;
        /** 运动过程的相关信息 */
        private ActionMovementInfo _m_miMovementInfo;

        /** 相关移动的参数，如距离，方向的模式向量 */
        private Vector3 _m_vNormalizeDirection;
        private float _m_fDistance;

        /** 监听事件对象队列 */
        private List<_IActionMovementListener> _m_lListenerList;
        /** 是否已经完全离开本状态 */
        private bool _m_bLeft;

        public ALMovementCharacterMoveState(_AALBasicCreatureControl _creature, ActionMovementInfo _movementInfo)
            : base(_creature)
        {
            //获取当前所在位置
            _m_vInitPos = _creature.transform.position;
            _m_fStartMoveTime = Time.time;

            _m_miMovementInfo = _movementInfo;

            _m_vNormalizeDirection = _m_miMovementInfo.position - _m_vInitPos;
            _m_fDistance = _m_vNormalizeDirection.magnitude;
            _m_vNormalizeDirection.Normalize();

            _m_lListenerList = new List<_IActionMovementListener>();
            _m_bLeft = false;
        }

        /********************
         * 返回状态对象的枚举类型
         * @return
         */
        public override ALCharacterMoveStateType getState()
        {
            return ALCharacterMoveStateType.MOVEMENT;
        }

        /********************
         * 检查本状态是否有效
         * @return
         */
        public override _AALBaseCharacterMoveState checkState()
        {
            //在运动时间内则有效
            if (Time.time >= (_m_fStartMoveTime + _m_miMovementInfo.processTime))
                return new ALIdleCharacterMoveState(getCreatureControl());

            return null;
        }

        /********************
         * change into the new state and return the new state
         * if null then can not change to new state
         * @param newState
         * @return
         */
        public override _AALBaseCharacterMoveState changeToState(_AALBaseCharacterMoveState _newState)
        {
            //只有同时位移状态才进行切换
            if (_newState.getState() == ALCharacterMoveStateType.MOVEMENT)
                return _newState;

            if (canBlend)
                return _newState;

            //本移动状态无法主动切换
            return null;
        }

        /**********************
         * 在角色进行每帧处理时处理的函数
         **/
        public override void onUpdate()
        {
            //角色向指定位置移动,根据时间以及位置进行判断
            Vector3 curPos = _m_vInitPos + ((_m_fDistance * (Time.time - _m_fStartMoveTime) / _m_miMovementInfo.processTime) * _m_vNormalizeDirection);
            //设置角色位置
            getCreatureControl().transform.position = curPos;
        }

        /******************
         * 添加监听对象
         **/
        public void regMovementListener(_IActionMovementListener _listener)
        {
            _m_lListenerList.Add(_listener);
        }

        /******************
         * 处理位移行为完毕的事件
         **/
        public void dealMovementDone(ALBaseCreatureActionObj _actionObj)
        {
            for (int i = 0; i < _m_lListenerList.Count; i++)
            {
                _IActionMovementListener listener = _m_lListenerList[i];
                if (null == listener)
                    continue;

                listener.onMovementDone(_actionObj);
            }

            //清除列表
            _m_lListenerList.Clear();
        }

        /****************
         * 判断位移是否完成
         **/
        public bool isMovementDone()
        {
            //返回位移状态是否完全退出
            return _m_bLeft;
        }

        /**********************
         * 进入本状态的子类处理函数
         **/
        protected override void _onEnterState()
        {
        }

        /**********************
         * 离开本状态的子类处理函数
         **/
        protected override void _onLeaveState()
        {
            //射线检测位置处的合法高度位置
            Vector3 curPos = getCreatureControl().transform.position;
            curPos.y = curPos.y + 1f;

            Vector3 legalPos = ALUnityCommon.getInstanceGroundPoint(curPos);
            getCreatureControl().transform.position = legalPos;

            //设置确认离开
            _m_bLeft = true;

            //处理位移完成消息
            getCreatureControl().getEventDealer().onMovementDone(_m_miMovementInfo);
        }

        /**********************
         * 在行动状态改变时，调用的事件函数，用于刷新动作
         **/
        protected override void _onMoveModeChg()
        {
        }
    }
}
#endif
