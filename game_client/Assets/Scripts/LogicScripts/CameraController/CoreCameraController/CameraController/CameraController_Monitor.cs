using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public sealed partial class CameraController
    {
        [NotNull][ItemNotNull] private readonly List<_ICameraMonitor> _m_cameraMonitorList = new List<_ICameraMonitor>();
        
        /// <summary>
        /// 注册一个相机监视器
        /// </summary>
        public void registerCameraMonitor(_ICameraMonitor _cameraMonitor)
        {
            if (_cameraMonitor == null)
                return;

            if (_m_cameraMonitorList.Contains(_cameraMonitor))
            {
                ALLog.Warning("[CameraController] 重复注册相机监视器");
                return;
            }
            
            _m_cameraMonitorList.Add(_cameraMonitor);
            _cameraMonitor.onEnter();
        }
        /// <summary>
        /// 注销一个相机监视器
        /// </summary>
        public void unregisterCameraMonitor(_ICameraMonitor _cameraMonitor)
        {
            if (_cameraMonitor == null)
                return;

            if (!_m_cameraMonitorList.Contains(_cameraMonitor))
            {
                ALLog.Warning("[CameraController] 注销一个未注册的相机监视器");
                return;
            }
            
            _m_cameraMonitorList.Remove(_cameraMonitor);
            _cameraMonitor.onExit();
        }
    }
}