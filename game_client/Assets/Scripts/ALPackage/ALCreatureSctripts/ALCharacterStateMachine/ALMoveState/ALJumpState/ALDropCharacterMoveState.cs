using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
/*****************************
 * 角色状态对象，此对象存储了角色状态的相关信息
 **/

namespace ALPackage
{
    public class ALDropCharacterMoveState : _AALBaseCharacterMoveState
    {
        private ALCharacterJumpStateType _m_eJumpState;
        /** 跳跃方向，在进入本状态时记录的信息 */
        private Vector3 _m_vDropForward;
        /** 跳跃前进的速度 */
        private float _m_fJumpSpeed;
        /** 起跳的时间点 */
        private float _m_fStartJumpingTime;
        /** 起跳时Y轴方向速度 */
        private float _m_fStartJumpYSpeed;
        /** 起跳时Y轴位置信息 */
        private float _m_fStartJumpYPos;
        /** 落地的时间点 */
        private float _m_fFinishTime;
        /** 原先主体对象的爬坡高度 */
        private float _m_fSrcSlopeLimit;

        /** 起跳过程中的动作控制对象 */
        private ALCreatureMoveStateAnimationSession _m_asJumpingAnimationSession;
        /** 落地的动作控制对象 */
        private ALCreatureMoveStateAnimationSession _m_asLandedAnimationSession;

        /** 临时存储移动角度的变量 */
        private Vector3 _m_vTmpEular;

        public ALDropCharacterMoveState(_AALBasicCreatureControl _creature)
            : base(_creature)
        {
            _m_eJumpState = ALCharacterJumpStateType.JUMPING;
            _m_vDropForward = getCreatureControl().moveForward;
            _m_fJumpSpeed = getCreatureControl().moveSpeed / 2;
            _m_fStartJumpingTime = 0;
            _m_fFinishTime = 0;
            _m_fStartJumpingTime = 0;

            //设置掉落初期的相关参数
            _m_fStartJumpYPos = _creature.transform.position.y;
            _m_fStartJumpYSpeed = 0;

            _m_asJumpingAnimationSession = null;
            _m_asLandedAnimationSession = null;
        }
        public ALDropCharacterMoveState(_AALBasicCreatureControl _creature, Vector3 _forward, float _moveSpeed)
            : base(_creature)
        {
            _m_eJumpState = ALCharacterJumpStateType.JUMPING;
            _m_vDropForward = _forward;
            _m_fJumpSpeed = _moveSpeed / 2;
            _m_fStartJumpingTime = 0;
            _m_fFinishTime = 0;
            _m_fStartJumpingTime = 0;

            //设置掉落初期的相关参数
            _m_fStartJumpYPos = _creature.transform.position.y;
            _m_fStartJumpYSpeed = 0;

            _m_asJumpingAnimationSession = null;
            _m_asLandedAnimationSession = null;
        }

        /********************
         * 返回状态对象的枚举类型
         * @return
         */
        public override ALCharacterMoveStateType getState()
        {
            return ALCharacterMoveStateType.JUMP;
        }

        /********************
         * 检查本状态是否有效
         * @return
         */
        public override _AALBaseCharacterMoveState checkState()
        {
            if (ALCharacterJumpStateType.JUMPING == _m_eJumpState)
            {
                //判断角色是否站在地面上，否则进行掉落处理
                //当角色在起跳后的过程中，并且已经落地则重新进入新的状态
                if (getCreatureControl().getFlowHeight(ALGlobalControl.instance.mapItemLayerMask) < ALSOGlobalSetting.Instance.stopDropHeight)
                {
                    ALJumpAnimationInfo jumpInfo = getParent().getMoveAnimationModeObj().jumpAnimationInfo;

                    _m_eJumpState = ALCharacterJumpStateType.LANDED;

                    _m_fFinishTime = Time.time + (null == jumpInfo ? 0 : jumpInfo.landedProcessTime);

                    //退出空中循环动画
                    getCreatureControl().getAnimationControler().removeAnimation(_m_asJumpingAnimationSession);
                    //添加落地后动画
                    _m_asLandedAnimationSession = getCreatureControl().getAnimationControler().addMoveStateAnimation(this, jumpInfo.landAnimation, 1.0f);
                }
            }
            else if (ALCharacterJumpStateType.LANDED == _m_eJumpState)
            {
                if (Time.time >= _m_fFinishTime)
                {
                    //超出结束时间直接结束动画
                    getCreatureControl().getAnimationControler().removeAnimation(_m_asLandedAnimationSession);

                    return new ALIdleCharacterMoveState(getCreatureControl());
                }
            }

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
            //位移类技能直接返回
            if (_newState.getState() == ALCharacterMoveStateType.MOVEMENT)
                return _newState;

            if (canBlend)
                return _newState;

            return null;
        }

