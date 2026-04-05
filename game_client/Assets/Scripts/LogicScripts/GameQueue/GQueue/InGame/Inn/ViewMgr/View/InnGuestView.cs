using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class InnGuestView : _AALBasicLoadObj
    {
        private static readonly int State = Animator.StringToHash("state");
        private static readonly int MoveDir = Animator.StringToHash("moveDir");
        
        private enum StateType
        {
            Moving,    
            Idle,
            Serving
        }
        
        
        [NotNull] private readonly InnGuestInfo _m_guestInfo;
        private readonly NPGGoIndex _m_resIndex;
        // 客人的移动速度
        private readonly float _m_moveSpeed;
        // 当前所在的坐标
        private Vector3 _m_position;
        // 记录下自己的排队偏移值
        private Vector3 _m_queueOffset;
        private GTDMonoInnGuest _m_mono;
        // 当前的客人状态
        private StateType _m_type;
        // 当前目标位置和在队列中的位置
        private Vector3 _m_targetPosition;
        
        
        public InnGuestView([NotNull] InnGuestInfo _guestInfo, float _moveSpeed)
        {
            _m_moveSpeed = _moveSpeed;
            _m_guestInfo = _guestInfo;
            _m_resIndex = _m_guestInfo.refObj?.td_res_index;
            _m_position = Vector3.zero;
            _m_type = StateType.Idle;
        }
        

        [NotNull] public InnGuestInfo guestInfo { get { return _m_guestInfo; } }
        public NPGGoIndex resIndex { get { return _m_resIndex; } }
        public GTDMonoInnGuest mono { get { return _m_mono; } }
        public Vector3 position
        {
            get { return _m_position; }
            set
            {
                _m_position = value;
                if (_m_mono != null)
                    _m_mono.transform.position = _m_position;
            }
        }
        public bool isMoving { get { return _m_type == StateType.Moving; } }
        public Vector3 queueOffset { get { return _m_queueOffset; } set { _m_queueOffset = value; } }


        protected override void _loadOp()
        {
            MainAdditionInnTDScene.instance.createUnit<GTDMonoInnGuest>(resIndex, _m_position, _mono =>
            {
                if (_mono == null)
                {
                    _setLoadDone();
                    return;
                }

                _m_mono = _mono;
                _onInitDone();
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            _onDiscard();
            MainAdditionInnTDScene.instance.discardUnit(resIndex, _m_mono);
        }
        

        /// <summary>
        /// 移动到目标位置
        /// </summary>
        public void moveToPosition(Vector3 _targetPos)
        {
            _m_targetPosition = _targetPos;
            _m_type = StateType.Moving;
            _refreshAnimState();
        }
        public void startServing()
        {
            _m_type = StateType.Serving;
            _refreshAnimState();
        }
        public void startIdle()
        {
            _m_type = StateType.Idle;
            _refreshAnimState();
        }
        /// <summary>
        /// 更新移动
        /// </summary>
        public void updateMovement()
        {
            if (_m_type != StateType.Moving || _m_mono == null)
                return;

            float distance = Vector3.Distance(position, _m_targetPosition);
            float moveDistance = _m_moveSpeed * Time.deltaTime;
            
            // Check if we can reach the target in this frame
            if (distance <= moveDistance)
            {
                // Directly set to target position to avoid overshooting
                position = _m_targetPosition;
                startIdle();
                return;
            }

            // Move towards target
            Vector3 direction = (_m_targetPosition - position).normalized;
            Vector3 newPos = position + direction * moveDistance;
            position = newPos;
        }


        private void _onInitDone()
        {
            _refreshAnimState();
        }
        private void _onDiscard()
        {
            
        }
        private void _refreshAnimState()
        {
            if (mono == null)
                return;

            if (mono.anim != null)
            {
                mono.anim.SetInteger(State, (int)_m_type);
                Vector3 vectorToTarget = _m_targetPosition - _m_position;
                float vertical = Mathf.Abs(vectorToTarget.z);
                float horizontal = Mathf.Abs(vectorToTarget.x);
                int moveDir = 0;
                if (vertical < horizontal)
                    moveDir = 1;
                mono.anim.SetInteger(MoveDir, moveDir);
            }
        }
    }
}