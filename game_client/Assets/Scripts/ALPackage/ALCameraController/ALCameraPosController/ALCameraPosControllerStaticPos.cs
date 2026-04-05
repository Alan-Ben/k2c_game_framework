using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 摄像头固定位置的控制对象
 **/
namespace ALPackage
{
    public class ALCameraPosControllerStaticPos : _IALCameraPosController
    {
        /** 固定位置的信息 */
        private Vector3 _m_vPos;

        public ALCameraPosControllerStaticPos(Vector3 _pos)
        {
            _m_vPos = _pos;
        }

        /** 获取当前摄像头位置 */
        public Vector3 cameraPos { get { return _m_vPos; } }
        /** 获取当前摄像头移动的速度 */
        public Vector3 cameraMoveSpeed { get { return Vector3.zero; } }
        /** 获取摄像头的目标位置 */
        public Vector3 cameraTargetPos { get { return _m_vPos; } }
        /** 获取当前摄像头是否在移动 */
        public bool isMoving { get { return false; } }
        /** 当前摄像头位置操作的优先级 */
        public int priority { get { return 0; } }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public _IALCameraPosController checkUpdate()
        {
            return null;
        }
    }
}
