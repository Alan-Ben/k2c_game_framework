
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 相机透视下的视野范围以平滑阻尼的方式变为目标值的控制器
    /// </summary>
    public class CameraFieldOfViewSmoothDampController : _ACameraFieldOfViewController
    {
        private readonly float _m_targetOrthographicSize;
        private readonly float _m_smoothTime;
        
        private float _m_fieldOfView;
        private float _m_smoothDampVelocity;

        public CameraFieldOfViewSmoothDampController(float _targetValue, float _smoothTime)
        {
            _m_targetOrthographicSize = _targetValue;
            _m_smoothTime = _smoothTime;
        }
        public CameraFieldOfViewSmoothDampController(float _targetValue)
        {
            _m_targetOrthographicSize = _targetValue;
            _m_smoothTime = GGameCommonInfo.instance.obj == null ? 0.3f : GGameCommonInfo.instance.obj.defaultSmoothDampTime;
        }

        protected override void onStart(float _cameraFieldOfView)
        {
            _m_fieldOfView = _cameraFieldOfView;
            _m_smoothDampVelocity = 0;
        }
        public override float updateFieldOfView()
        {
            _m_fieldOfView = Mathf.SmoothDamp(_m_fieldOfView, _m_targetOrthographicSize, ref _m_smoothDampVelocity, _m_smoothTime, float.PositiveInfinity, Time.deltaTime);
            return _m_fieldOfView;
        }
    }
}