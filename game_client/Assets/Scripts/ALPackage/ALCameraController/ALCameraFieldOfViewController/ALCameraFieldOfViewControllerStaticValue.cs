using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 摄像头固定位置的控制对象
 **/
namespace ALPackage
{
    public class ALCameraFieldOfViewControllerStaticValue : _IALCameraFieldOfViewController
    {
        /** 固定视角宽度的信息 */
        private float _m_fFieldOfView;

        public ALCameraFieldOfViewControllerStaticValue(float _fieldOfView)
        {
            _m_fFieldOfView = _fieldOfView;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public _IALCameraFieldOfViewController checkUpdate()
        {
            return null;
        }

        /** 获取当前摄像头视角 */
        public float fieldOfView { get { return _m_fFieldOfView; } }
        /** 获取当前摄像头视角变换的速度 */
        public float fieldOfViewChgSpeed { get { return 0; } }
        /** 获取摄像头的目标视角值 */
        public float fieldOfViewTargetValue { get { return _m_fFieldOfView; } }
        /** 获取当前摄像头视角是否在变化 */
        public bool isMoving { get { return false; } }
        /** 当前摄像头位置操作的优先级 */
        public int priority { get { return 0; } }
    }
}
