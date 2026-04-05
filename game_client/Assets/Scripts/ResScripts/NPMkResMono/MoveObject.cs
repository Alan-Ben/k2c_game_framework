using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MoveObject : MonoBehaviour
    {
        public float speed = 10f;
        public float acceleration = 0f;
        public float deceleration = 0f;


        #if NP_GAME
        
        private int _m_serialize;


        public void startMove(Vector3 _targetPos, Action _complete)
        {
            _m_serialize = ALSerializeOpMgr.next();
            new _TickTask(this, _targetPos, _complete).deal();
        }
        public void cancelMove()
        {
            _m_serialize = ALSerializeOpMgr.next();
        }


        private class _TickTask : _IALBaseMonoTask
        {
            [NotNull] private readonly MoveObject _m_obj;
            
            private readonly Vector3 _m_targetPos;
            private readonly Action _m_completeDelegate;
            private readonly int _m_serialize;
            
            private float _m_speed;
            private bool _m_isComplete;


            public _TickTask([NotNull] MoveObject _obj, Vector3 _targetPos, Action _complete)
            {
                _m_obj = _obj;
                _m_serialize = _obj._m_serialize;
                _m_targetPos = _targetPos;
                _m_completeDelegate = _complete;
                _m_isComplete = false;
                _m_speed = 0;
            }
            
            
            public Vector3 position { get { return _m_obj.transform.position; } set { _m_obj.transform.position = value; } }
            public float speed { get { return _m_obj.speed; } }
            public float acceleration { get { return _m_obj.acceleration; } }
            public float deceleration { get { return _m_obj.deceleration; } }


            public void deal()
            {
                if (_m_isComplete || _m_serialize != _m_obj._m_serialize)
                    return;

                Vector3 direction = _m_targetPos - position;
                float distance = direction.magnitude;

                if (distance < 0.01f)
                {
                    position = _m_targetPos;
                    _m_isComplete = true;
                    _m_completeDelegate?.Invoke();
                    return;
                }

                direction.Normalize();

                float decelerationDistance = deceleration > 0f ? (_m_speed * _m_speed) / (2f * deceleration) : 0f;
                bool inDecelerationZone = deceleration > 0f && distance <= decelerationDistance;

                if (inDecelerationZone)
                {
                    _m_speed -= deceleration * Time.deltaTime;
                    if (_m_speed < 0f)
                    {
                        _m_speed = 0f;
                    }
                }
                else if (acceleration > 0f && _m_speed < speed)
                {
                    _m_speed += acceleration * Time.deltaTime;
                    if (_m_speed > speed)
                    {
                        _m_speed = speed;
                    }
                }
                else if (acceleration <= 0f || deceleration <= 0f)
                {
                    _m_speed = speed;
                }

                float moveDistance = _m_speed * Time.deltaTime;
                if (moveDistance >= distance)
                {
                    position = _m_targetPos;
                    _m_isComplete = true;
                    _m_completeDelegate?.Invoke();
                }
                else
                {
                    position += direction * moveDistance;
                }
                
                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }
        #endif
    }
}