        /**********************
         * 在角色进行每帧处理时处理的函数
         **/
        public override void onUpdate()
        {
            //角色向指定方向移动，移动操作在起跳过程中是一定需要操作的
            if (ALCharacterJumpStateType.LANDED != _m_eJumpState)
                getCreatureControl().getCharacterController().Move(Time.deltaTime * _m_vDropForward * _m_fJumpSpeed);

            if (ALCharacterJumpStateType.JUMPING == _m_eJumpState)
            {
                float jumpingTime = Time.time - _m_fStartJumpingTime;
                //跳跃过程中需要重新计算对象的Y轴位置
                float yDis = (_m_fStartJumpYSpeed * jumpingTime) - (4.9f * jumpingTime * jumpingTime);
                float targetYPos = _m_fStartJumpYPos + yDis;

                //调用位移函数，避免穿出碰撞体
                getCreatureControl().getCharacterController().Move(new Vector3(0, targetYPos - getCreatureControl().transform.position.y, 0));
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
            ALJumpAnimationInfo jumpInfo = getParent().getMoveAnimationModeObj().jumpAnimationInfo;
            _m_fStartJumpingTime = Time.time + (null == jumpInfo ? 0 : jumpInfo.jumpUpTime);

            //进入起跳状态则切换相关动画
            _m_asJumpingAnimationSession = getCreatureControl().getAnimationControler().addMoveStateAnimation(this, jumpInfo.jumpingLoopAnimation, 1.0f);

            //设置玩家的爬坡高度为0
            _m_fSrcSlopeLimit = getCreatureControl().getCharacterController().slopeLimit;
            getCreatureControl().getCharacterController().slopeLimit = 0;

            //调用事件函数
            _AALCharacterEventDealer eventDealer = getCreatureControl().getEventDealer();
            if (null != eventDealer)
                eventDealer.onEnterDrop(_m_vDropForward);
        }

        /**********************
         * 离开本状态的子类处理函数
         **/
        protected override void _onLeaveState()
        {
            //移除动作
            if (null != _m_asJumpingAnimationSession)
                getCreatureControl().getAnimationControler().removeAnimation(_m_asJumpingAnimationSession);
            if (null != _m_asLandedAnimationSession)
                getCreatureControl().getAnimationControler().removeAnimation(_m_asLandedAnimationSession);

            //设置玩家的爬坡高度为45
            getCreatureControl().getCharacterController().slopeLimit = _m_fSrcSlopeLimit;

            _AALCharacterEventDealer eventDealer = getCreatureControl().getEventDealer();
            if (null != eventDealer)
                eventDealer.onDropDone();
        }

        /**********************
         * 在行动状态改变时，调用的事件函数，用于刷新动作
         **/
        protected override void _onMoveModeChg()
        {
            //当角色开始跳跃时与是否能移动已经无关，定会起跳
            if (ALCharacterJumpStateType.JUMPING == _m_eJumpState)
            {
                ALJumpAnimationInfo jumpInfo = getParent().getMoveAnimationModeObj().jumpAnimationInfo;

                //移除动作
                if (null != _m_asJumpingAnimationSession)
                    getCreatureControl().getAnimationControler().removeAnimation(_m_asJumpingAnimationSession);
                //增加新的准备动作
                _m_asJumpingAnimationSession = getCreatureControl().getAnimationControler().addMoveStateAnimation(this, jumpInfo.jumpingLoopAnimation, 1.0f);
            }
            else if (ALCharacterJumpStateType.LANDED == _m_eJumpState)
            {
                ALJumpAnimationInfo jumpInfo = getParent().getMoveAnimationModeObj().jumpAnimationInfo;

                //移除动作
                if (null != _m_asLandedAnimationSession)
                    getCreatureControl().getAnimationControler().removeAnimation(_m_asLandedAnimationSession);
                //增加新的准备动作
                _m_asLandedAnimationSession = getCreatureControl().getAnimationControler().addMoveStateAnimation(this, jumpInfo.landAnimation, 1.0f);
            }
        }
    }
}
#endif
