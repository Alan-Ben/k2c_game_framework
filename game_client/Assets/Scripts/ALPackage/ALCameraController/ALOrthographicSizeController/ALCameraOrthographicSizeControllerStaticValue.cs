using System;
using System.Collections.Generic;

using UnityEngine;

/********************************
 * 固定的控制正交视野的控制对象
 **/
namespace ALPackage
{
    public class ALCameraOrthographicSizeControllerStaticValue : _AALCameraOrthographicSizeController
    {
        /** 固定视角宽度的信息 */
        private float _m_fOrthographicSize;

        public ALCameraOrthographicSizeControllerStaticValue(float _orthographicSize)
        {
            _m_fOrthographicSize = _orthographicSize;
        }

        /***********
         * 每帧调用的处理函数，如返回新的状态对象则表明需要切换到新状态
         **/
        public override _AALCameraOrthographicSizeController checkUpdate()
        {
            return null;
        }

        /**************
         * 设置对应的视野尺寸
         **/
        public void setSize(float _orthographicSize)
        {
            _m_fOrthographicSize = _orthographicSize;

            _m_bIsInit = false;
        }

        /** 获取当前摄像头视角 */
        public override float orthographicSize { get { return _m_fOrthographicSize; } }
        /** 获取当前摄像头视角变换的速度 */
        public override float orthographicSizeChgSpeed { get { return 0; } }
        /** 获取摄像头的目标视角值 */
        public override float orthographicSizeTargetValue { get { return _m_fOrthographicSize; } }
        /** 获取当前摄像头视角是否在变化 */
        public override bool isMoving { get { return false; } }
        /** 当前摄像头位置操作的优先级 */
        public override int priority { get { return 0; } }
    }
}
