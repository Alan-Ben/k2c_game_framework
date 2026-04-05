using System;
using System.Collections.Generic;

using UnityEngine;

/*****************************
 * 控制正交视野的控制对象
 **/
namespace ALPackage
{
    public abstract class _AALCameraOrthographicSizeController
    {
        protected bool _m_bIsInit = false;

        public bool isInited { get { return _m_bIsInit; } }
        public void setInited() { _m_bIsInit = true; }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public abstract _AALCameraOrthographicSizeController checkUpdate();

        /** 获取当前摄像头视角 */
        public abstract float orthographicSize { get; }
        /** 获取当前摄像头视角变换的速度 */
        public abstract float orthographicSizeChgSpeed { get; }
        /** 获取摄像头的目标视角值 */
        public abstract float orthographicSizeTargetValue { get; }
        /** 获取当前摄像头视角是否在变化 */
        public abstract bool isMoving { get; }
        /** 当前摄像头位置操作的优先级 */
        public abstract int priority { get; }
    }
}
