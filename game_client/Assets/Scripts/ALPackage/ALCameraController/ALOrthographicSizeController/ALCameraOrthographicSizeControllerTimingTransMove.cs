using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/********************************
 * 过渡控制正交视野的控制对象
 **/
namespace ALPackage
{
    public class ALCameraOrthographicSizeControllerTimingTransMove : _AALCameraOrthographicSizeController
    {
        /** 视角控制对象 */
        private ALRealTimeFloatFadeController _m_fcPosTransController;

        /** 偏移过程中的移动优先级 */
        private int _m_iMovePriority;
        /** 重置回位时的摄像头视角宽度 */
        private float _m_fResetOrthographicSize;
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

        /// <param name="_priority">偏移过程中的移动优先级</param>
        /// <param name="_startOrthographicSize">开始数值</param>
        /// <param name="_stayOrthographicSize">目标数值</param>
        /// <param name="_srcChgSpeed">初始化的速度</param>
        /// <param name="_moveTotalTime">过渡总时间</param>
        /// <param name="_firstAccSpeedTime">过渡过程的加速时间</param>
        /// <param name="_stayTime">第一次移动到目标点后停留的时间</param>
        /// <param name="_resetOrthographicSize">重置回位时的摄像头视角宽度</param>
        /// <param name="_fallbackTotalTime">重置回位的总时间</param>
        /// <param name="_fallbackAccSpeedTime">重置回位过程的加速时间</param>
        public ALCameraOrthographicSizeControllerTimingTransMove(int _priority, float _startOrthographicSize, float _stayOrthographicSize, float _srcChgSpeed, float _moveTotalTime
                    , float _firstAccSpeedTime, float _stayTime, float _resetOrthographicSize, float _fallbackTotalTime, float _fallbackAccSpeedTime)
        {
            _m_iMovePriority = _priority;

            //设置状态
            _m_eTransMoveState = EALCameraTimingTransMoveState.FIRST_MOVE;
            //创建第一次的控制数组
            _m_fcPosTransController = new ALRealTimeFloatFadeController(_startOrthographicSize, _stayOrthographicSize, _srcChgSpeed, _moveTotalTime, _firstAccSpeedTime);

            //设置其他相关参数
            _m_fResetOrthographicSize = _resetOrthographicSize;
            _m_fFallbackTotalMoveTime = _fallbackTotalTime;
            _m_fFallbackAccSpeedTime = _fallbackAccSpeedTime;
            _m_fFirstMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime;
            _m_fStayFallbackTime = Time.realtimeSinceStartup + _moveTotalTime + _stayTime;
            _m_fTotalMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime + _stayTime + _fallbackTotalTime;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public override _AALCameraOrthographicSizeController checkUpdate()
        {
            //根据不同阶段进行判断
            if(EALCameraTimingTransMoveState.FIRST_MOVE == _m_eTransMoveState)
            {
                if(Time.realtimeSinceStartup >= _m_fFirstMoveDoneTime)
                {
                    _m_eTransMoveState = EALCameraTimingTransMoveState.STAY;
                }
            }

            //判断是否在停留阶段
            if(EALCameraTimingTransMoveState.STAY == _m_eTransMoveState)
            {
                //此时判断时间是否到了需要重置的阶段
                if(Time.realtimeSinceStartup >= _m_fStayFallbackTime)
                {
                    float curFieldOfView = orthographicSize;
                    //进入重置阶段的运动
                    _m_fcPosTransController = new ALRealTimeFloatFadeController(curFieldOfView, _m_fResetOrthographicSize, 0, _m_fFallbackTotalMoveTime, _m_fFallbackAccSpeedTime);

                    _m_eTransMoveState = EALCameraTimingTransMoveState.RESET;
                }
            }

            if(EALCameraTimingTransMoveState.RESET == _m_eTransMoveState)
            {
                if(Time.realtimeSinceStartup >= _m_fTotalMoveDoneTime)
                {
                    _m_eTransMoveState = EALCameraTimingTransMoveState.DONE;

                    //移动完成后, 返回静态状态
                    return new ALCameraOrthographicSizeControllerStaticValue(_m_fResetOrthographicSize);
                }
            }

            return null;
        }

        /** 获取当前摄像头视角 */
        public override float orthographicSize { get { return _m_fcPosTransController.curValue; } }
        /** 获取当前摄像头视角变换的速度 */
        public override float orthographicSizeChgSpeed { get { return _m_fcPosTransController.curFadeSpeed; } }
        /** 获取摄像头的目标视角值 */
        public override float orthographicSizeTargetValue { get { return _m_fResetOrthographicSize; } }
        /** 获取当前摄像头视角是否在变化 */
        public override bool isMoving { get { if(EALCameraTimingTransMoveState.DONE == _m_eTransMoveState) return false; else return true; } }
        /** 当前摄像头位置操作的优先级 */
        public override int priority { get { return _m_iMovePriority; } }
    }
}
