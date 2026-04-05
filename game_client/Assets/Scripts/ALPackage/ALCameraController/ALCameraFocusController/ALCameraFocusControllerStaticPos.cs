using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 摄像头固定位置的控制对象
 **/
namespace ALPackage
{
    public class ALCameraFocusControllerStaticPos : _IALCameraFocusController
    {
        /** 固定位置的信息 */
        private Vector3 _m_vPos;

        public ALCameraFocusControllerStaticPos(Vector3 _pos)
        {
            _m_vPos = _pos;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public _IALCameraFocusController checkUpdate()
        {
            return null;
        }

        /** 焦点位置信息 */
        public Vector3 focusPoint { get { return _m_vPos; } }
        /** 焦点位置的移动速度 */
        public Vector3 focusPointMoveSpeed { get { return Vector3.zero; } }
        /** 获取摄像头的目标位置 */
        public Vector3 targetFocusPoint { get { return _m_vPos; } }
        /** 获取当前摄像头是否在移动 */
        public bool isMoving { get { return false; } }
        /** 当前摄像头位置操作的优先级 */
        public int priority { get { return 0; } }
    }
}
