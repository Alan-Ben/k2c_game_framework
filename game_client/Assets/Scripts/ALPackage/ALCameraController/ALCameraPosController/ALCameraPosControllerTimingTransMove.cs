using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 摄像头过渡移动位置的控制对象
 **/
namespace ALPackage
{
    public class ALCameraPosControllerTimingTransMove : _IALCameraPosController
    {
        /** 位置控制对象的数组 */
        private ALRealTimeFloatFadeController[] _m_arrPosTransController;

        /** 偏移过程中的移动优先级 */
        private int _m_iMovePriority;
        /** 重置回位时的摄像头位置 */
        private Vector3 _m_vResetPos;
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

        public ALCameraPosControllerTimingTransMove(int _priority, Vector3 _startMovePos, Vector3 _stayPos, Vector3 _srcMoveSpeed, float _moveTotalTime
                    , float _firstAccSpeedTime, float _stayTime, Vector3 _resetPos, float _fallbackTotalTime, float _fallbackAccSpeedTime)
        {
            _m_iMovePriority = _priority;
            
            //设置状态
            _m_eTransMoveState = EALCameraTimingTransMoveState.FIRST_MOVE;
            //创建第一次的控制数组
            _m_arrPosTransController = new ALRealTimeFloatFadeController[3];
            _m_arrPosTransController[0] = new ALRealTimeFloatFadeController(_startMovePos.x, _stayPos.x, _srcMoveSpeed.x, _moveTotalTime, _firstAccSpeedTime);
            _m_arrPosTransController[1] = new ALRealTimeFloatFadeController(_startMovePos.y, _stayPos.y, _srcMoveSpeed.y, _moveTotalTime, _firstAccSpeedTime);
            _m_arrPosTransController[2] = new ALRealTimeFloatFadeController(_startMovePos.z, _stayPos.z, _srcMoveSpeed.z, _moveTotalTime, _firstAccSpeedTime);

            //设置其他相关参数
            _m_vResetPos = _resetPos;
            _m_fFallbackTotalMoveTime = _fallbackTotalTime;
            _m_fFallbackAccSpeedTime = _fallbackAccSpeedTime;
            _m_fFirstMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime;
            _m_fStayFallbackTime = Time.realtimeSinceStartup + _moveTotalTime + _stayTime;
            _m_fTotalMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime + _stayTime + _fallbackTotalTime;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public _IALCameraPosController checkUpdate()
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
                    Vector3 curPos = cameraPos;
                    //进入重置阶段的运动
                    _m_arrPosTransController[0] = new ALRealTimeFloatFadeController(curPos.x, _m_vResetPos.x, 0, _m_fFallbackTotalMoveTime, _m_fFallbackAccSpeedTime);
                    _m_arrPosTransController[1] = new ALRealTimeFloatFadeController(curPos.y, _m_vResetPos.y, 0, _m_fFallbackTotalMoveTime, _m_fFallbackAccSpeedTime);
                    _m_arrPosTransController[2] = new ALRealTimeFloatFadeController(curPos.z, _m_vResetPos.z, 0, _m_fFallbackTotalMoveTime, _m_fFallbackAccSpeedTime);

                    _m_eTransMoveState = EALCameraTimingTransMoveState.RESET;
                }
            }

            if (EALCameraTimingTransMoveState.RESET == _m_eTransMoveState)
            {
                if (Time.realtimeSinceStartup >= _m_fTotalMoveDoneTime)
                {
                    _m_eTransMoveState = EALCameraTimingTransMoveState.DONE;

                    //返回静态状态
                    return new ALCameraPosControllerStaticPos(_m_vResetPos);
                }
            }

            return null;
        }

        /** 获取当前摄像头位置 */
        public Vector3 cameraPos { get { return new Vector3(_m_arrPosTransController[0].curValue, _m_arrPosTransController[1].curValue, _m_arrPosTransController[2].curValue); } }
        /** 获取当前摄像头移动的速度 */
        public Vector3 cameraMoveSpeed { get { return new Vector3(_m_arrPosTransController[0].curFadeSpeed, _m_arrPosTransController[1].curFadeSpeed, _m_arrPosTransController[2].curFadeSpeed); } }
        /** 获取摄像头的目标位置 */
        public Vector3 cameraTargetPos { get { return _m_vResetPos; } }
        /** 获取当前摄像头是否在移动 */
        public bool isMoving { get { if (EALCameraTimingTransMoveState.DONE == _m_eTransMoveState) return false; else return true; } }
        /** 当前摄像头位置操作的优先级 */
        public int priority { get { return _m_iMovePriority; } }
    }
}
