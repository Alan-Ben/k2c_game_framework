using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 摄像头焦点位置移动位置的控制对象
 **/
namespace ALPackage
{
    public class ALCameraFocusControllerTransMove : _IALCameraFocusController
    {
        /** 偏移过程中的移动优先级 */
        private int _m_iMovePriority;
        /** 位置控制对象的数组 */
        private ALRealTimeFloatFadeController[] _m_arrFocusPointTransController;
        /** 总的完结时间 */
        private float _m_fTotalMoveDoneTime;

        public ALCameraFocusControllerTransMove(int _priority, Vector3 _startFocusPoint, Vector3 _targetFocusPoint, Vector3 _srcMoveSpeed, float _moveTotalTime, float _firstAccSpeedTime)
        {
            _m_iMovePriority = _priority;
            
            //设置状态
            //创建第一次的控制数组
            _m_arrFocusPointTransController = new ALRealTimeFloatFadeController[3];
            _m_arrFocusPointTransController[0] = new ALRealTimeFloatFadeController(_startFocusPoint.x, _targetFocusPoint.x, _srcMoveSpeed.x, _moveTotalTime, _firstAccSpeedTime);
            _m_arrFocusPointTransController[1] = new ALRealTimeFloatFadeController(_startFocusPoint.y, _targetFocusPoint.y, _srcMoveSpeed.y, _moveTotalTime, _firstAccSpeedTime);
            _m_arrFocusPointTransController[2] = new ALRealTimeFloatFadeController(_startFocusPoint.z, _targetFocusPoint.z, _srcMoveSpeed.z, _moveTotalTime, _firstAccSpeedTime);

            //设置其他相关参数
            _m_fTotalMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public _IALCameraFocusController checkUpdate()
        {
            if (Time.realtimeSinceStartup >= _m_fTotalMoveDoneTime)
            {
                //返回静态状态
                return new ALCameraFocusControllerStaticPos(targetFocusPoint);
            }

            return null;
        }

        /** 获取当前摄像头位置 */
        public Vector3 focusPoint { get { return new Vector3(_m_arrFocusPointTransController[0].curValue, _m_arrFocusPointTransController[1].curValue, _m_arrFocusPointTransController[2].curValue); } }
        /** 获取当前摄像头移动的速度 */
        public Vector3 focusPointMoveSpeed { get { return new Vector3(_m_arrFocusPointTransController[0].curFadeSpeed, _m_arrFocusPointTransController[1].curFadeSpeed, _m_arrFocusPointTransController[2].curFadeSpeed); } }
        /** 获取摄像头的目标位置 */
        public Vector3 targetFocusPoint { get { return new Vector3(_m_arrFocusPointTransController[0].targetValue, _m_arrFocusPointTransController[1].targetValue, _m_arrFocusPointTransController[2].targetValue); } }
        /** 获取当前摄像头是否在移动 */
        public bool isMoving { get { return true; } }
        /** 当前摄像头位置操作的优先级 */
        public int priority { get { return _m_iMovePriority; } }
    }
}
