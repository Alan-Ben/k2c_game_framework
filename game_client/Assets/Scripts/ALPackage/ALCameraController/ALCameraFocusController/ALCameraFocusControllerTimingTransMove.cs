using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 摄像头焦点位置过渡移动位置的控制对象
 **/
namespace ALPackage
{
    public class ALCameraFocusControllerTimingTransMove : _IALCameraFocusController
    {
        /** 位置控制对象的数组 */
        private ALRealTimeFloatFadeController[] _m_arrFocusPointTransController;

        /** 偏移过程中的移动优先级 */
        private int _m_iPriority;
        /** 重置回位时的摄像头焦点位置 */
        private Vector3 _m_vResetFocusPoint;
        /** 回位的相关参数 */
        private float _m_fFallbackTotalMoveTime;
        private float _m_fFallbackAccSpeedTime;
        /** 第一次运动的截至时间 */
        private float _m_fFirstMoveDoneTime;
        /** 在第一次移动到目标点后停止的截至时间 */
        private float _m_fStayFallbackTime;
        /** 总的完结时间 */
        private float _m_fTotalMoveDoneTime;

        /** 当前的移动状态 */
        private EALCameraTimingTransMoveState _m_eTransMoveState;

        public ALCameraFocusControllerTimingTransMove(int _priority, Vector3 _startFocusPoint, Vector3 _stayPoint, Vector3 _srcMoveSpeed, float _moveTotalTime
                    , float _firstAccSpeedTime, float _stayTime, Vector3 _resetFocusPos, float _fallbackTotalTime, float _fallbackAccSpeedTime)
        {
            _m_iPriority = _priority;
            
            //设置状态
            _m_eTransMoveState = EALCameraTimingTransMoveState.FIRST_MOVE;
            //创建第一次的控制数组
            _m_arrFocusPointTransController = new ALRealTimeFloatFadeController[3];
            _m_arrFocusPointTransController[0] = new ALRealTimeFloatFadeController(_startFocusPoint.x, _stayPoint.x, _srcMoveSpeed.x, _moveTotalTime, _firstAccSpeedTime);
            _m_arrFocusPointTransController[1] = new ALRealTimeFloatFadeController(_startFocusPoint.y, _stayPoint.y, _srcMoveSpeed.y, _moveTotalTime, _firstAccSpeedTime);
            _m_arrFocusPointTransController[2] = new ALRealTimeFloatFadeController(_startFocusPoint.z, _stayPoint.z, _srcMoveSpeed.z, _moveTotalTime, _firstAccSpeedTime);

            //设置其他相关参数
            _m_vResetFocusPoint = _resetFocusPos;
            _m_fFallbackTotalMoveTime = _fallbackTotalTime;
            _m_fFallbackAccSpeedTime = _fallbackAccSpeedTime;
            _m_fFirstMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime;
            _m_fStayFallbackTime = Time.realtimeSinceStartup + _moveTotalTime + _stayTime;
            _m_fTotalMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime + _stayTime + _fallbackTotalTime;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public _IALCameraFocusController checkUpdate()
        {
            //根据不同阶段进行判断
            if (EALCameraTimingTransMoveState.FIRST_MOVE == _m_eTransMoveState)
            {
                if (Time.realtimeSinceStartup >= _m_fFirstMoveDoneTime)
                {
                    _m_eTransMoveState = EALCameraTimingTransMoveState.STAY;
                }
            }

            //判断是否在停留阶段
            if (EALCameraTimingTransMoveState.STAY == _m_eTransMoveState)
            {
                //此时判断时间是否到了需要重置的阶段
                if (Time.realtimeSinceStartup >= _m_fStayFallbackTime)
                {
                    Vector3 curPoint = focusPoint;
                    //进入重置阶段的运动
                    _m_arrFocusPointTransController[0] = new ALRealTimeFloatFadeController(curPoint.x, _m_vResetFocusPoint.x, 0, _m_fFallbackTotalMoveTime, _m_fFallbackAccSpeedTime);
                    _m_arrFocusPointTransController[1] = new ALRealTimeFloatFadeController(curPoint.y, _m_vResetFocusPoint.y, 0, _m_fFallbackTotalMoveTime, _m_fFallbackAccSpeedTime);
                    _m_arrFocusPointTransController[2] = new ALRealTimeFloatFadeController(curPoint.z, _m_vResetFocusPoint.z, 0, _m_fFallbackTotalMoveTime, _m_fFallbackAccSpeedTime);

                    _m_eTransMoveState = EALCameraTimingTransMoveState.RESET;
                }
            }

            if (EALCameraTimingTransMoveState.RESET == _m_eTransMoveState)
            {
                if (Time.realtimeSinceStartup >= _m_fTotalMoveDoneTime)
                {
                    _m_eTransMoveState = EALCameraTimingTransMoveState.DONE;

                    //返回静态状态
                    return new ALCameraFocusControllerStaticPos(_m_vResetFocusPoint);
                }
            }

            return null;
        }

        /** 获取当前摄像头位置 */
        public Vector3 focusPoint { get { return new Vector3(_m_arrFocusPointTransController[0].curValue, _m_arrFocusPointTransController[1].curValue, _m_arrFocusPointTransController[2].curValue); } }
        /** 获取当前摄像头移动的速度 */
        public Vector3 focusPointMoveSpeed { get { return new Vector3(_m_arrFocusPointTransController[0].curFadeSpeed, _m_arrFocusPointTransController[1].curFadeSpeed, _m_arrFocusPointTransController[2].curFadeSpeed); } }
        /** 获取摄像头的目标位置 */
        public Vector3 targetFocusPoint { get { return _m_vResetFocusPoint; } }
        /** 获取当前摄像头是否在移动 */
        public bool isMoving { get { if (EALCameraTimingTransMoveState.DONE == _m_eTransMoveState) return false; else return true; } }
        /** 当前摄像头位置操作的优先级 */
        public int priority { get { return _m_iPriority; } }
    }
}
