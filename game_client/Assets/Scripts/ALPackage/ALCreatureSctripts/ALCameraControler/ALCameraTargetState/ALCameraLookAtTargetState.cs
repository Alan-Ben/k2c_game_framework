using UnityEngine;
using System.Collections;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /******************
     * 摄像头的目标状态
     * 在每帧中操作摄像头执行需要的操作
     **/
    public class ALCameraLookAtTargetState : _IALBasicCameraTargetState
    {
        /** 目标对象 */
        private Transform _m_tTargetGO;
        private Vector3 _m_vFocusPos;

        public ALCameraLookAtTargetState(Transform _targetGO)
        {
            _m_tTargetGO = _targetGO;
            _m_vFocusPos = new Vector3(0, 0, 0);
        }
        public ALCameraLookAtTargetState(Transform _targetGO, Vector3 _focusPos)
        {
            _m_tTargetGO = _targetGO;
            _m_vFocusPos = _focusPos;
        }

        public void updateCameraState(Camera _camera)
        {
            if (null == _camera)
                return;

            if (null != _m_tTargetGO)
            {
                _camera.transform.LookAt(_m_tTargetGO.position + _m_vFocusPos);
            }
        }
    }
}
#endif
