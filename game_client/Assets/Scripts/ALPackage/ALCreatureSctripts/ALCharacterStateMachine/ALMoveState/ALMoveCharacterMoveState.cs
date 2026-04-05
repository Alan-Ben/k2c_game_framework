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
    public class ALMoveCharacterMoveState : _AALBaseCharacterMoveState
    {
        /** 临时存储移动角度的变量 */
        private Vector3 _m_vTmpEular;

        public ALMoveCharacterMoveState(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
        }

        /********************
         * 返回状态对象的枚举类型
         * @return
         */
        public override ALCharacterMoveStateType getState()
        {
            return ALCharacterMoveStateType.MOVE;
        }

        /********************
         * 检查本状态是否有效
         * @return
         */
        public override _AALBaseCharacterMoveState checkState()
        {
            //当角色状态机不允许移动则返回空闲状态
            if (!getCreatureControl().getActionStateMachine().canMove())
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
            return _newState;
        }

        /**********************
         * 在角色进行每帧处理时处理的函数
         **/
        public override void onUpdate()
        {
            //角色向指定方向移动
            getCreatureControl().getCharacterController().Move(Time.deltaTime * getCreatureControl().moveForward * getCreatureControl().moveSpeed);

            //判断角色是否站在地面上，否则进行掉落处理
            float flowHeight = getCreatureControl().getFlowHeight(ALGlobalControl.instance.mapItemLayerMask);
            if (flowHeight > ALSOGlobalSetting.Instance.judgeDropHeight)
            {
                //掉落事件函数
                _AALCharacterEventDealer eventDealer = getCreatureControl().getEventDealer();
                if (null != eventDealer)
                    eventDealer.onDrop();
                return;
            }
            else if (flowHeight > 0)
            {
                //此时仅做高度纠正
                getCreatureControl().getCharacterController().Move(new Vector3(0, -flowHeight, 0));
            }

            //纠正旋转角度
            _m_vTmpEular = getCreatureControl().transform.eulerAngles;
            _m_vTmpEular.x = 0;
            _m_vTmpEular.z = 0;
            getCreatureControl().transform.eulerAngles = _m_vTmpEular;
        }

        /**********************
         * 进入本状态的子类处理函数
         **/
        protected override void _onEnterState()
        {
            _AALCharacterEventDealer eventDealer = getCreatureControl().getEventDealer();
            if (null != eventDealer)
                eventDealer.onStartMove();
        }

        /**********************
         * 离开本状态的子类处理函数
         **/
        protected override void _onLeaveState()
        {
            _AALCharacterEventDealer eventDealer = getCreatureControl().getEventDealer();
            if (null != eventDealer)
                eventDealer.onStopMove();
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
