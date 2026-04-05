using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class CameraControlDragInfo
    {
        public Vector3 dragDis;
        public float opTimeTick;
        public float dragTime;

        public CameraControlDragInfo()
        {
            dragDis = Vector3.zero;
            opTimeTick = 0;
            dragTime = 0f;
        }

        public void setValue(Vector3 _dragDis)
        {
            dragDis = _dragDis;
            opTimeTick = Time.unscaledTime;
            dragTime = Time.unscaledDeltaTime;
        }
        public void setValue(CameraControlDragInfo _info)
        {
            dragDis = _info.dragDis;
            opTimeTick = _info.opTimeTick;
            dragTime = _info.dragTime;
        }
    }
    public class CameraMoveDragController : _ACameraMoveController
    {
        //拖拽操作计算惯性的时间
        private const float _k_fDragCalTime = 0.2f;
        //最大的惯性速度
        private const float _k_fMaxVelocitySpeex = 120f;
        
        [NotNull] private readonly CameraControlDragInfo _m_lPreDragInfo;
        [NotNull] private readonly CameraControlDragInfo _m_lDragInfo;
        private readonly _ALogicPlane2DPosGetter _m_posGetter;
        private readonly float _m_airFriction;
        private readonly float _m_slideFriction;

        private Vector3 _m_cameraPos;
        private Vector2 _m_lastScreenPos;
        
        public CameraMoveDragController(_ALogicPlane2DPosGetter _groundPosGetter, float _airFriction = 0.3f, float _slideFriction = 10f)
        {
            _m_posGetter = _groundPosGetter;
            _m_airFriction = _airFriction;
            _m_slideFriction = _slideFriction;

            _m_lPreDragInfo = new CameraControlDragInfo();
            _m_lDragInfo = new CameraControlDragInfo();
        }

        protected override void onStart(Vector3 _cameraPos, Vector3 _focusPos, Vector3 _cameraPosBeforeLimitation, Vector3 _cameraFocusPosBeforeLimitation, _APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            _m_cameraPos = _cameraPosBeforeLimitation;
        }

        public void beginDrag(Vector2 _screenPos)
        {
            if (_m_posGetter == null)
                return;
            
            // 获取当前拖拽开始时地面的坐标
            _m_lastScreenPos = _screenPos;

            // 清空拖拽操作
            _m_lPreDragInfo.setValue(Vector3.zero);
            _m_lDragInfo.setValue(Vector3.zero);
        }

        public void drag(Vector2 _screenPos)
        {
            if (_m_posGetter == null)
                return;

            _m_posGetter.intersectionWorldPointWithWorldRay(CameraController.instance.controlCamera.ScreenPointToRay(_m_lastScreenPos), out Vector3 lastIntersectionPos);
            _m_posGetter.intersectionWorldPointWithWorldRay(CameraController.instance.controlCamera.ScreenPointToRay(_screenPos), out Vector3 intersectionPos);
            _m_lastScreenPos = _screenPos;

            Vector3 delta = intersectionPos - lastIntersectionPos;
            _m_cameraPos -= delta;
            
            _m_lPreDragInfo.setValue(_m_lDragInfo);
            _m_lDragInfo.setValue(delta);
        }

        public void endDrag()
        {
            //根据最后一帧距离计算最后的拖拽  加速度
            Vector3 dragDirection = Vector3.zero;
            float velocitySummation = _calculateDragSpeed(ref dragDirection);
            if(velocitySummation > _k_fMaxVelocitySpeex / CameraController.instance.inverseProportion)
                velocitySummation = _k_fMaxVelocitySpeex / CameraController.instance.inverseProportion;

            //当惯性超出一定距离则进行处理
            if(velocitySummation > 0f)
            {
                // todo: 底层加入 controller 的有效序列号判断，避免这个 controller 已经失效了，但是还影响到了 CameraController 的运行
                CameraController.instance.setCameraMoveController(new CameraMoveInertiaController(-dragDirection, velocitySummation, _m_airFriction, _m_slideFriction));
            }
            else
            {
                setMovingDone();
            }
        }

        public void setDone()
        {
            setMovingDone();
        }
        
        public override Vector3 updateCameraPos(_APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            return _m_cameraPos;
        }
        
        /**************
         * 通过所有操作节点，计算当前的滑动操作速度
         **/
        protected float _calculateDragSpeed(ref Vector3 _direction)
        {
            //判断时间是否有效
            if(Time.unscaledTime - _m_lPreDragInfo.opTimeTick > _k_fDragCalTime)
                return 0f;

            float speed_cur = _m_lDragInfo.dragDis.magnitude;
            float speed_pre = _m_lPreDragInfo.dragDis.magnitude;
            _direction = _m_lDragInfo.dragDis;
            if(speed_pre > 0)
                _direction += _m_lPreDragInfo.dragDis;

            //方向模态化
            _direction.Normalize();

            return (((_m_lDragInfo.dragTime > 0 ? (speed_cur / _m_lDragInfo.dragTime) : 0) + (_m_lPreDragInfo.dragTime > 0 ? (speed_pre / _m_lPreDragInfo.dragTime) : 0))) * 0.5f;
        }
    }
}