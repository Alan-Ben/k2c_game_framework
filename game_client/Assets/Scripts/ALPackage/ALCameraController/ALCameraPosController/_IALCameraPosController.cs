using System;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 摄像头的位置控制对象接口类
 **/
namespace ALPackage
{
    public interface _IALCameraPosController
    {
        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        _IALCameraPosController checkUpdate();

        /** 获取当前摄像头位置 */
        Vector3 cameraPos { get; }
        /** 获取当前摄像头移动的速度 */
        Vector3 cameraMoveSpeed { get; }
        /** 获取摄像头的目标位置 */
        Vector3 cameraTargetPos { get; }
        /** 获取当前摄像头是否在移动 */
        bool isMoving { get; }
        /** 当前摄像头位置操作的优先级 */
        int priority { get; }
    }
}
