using System;
using System.Collections.Generic;

using UnityEngine;

/*********************
 * 摄像头的焦点控制对象
 **/
namespace ALPackage
{
    public interface _IALCameraFocusController
    {
        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        _IALCameraFocusController checkUpdate();

        /** 焦点位置信息 */
        Vector3 focusPoint { get; }
        /** 焦点位置的移动速度 */
        Vector3 focusPointMoveSpeed { get; }
        /** 获取摄像头的目标位置 */
        Vector3 targetFocusPoint { get; }
        /** 获取当前摄像头是否在移动 */
        bool isMoving { get; }
        /** 本焦点控制对象的优先级信息 */
        int priority { get; }
    }
}
