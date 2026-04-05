using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /************************
     * 输入操作的基本监听操作类，在每个Update操作中进行监听以及处理
     **/
    public class ALEmptyInputListener : _AALInputListener
    {
        private int _m_iOpSerializeCounter = 1;

        /// <summary>
        /// 重置本输入对象的状态
        /// </summary>
        public override void reset()
        {

        }

        /*******************
        * 每帧处理的函数
        **/
        public override void frameCheck()
        {
        }

        /**********************
         * 获取对应按键的操作序列号
         **/
        public override int getOpSerialize(EALGUIOpButtonType _btn)
        {
            return _m_iOpSerializeCounter++;
        }

        /*******************
         * 获取对应的按键是否按下
         **/
        public override bool isBtnDown(EALGUIOpButtonType _btn)
        {
            return false;
        }

        /*******************
         * 判断对应的按键是否是在UI上点下的
         **/
        public override bool isBtnPressUGUI(EALGUIOpButtonType _btn)
        {
            return false;
        }

        /*******************
         * 获取对应的案件的fingerId
         **/
        public override int getBtnFingerId(EALGUIOpButtonType _btn)
        {
            return -1;
        }

        /********************
         * 获取相关按键的状态
         **/
        public override Vector2 getBtnPreScreenPos(EALGUIOpButtonType _btn)
        {
            return Vector2.zero;
        }
        public override Vector2 getBtnScreenPos(EALGUIOpButtonType _btn)
        {
            return Vector2.zero;
        }
        public override Vector2 getBtnUnityPos(EALGUIOpButtonType _btn)
        {
            return Vector2.zero;
        }
        public override Vector2 getBtnFrameMoveVector(EALGUIOpButtonType _btn)
        {
            return Vector2.zero;
        }
        public override Vector2 getBtnFrameMoveUnityVector(EALGUIOpButtonType _btn)
        {
            return Vector2.zero;
        }

        /*********************
         * 获取本帧的缩放尺寸变更大小
         **/
        public override float getScaleChg()
        {
            return 0;
        }

        /// <summary>
        /// 获取本帧的缩放中心点
        /// </summary>
        /// <returns></returns>
        public override Vector2 getScaleCenterUnityPos()
        {
            return new Vector2(Screen.width / 2, Screen.height / 2);
        }
        public override Vector2 getScaleCenterScreenPos()
        {
            return new Vector2(Screen.width / 2, Screen.height / 2);
        }
    }
}
