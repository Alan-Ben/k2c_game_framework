using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/********************************
 * 控制正交视野变动的控制对象
 **/
namespace ALPackage
{
    public class ALCameraOrthographicSizeControllerTransMove : _AALCameraOrthographicSizeController
    {
        /** 偏移过程中的移动优先级 */
        private int _m_iMovePriority;
        /** 视角控制对象 */
        private ALRealTimeFloatFadeController _m_fcPosTransController;
        /** 总的完结时间 */
        private float _m_fTotalMoveDoneTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_priority">偏移过程中的移动优先级</param>
        /// <param name="_startOrthographicSize">开始数值</param>
        /// <param name="_targetOrthographicSize">目标数值</param>
        /// <param name="_srcChgSpeed">初始化的速度</param>
        /// <param name="_moveTotalTime">过渡总时间</param>
        /// <param name="_firstAccSpeedTime">过渡过程的加速时间</param>
        public ALCameraOrthographicSizeControllerTransMove(int _priority, float _startOrthographicSize, float _targetOrthographicSize, float _srcChgSpeed, float _moveTotalTime, float _firstAccSpeedTime)
        {
            _m_iMovePriority = _priority;

            //设置状态
            //创建第一次的控制数组
            _m_fcPosTransController = new ALRealTimeFloatFadeController(_startOrthographicSize, _targetOrthographicSize, _srcChgSpeed, _moveTotalTime, _firstAccSpeedTime);

            //设置其他相关参数
            _m_fTotalMoveDoneTime = Time.realtimeSinceStartup + _moveTotalTime;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public override _AALCameraOrthographicSizeController checkUpdate()
        {
            if(Time.realtimeSinceStartup >= _m_fTotalMoveDoneTime)
            {
                //返回静态状态
                return new ALCameraOrthographicSizeControllerStaticValue(orthographicSize);
            }

            return null;
        }

        /** 获取当前摄像头位置 */
        public override float orthographicSize { get { return _m_fcPosTransController.curValue; } }
        /** 获取当前摄像头视角变换的速度 */
        public override float orthographicSizeChgSpeed { get { return _m_fcPosTransController.curFadeSpeed; } }
        /** 获取摄像头的目标视角值 */
        public override float orthographicSizeTargetValue { get { return _m_fcPosTransController.targetValue; } }
        /** 获取当前摄像头视角是否在变化 */
        public override bool isMoving { get { return true; } }
        /** 当前摄像头位置操作的优先级 */
        public override int priority { get { return _m_iMovePriority; } }
    }
}
