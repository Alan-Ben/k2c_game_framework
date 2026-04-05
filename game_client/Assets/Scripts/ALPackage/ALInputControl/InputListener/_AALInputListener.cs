using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /************************
     * 输入操作的基本监听操作类，在每个Update操作中进行监听以及处理
     **/
    public abstract class _AALInputListener
    {
        /// <summary>
        /// 重置本输入对象的状态
        /// </summary>
        public abstract void reset();

        /**********************
         * 每帧的处理函数
         **/
        public abstract void frameCheck();

        /**********************
         * 获取对应按键的操作序列号
         **/
        public abstract int getOpSerialize(EALGUIOpButtonType _btn);

        /*******************
         * 获取对应的按键是否按下
         **/
        public abstract bool isBtnDown(EALGUIOpButtonType _btn);

        /*******************
         * 判断对应的按键是否是在UI上点下的
         **/
        public abstract bool isBtnPressUGUI(EALGUIOpButtonType _btn);

        /*******************
         * 获取对应的案件的fingerId
         **/
        public abstract int getBtnFingerId(EALGUIOpButtonType _btn);

        /********************
         * 获取相关按键的状态
         **/
        public abstract Vector2 getBtnPreScreenPos(EALGUIOpButtonType _btn);
        public abstract Vector2 getBtnScreenPos(EALGUIOpButtonType _btn);
        public abstract Vector2 getBtnUnityPos(EALGUIOpButtonType _btn);
        public abstract Vector2 getBtnFrameMoveVector(EALGUIOpButtonType _btn);
        public abstract Vector2 getBtnFrameMoveUnityVector(EALGUIOpButtonType _btn);

        /// <summary>
        /// 获取本帧的缩放尺寸变更大小
        /// </summary>
        /// <returns></returns>
        public abstract float getScaleChg();
        /// <summary>
        /// 获取本帧缩放的缩放中心位置坐标
        /// </summary>
        /// <returns></returns>
        public abstract Vector2 getScaleCenterUnityPos();
        public abstract Vector2 getScaleCenterScreenPos();
    }
}
