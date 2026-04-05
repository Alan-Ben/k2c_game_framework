using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 相机自定义移动管理器
    /// </summary>
    public class CameraCustomMovementMgr
    {
        [NotNull] public static CameraCustomMovementMgr instance { get { return _g_instance ??= new CameraCustomMovementMgr(); } }
        private static CameraCustomMovementMgr _g_instance;


        // 当前正在启用的移动
        private _ICameraCustomMovement _m_currentMovement;
        

        private CameraCustomMovementMgr()
        {
        }

        
        /// <summary>
        /// 执行一个移动操作
        /// </summary>
        public void doMovement(_ICameraCustomMovement _movement)
        {
            if (_movement == null)
                return;

            if (_m_currentMovement != null && !_movement.interruptPreviousMovement)
            {
                ALLog.Sys("[CameraCustomMovementMgr] 想要执行的移动操作不会被执行，因为当前已经有一个移动操作在生效中了");
                return;
            }
            
            // 取消前一个移动操作后，激活新的移动操作
            if (_m_currentMovement != null)
                _m_currentMovement.disableMovement();
            _m_currentMovement = _movement;
            _m_currentMovement.enableMovement();
        }
        /// <summary>
        /// 取消一个移动操作
        /// </summary>
        public void cancelMovement(_ICameraCustomMovement _movement)
        {
            if (_movement == null)
                return;

            // 如果当前移动操作不是这个操作，则忽略，说明之前的移动操作没有启动成功，或是代码本身就有问题
            if (_m_currentMovement != _movement) 
                return;
            
            _m_currentMovement.disableMovement();
            _m_currentMovement = null;
        }
    }
}