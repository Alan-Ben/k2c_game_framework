using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /************************
     * 输入操作的基本监听操作类，在每个Update操作中进行监听以及处理
     **/
    public class ALPCInputListener : _AALInputListener
    {
        private int _m_iOpSerializeCounter = 1;

        /** 按键操作序列号 */
        private int _m_iOpSerialize = 0;
        private int _m_iAssistOpSerialize = 0;
        /** 按键是否按下 */
        private bool _m_bIsOpBtnPress = false;
        private bool _m_bIsAssistOpBtnPress = false;
        /** 按键是否在UI按下 */
        private bool _m_bIsOpBtnPressUI = false;
        private bool _m_bIsAssistOpBtnPressUI = false;
        /** 当前帧操作标记的位置 */
        private Vector2 _m_vTagScreenPos;
        /** unity中的位置标记 */
        private Vector2 _m_vUnityTagPos;
        /** 上次操作标记的位置 */
        private Vector2 _m_vTagPreScreenPos;
        private Vector2 _m_vTagPreUnityPos;
        /** 当前帧操作标记移动的距离 */
        private Vector2 _m_vTagMoveVector;
        private Vector2 _m_vTagUnityMoveVector;

        /// <summary>
        /// 重置本输入对象的状态
        /// </summary>
        public override void reset()
        {
            //重置第一触摸点
            _m_iOpSerialize = _m_iOpSerializeCounter++;
            _m_bIsOpBtnPress = false;
            _m_bIsOpBtnPressUI = false;

            //重置第二触摸点
            _m_iAssistOpSerialize = _m_iOpSerializeCounter++;
            _m_bIsAssistOpBtnPress = false;
            _m_bIsAssistOpBtnPressUI = false;

            _m_vTagMoveVector = Vector2.zero;
            _m_vTagUnityMoveVector = Vector2.zero;
        }

        /*******************
        * 每帧处理的函数
        **/
        public override void frameCheck()
        {
            //设置鼠标之前坐标
            _m_vTagPreScreenPos = _m_vTagScreenPos;
            _m_vTagPreUnityPos = _m_vUnityTagPos;

            //获取当前标记位置
            _m_vTagScreenPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            _m_vUnityTagPos = Input.mousePosition;

            //计算移动距离
            _m_vTagMoveVector = new Vector2(_m_vTagScreenPos.x - _m_vTagPreScreenPos.x, _m_vTagScreenPos.y - _m_vTagPreScreenPos.y);
            _m_vTagUnityMoveVector = _m_vUnityTagPos - _m_vTagPreUnityPos;

            bool curOpBtnEnable = Input.GetMouseButton(0);
            if (curOpBtnEnable != _m_bIsOpBtnPress)
            {
                _m_iOpSerialize = _m_iOpSerializeCounter++;
                _m_bIsOpBtnPress = curOpBtnEnable;

                //当按下的时候获取是否在UI按下
                if (_m_bIsOpBtnPress)
                {
                    _m_bIsOpBtnPressUI = _AALMonoMain.instance.isPointInUgui(_m_vUnityTagPos);
                }
                else
                {
                    _m_bIsOpBtnPressUI = false;
                }
            }

            bool curAssistOpBtnEnable = Input.GetMouseButton(1);
            if (curAssistOpBtnEnable != _m_bIsAssistOpBtnPress)
            {
                _m_iAssistOpSerialize = _m_iOpSerializeCounter++;
                _m_bIsAssistOpBtnPress = curAssistOpBtnEnable;

                //当按下的时候获取是否在UI按下
                if (_m_bIsAssistOpBtnPress)
                {
                    _m_bIsAssistOpBtnPressUI = _AALMonoMain.instance.isPointInUgui(_m_vUnityTagPos);
                }
                else
                {
                    _m_bIsAssistOpBtnPressUI = false;
                }
            }
        }

        /**********************
         * 获取对应按键的操作序列号
         **/
        public override int getOpSerialize(EALGUIOpButtonType _btn)
        {
            //左键
            if (EALGUIOpButtonType.OP_BTN == _btn)
                return _m_iOpSerialize;
            //右键
            else if (EALGUIOpButtonType.ASSIST_BTN == _btn)
                return _m_iAssistOpSerialize;
            else
                return 0;
        }

        /*******************
         * 获取对应的按键是否按下
         **/
        public override bool isBtnDown(EALGUIOpButtonType _btn)
        {
            //左键
            if (EALGUIOpButtonType.OP_BTN == _btn)
                return Input.GetMouseButton(0);
            //右键
            else if (EALGUIOpButtonType.ASSIST_BTN == _btn)
                return Input.GetMouseButton(1);
            else
                return false;
        }

        /*******************
         * 判断对应的按键是否是在UI上点下的
         **/
        public override bool isBtnPressUGUI(EALGUIOpButtonType _btn)
        {
            //左键
            if (EALGUIOpButtonType.OP_BTN == _btn)
                return _m_bIsOpBtnPressUI;
            //右键
            else if (EALGUIOpButtonType.ASSIST_BTN == _btn)
                return _m_bIsAssistOpBtnPressUI;
            else
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
            return _m_vTagPreScreenPos;
        }
        public override Vector2 getBtnScreenPos(EALGUIOpButtonType _btn)
        {
            return _m_vTagScreenPos;
        }
        public override Vector2 getBtnUnityPos(EALGUIOpButtonType _btn)
        {
            return _m_vUnityTagPos;
        }
        public override Vector2 getBtnFrameMoveVector(EALGUIOpButtonType _btn)
        {
            return _m_vTagMoveVector;
        }
        public override Vector2 getBtnFrameMoveUnityVector(EALGUIOpButtonType _btn)
        {
            return _m_vTagUnityMoveVector;
        }

        /*********************
         * 获取本帧的缩放尺寸变更大小
         **/
        public override float getScaleChg()
        {
            float scaleChg = Input.GetAxis("Mouse ScrollWheel");
            // 如果此时鼠标在UGUI上则不允许缩放动作
            if (scaleChg != 0 && !_AALMonoMain.instance.isPointInUgui(_m_vUnityTagPos))
                return scaleChg;

            return 0;
        }
        /// <summary>
        /// 获取本帧的缩放中心点
        /// </summary>
        /// <returns></returns>
        public override Vector2 getScaleCenterUnityPos()
        {
            return _m_vUnityTagPos;
        }
        public override Vector2 getScaleCenterScreenPos()
        {
            return _m_vTagScreenPos;
        }
    }
}
