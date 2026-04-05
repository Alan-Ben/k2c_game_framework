using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 摄像头移动位置的控制对象
 **/
namespace ALPackage
{
    public class ALCameraFieldOfViewControllerTransMove : _IALCameraFieldOfViewController
    {
        /** 偏移过程中的移动优先级 */
        private int _m_iMovePriority;
        /** 视角控制对象 */
        private ALRealTimeFloatFadeController _m_fcPosTransController;
        /** 总的完结时间 */
        private float _m_fTotalMoveDoneTime;

        public ALCameraFieldOfViewControllerTransMove(int _priority, float _startFieldOfView, float _targetFieldOfView, float _srcChgSpeed, float _moveTotalTime, float _firstAccSpeedTime)
        {
            _m_iMovePriority = _priority;
            
            //设置状态
            //创建第一次的控制数组
            _m_fcPosTransController = new ALRealTimeFloatFadeController(_startFieldOfView, _targetFieldOfView, _srcChgSpeed, _moveTotalTime, _firstAccSpeedTime);

            //设置其他相关参数
            _m_fTotalMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public _IALCameraFieldOfViewController checkUpdate()
        {
            if (Time.realtimeSinceStartup >= _m_fTotalMoveDoneTime)
            {
                //返回静态状态
                return new ALCameraFieldOfViewControllerStaticValue(fieldOfView);
            }

            return null;
        }

        /** 获取当前摄像头位置 */
        public float fieldOfView { get { return _m_fcPosTransController.curValue; } }
        /** 获取当前摄像头视角变换的速度 */
        public float fieldOfViewChgSpeed { get { return _m_fcPosTransController.curFadeSpeed; } }
        /** 获取摄像头的目标视角值 */
        public float fieldOfViewTargetValue { get { return _m_fcPosTransController.targetValue; } }
        /** 获取当前摄像头视角是否在变化 */
        public bool isMoving { get { return true; } }
        /** 当前摄像头位置操作的优先级 */
        public int priority { get { return _m_iMovePriority; } }
    }
}
