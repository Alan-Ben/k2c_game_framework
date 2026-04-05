
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 进行平衡变化的 mono
    /// </summary>
    public class NPSmoothLocalTransformMono : MonoBehaviour
    {
        [ALHeader("坐标的平滑阻尼系数")]
        public float positionSmoothTime = 0.15f;
        [ALHeader("旋转的插值系数")]
        public float rotationLerp = 0.7f;
        [ALHeader("缩放的平滑阻尼系数")]
        public float scaleSmoothTime = 0.15f;
        
        // 目标的坐标和平滑阻尼速度
        private Vector3 _m_targetPosition;
        private Vector3 _m_positionSmoothDampVelocity;

        // 目标的旋转角
        private Quaternion _m_targetRotation;

        // 目标的缩放值和平滑阻尼系数
        private Vector3 _m_targetScale;
        private Vector3 _m_scaleSmoothDampVelocity;

        private UpdateTransform _m_updateTask;

        /// <summary>
        /// 设置这个单位的坐标
        /// </summary>
        public void setLocalPosition(Vector3 _localPosition, bool _isFade = true)
        {
            _m_targetPosition = _localPosition;
            
            if (!_isFade)
                transform.localPosition = _m_targetPosition;
            else
                _tryUpdate();
        }
        /// <summary>
        /// 设置这个单位的旋转
        /// </summary>
        public void setLocalRotation(Quaternion _targetRotation, bool _isFade = true)
        {
            _m_targetRotation = _targetRotation;

            if (!_isFade)
                transform.localRotation = _m_targetRotation;
            else
                _tryUpdate();
        }
        /// <summary>
        /// 设置这个单位的缩放
        /// </summary>
        public void setLocalScale(Vector3 _localScale, bool _isFade = true)
        {
            _m_targetScale = _localScale;

            if (!_isFade)
                transform.localScale = _m_targetScale;
            else
                _tryUpdate();
        }
        /// <summary>
        /// 直接完成过度 
        /// </summary>
        public void completeSmooth()
        {
            transform.localPosition = _m_targetPosition;
            transform.localRotation = _m_targetRotation;
            transform.localScale = _m_targetScale;
        }

        /// <summary>
        /// 尝试处理刷新缓冲运动的任务
        /// </summary>
        protected  void _tryUpdate()
        {
            if (null == _m_updateTask)
                _m_updateTask = new UpdateTransform(this);

            //如果在运行则不处理
            if (_m_updateTask.isRunning)
                return;

            _m_updateTask.deal();
        }

        private void Awake()
        {
            // 赋值初始值
            _m_targetPosition = transform.localPosition;
            _m_targetRotation = transform.localRotation;
            _m_targetScale = transform.localScale;

            _m_updateTask = null;
        }
        private void OnDestroy()
        {
            _m_updateTask = null;
        }

        /// <summary>
        /// 内部的 task 任务
        /// </summary>
        private class UpdateTransform : _IALBaseMonoTask
        {
            private NPSmoothLocalTransformMono _m_mono;
            private bool _m_isRunning;

            public UpdateTransform(NPSmoothLocalTransformMono  _mono)
            {
                _m_mono = _mono;
                _m_isRunning = false;
            }
            
            /// <summary>
            /// 是否正在运行
            /// </summary>
            public bool isRunning { get { return _m_isRunning; } }

            public void deal()
            {
                // transform 被销毁了
                if (_m_mono == null)
                    return ;

                Transform trans = _m_mono.transform;
                if (null == trans)
                    return;

                // 已经变换结束了
                if(NPGameUtility.Approximately(trans.localPosition, _m_mono._m_targetPosition) &&
                        NPGameUtility.Approximately(trans.localRotation, _m_mono._m_targetRotation) &&
                        NPGameUtility.Approximately(trans.localScale, _m_mono._m_targetScale))
                {
                    _m_isRunning = false;
                    return;
                }

                _m_isRunning = true;
                trans.localPosition = Vector3.SmoothDamp
                (
                    trans.localPosition,
                    _m_mono._m_targetPosition,
                    ref _m_mono._m_positionSmoothDampVelocity,
                    _m_mono.positionSmoothTime,
                    float.PositiveInfinity,
                    Time.unscaledDeltaTime
                );
                trans.localRotation = Quaternion.Slerp(trans.localRotation, _m_mono._m_targetRotation, _m_mono.rotationLerp);
                trans.localScale = Vector3.SmoothDamp
                (
                    trans.localScale,
                    _m_mono._m_targetScale,
                    ref _m_mono._m_scaleSmoothDampVelocity,
                    _m_mono.scaleSmoothTime,
                    float.PositiveInfinity,
                    Time.unscaledDeltaTime
                );
                
                // 下一帧接着执行
                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }
    }
}