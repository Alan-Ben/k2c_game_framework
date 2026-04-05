using System;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 摄像头的位置控制对象接口类
 **/
namespace ALPackage
{
    public interface _IALCameraFieldOfViewController
    {
        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        _IALCameraFieldOfViewController checkUpdate();

        /** 获取当前摄像头视角 */
        float fieldOfView { get; }
        /** 获取当前摄像头视角变换的速度 */
        float fieldOfViewChgSpeed { get; }
        /** 获取摄像头的目标视角值 */
        float fieldOfViewTargetValue { get; }
        /** 获取当前摄像头视角是否在变化 */
        bool isMoving { get; }
        /** 当前摄像头位置操作的优先级 */
        int priority { get; }
    }
}
