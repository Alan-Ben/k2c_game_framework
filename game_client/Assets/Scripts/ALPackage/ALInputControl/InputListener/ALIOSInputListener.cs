using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /************************
     * 输入操作的基本监听操作类，在每个Update操作中进行监听以及处理
     **/
    public class ALIOSInputListener : _AALInputListener
    {
        private int _m_iOpSerializeCounter = 1;

        private Vector2 _m_vPreTagScreenPos;
        private Vector2 _m_vPreTagUnityPos;
        private Vector2 _m_vTagScreenPos;
        private Vector2 _m_vTagUnityPos;
        private Vector2 _m_vTagMoveVector;
        private Vector2 _m_vTagMoveUnityVector;

        /** 有效的触摸焦点 */
        private int _m_iFocusOpSerialize = 0;
        private bool _m_bFocusTouchEnable = false;
        private bool _m_bFocusTouchPressUI = false;
        private Touch _m_tFocusTouch;
        private Touch _m_tTmpFocusTouch;

        /** 第2触点相关信息 */
        private int _m_iSecTouchOpSerialize = 0;
        private bool _m_bSecTouchEnable = false;
        private bool _m_bSecTouchPressUI = false;
        private Touch _m_tSecTouchInfo;
        private Touch _m_tTmpSecTouchInfo;
        /** 第2触点原先的坐标位置信息 */
        private Vector2 _m_vSecPreTagScreenPos;
        private Vector2 _m_vSecPreTagUnityPos;
        private Vector2 _m_vSecTagScreenPos;
        private Vector2 _m_vSecTagUnityPos;
        private Vector2 _m_vSecTagMoveVector;
        private Vector2 _m_vSecTagMoveUnityVector;
        /** 两个触点变动后的缩放倍率存储变量 */
        private float _m_fSqrSrcMultiTouchDis = 0;
        private float _m_fScaleSize;
        private float _m_fScaleSizeChgValue;
        private Vector2 _m_vScaleCenterUnityPos;
        private Vector2 _m_vScaleCenterScreenPos;

        /// <summary>
        /// 重置本输入对象的状态
        /// </summary>
        public override void reset()
        {
            //重置第一触摸点
            //设置当前触摸点无效
            _m_bFocusTouchEnable = false;
            _m_bFocusTouchPressUI = false;
            _m_iFocusOpSerialize = _m_iOpSerializeCounter++;
            //此时设置坐标无效
            _m_vPreTagScreenPos = Vector2.zero;
            _m_vPreTagUnityPos = Vector2.zero;
            _m_vTagScreenPos = new Vector2(float.MinValue, float.MinValue);
            _m_vTagUnityPos = new Vector2(float.MinValue, float.MinValue);
            _m_vTagMoveVector = Vector2.zero;
            _m_vTagMoveUnityVector = Vector2.zero;

            //重置第二触摸点
            //设置当前按键无效
            _m_bSecTouchEnable = false;
            _m_bSecTouchPressUI = false;
            _m_iSecTouchOpSerialize = _m_iOpSerializeCounter++;
            //此时设置坐标无效
            _m_vSecTagUnityPos = new Vector2(float.MinValue, float.MinValue);
            _m_vSecTagScreenPos = new Vector2(float.MinValue, float.MinValue);
            _m_vSecPreTagScreenPos = Vector2.zero;
            _m_vSecPreTagUnityPos = Vector2.zero;
            _m_vSecTagMoveVector = Vector2.zero;
            _m_vSecTagMoveUnityVector = Vector2.zero;

            _m_vScaleCenterUnityPos = Vector2.zero;
            _m_vScaleCenterScreenPos = Vector2.zero;

            _m_fSqrSrcMultiTouchDis = 0;
        }

        /*******************
        * 每帧处理的函数
        **/
        public override void frameCheck()
        {
            //重置比率缩放
            _m_fScaleSizeChgValue = 0;

            //判断按下的按键是否有效，有效则判断是否已释放
            if (_m_bFocusTouchEnable)
            {
                //新的触摸点是否有效
                bool isEnable = false;

                //获取当前的最新按键对象
                for (int i = 0; i < Input.touchCount; i++)
                {
                    _m_tTmpFocusTouch = Input.GetTouch(i);
                    if (_m_tTmpFocusTouch.fingerId == _m_tFocusTouch.fingerId && _m_tTmpFocusTouch.phase != TouchPhase.Began)
                    {
                        //判断是否当前在监控的触摸焦点
                        isEnable = true;
                        break;
                    }
                }

                //判断该触摸点是否还在屏幕上
                if (isEnable)
                {
                    //设置按键的新位置
                    _m_tFocusTouch = _m_tTmpFocusTouch;

                    _m_vTagUnityPos = _m_tFocusTouch.position;
                    _m_vTagScreenPos = new Vector2(_m_vTagUnityPos.x, Screen.height - _m_vTagUnityPos.y);
                    //计算移动距离
                    _m_vTagMoveVector = _m_vTagScreenPos - _m_vPreTagScreenPos;
                    _m_vTagMoveUnityVector = _m_vTagUnityPos - _m_vPreTagUnityPos;

                    //设置前置位置
                    _m_vPreTagScreenPos = _m_vTagScreenPos;
                    _m_vPreTagUnityPos = _m_vTagUnityPos;
                }
                else
                {
                    //设置当前触摸点无效
                    _m_bFocusTouchEnable = false;
                    _m_bFocusTouchPressUI = false;
                    _m_iFocusOpSerialize = _m_iOpSerializeCounter++;
                    //此时设置坐标无效
                    _m_vPreTagScreenPos = Vector2.zero;
                    _m_vPreTagUnityPos = Vector2.zero;
                    _m_vTagScreenPos = new Vector2(float.MinValue, float.MinValue);
                    _m_vTagUnityPos = new Vector2(float.MinValue, float.MinValue);
                    _m_vTagMoveVector = Vector2.zero;
                    _m_vTagMoveUnityVector = Vector2.zero;
                }
            }
            else
            {
                //如原先无按键按下，则取第一个按下的对象作为按键信息
                for (int i = 0; i < Input.touchCount; i++)
                {
                    _m_tTmpFocusTouch = Input.GetTouch(i);

                    //判断是否刚按下的按键
                    if (TouchPhase.Began == _m_tTmpFocusTouch.phase)
                    {
                        //是则设置按键对象
                        _m_tFocusTouch = _m_tTmpFocusTouch;
                        _m_bFocusTouchEnable = true;
                        _m_bFocusTouchPressUI = _AALMonoMain.instance.isPointInUgui(_m_tFocusTouch.position);
                        _m_iFocusOpSerialize = _m_iOpSerializeCounter++;

                        //初始化所有操作信息
                        _m_vTagUnityPos = _m_tFocusTouch.position;
                        _m_vTagScreenPos = new Vector2(_m_vTagUnityPos.x, Screen.height - _m_vTagUnityPos.y);
                        _m_vPreTagScreenPos = _m_vTagScreenPos;
                        _m_vPreTagUnityPos = _m_vTagUnityPos;
                        _m_vTagMoveVector = Vector2.zero;
                        _m_vTagMoveUnityVector = Vector2.zero;

                        break;
                    }
                }
            }

            /******************
             * 进行第二触点判断
             **/
            if (_m_bSecTouchEnable)
            {
                bool isEnable = false;

                //第2触点有效则先判断第2触点是否失效
                for (int i = 0; i < Input.touchCount; i++)
                {
                    _m_tTmpSecTouchInfo = Input.GetTouch(i);
                    if (_m_tTmpSecTouchInfo.fingerId == _m_tSecTouchInfo.fingerId && _m_tTmpFocusTouch.phase != TouchPhase.Began)
                    {
                        isEnable = true;
                        break;
                    }
                }

                //判断该触摸点是否还在屏幕
                if (isEnable)
                {
                    //设置第2触点信息
                    _m_tSecTouchInfo = _m_tTmpSecTouchInfo;

                    _m_vSecTagUnityPos = _m_tSecTouchInfo.position;
                    _m_vSecTagScreenPos = new Vector2(_m_vSecTagUnityPos.x, Screen.height - _m_vSecTagUnityPos.y);
                    //计算移动位置
                    _m_vSecTagMoveVector = _m_vSecTagScreenPos - _m_vSecPreTagScreenPos;
                    _m_vSecTagMoveUnityVector = _m_vSecTagUnityPos - _m_vSecPreTagUnityPos;

                    _m_vSecPreTagUnityPos = _m_vSecTagUnityPos;
                    _m_vSecPreTagScreenPos = _m_vSecTagScreenPos;
                }
                else
                {
                    //设置当前按键无效
                    _m_bSecTouchEnable = false;
                    _m_bSecTouchPressUI = false;
                    _m_iSecTouchOpSerialize = _m_iOpSerializeCounter++;
                    //此时设置坐标无效
                    _m_vSecTagUnityPos = new Vector2(float.MinValue, float.MinValue);
                    _m_vSecTagScreenPos = new Vector2(float.MinValue, float.MinValue);
                    _m_vSecPreTagScreenPos = Vector2.zero;
                    _m_vSecPreTagUnityPos = Vector2.zero;
                    _m_vSecTagMoveVector = Vector2.zero;
                    _m_vSecTagMoveUnityVector = Vector2.zero;
                }
            }
            else
            {
                //如原先无按键按下，则取第一个按下的对象作为按键信息
                for (int i = 0; i < Input.touchCount; i++)
                {
                    _m_tTmpSecTouchInfo = Input.GetTouch(i);
                    //判断是否刚按下的按键
                    if (_m_tTmpSecTouchInfo.fingerId != _m_tFocusTouch.fingerId
                        && TouchPhase.Began == _m_tTmpSecTouchInfo.phase)
                    {
                        //非第1触点对象则为第2触点
                        _m_tSecTouchInfo = _m_tTmpSecTouchInfo;
                        _m_bSecTouchEnable = true;
                        _m_bSecTouchPressUI = _AALMonoMain.instance.isPointInUgui(_m_tSecTouchInfo.position);
                        _m_iSecTouchOpSerialize = _m_iOpSerializeCounter++;

                        //记录第2触点的位置信息
                        _m_vSecTagUnityPos = _m_tSecTouchInfo.position;
                        _m_vSecTagScreenPos = new Vector2(_m_vSecTagUnityPos.x, Screen.height - _m_vSecTagUnityPos.y);
                        _m_vSecPreTagUnityPos = _m_vSecTagUnityPos;
                        _m_vSecPreTagScreenPos = _m_vSecTagScreenPos;
                        _m_vSecTagMoveVector = Vector2.zero;
                        _m_vSecTagMoveUnityVector = Vector2.zero;

                        break;
                    }
                }
            }

            //当第一第二触点都有效时，并且没有点击到UGUI时，计算间距
            if (!_m_bFocusTouchEnable || !_m_bSecTouchEnable ||
                _m_bFocusTouchPressUI || _m_bSecTouchPressUI)
            {
                _m_fSqrSrcMultiTouchDis = 0;
            }
            else
            {
                //判断之前是否存在间距，存在则计算缩放
                if (_m_fSqrSrcMultiTouchDis > 1)
                {
                    //计算当前的位移便宜距离后计算缩放大小
                    float sqrMultiTouchDis = (_m_vTagUnityPos - _m_vSecTagUnityPos).sqrMagnitude;

                    //计算距离改变的倍率，作为缩放倍率进行使用
                    float newScaleSize = (float)(Math.Sqrt(sqrMultiTouchDis / _m_fSqrSrcMultiTouchDis));
                    _m_fScaleSizeChgValue = newScaleSize - 1f;
                    //计算缩放两指的中心位置
                    _m_vScaleCenterUnityPos = (_m_vTagUnityPos + _m_vSecTagUnityPos)/2;
                    _m_vScaleCenterScreenPos = new Vector2(_m_vScaleCenterUnityPos.x, Screen.height - _m_vScaleCenterUnityPos.y);


                    //设置新间距
                    _m_fSqrSrcMultiTouchDis = sqrMultiTouchDis;
                }
                else
                {
                    _m_fSqrSrcMultiTouchDis = (_m_vTagUnityPos - _m_vSecTagUnityPos).sqrMagnitude;
                }
            }
        }

        /**********************
         * 获取对应按键的操作序列号
         **/
        public override int getOpSerialize(EALGUIOpButtonType _opBtnType)
        {
            //左键
            if (EALGUIOpButtonType.OP_BTN == _opBtnType)
                return _m_iFocusOpSerialize;
            //右键
            else if (EALGUIOpButtonType.ASSIST_BTN == _opBtnType)
                return _m_iSecTouchOpSerialize;
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
                return _m_bFocusTouchEnable;
            //右键
            else if (EALGUIOpButtonType.ASSIST_BTN == _btn)
                return _m_bSecTouchEnable;
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
                return _m_bFocusTouchPressUI;
            //右键
            else if (EALGUIOpButtonType.ASSIST_BTN == _btn)
                return _m_bSecTouchPressUI;
            else
                return false;
        }

        /*******************
         * 获取对应的案件的fingerId
         **/
        public override int getBtnFingerId(EALGUIOpButtonType _btn)
        {
            //左键
            if (EALGUIOpButtonType.OP_BTN == _btn)
                return _m_tFocusTouch.fingerId;
            //右键
            else if (EALGUIOpButtonType.ASSIST_BTN == _btn)
                return _m_tSecTouchInfo.fingerId;
            else
                return -1;
        }

        /********************
         * 获取相关按键的状态
         **/
        public override Vector2 getBtnPreScreenPos(EALGUIOpButtonType _btn)
        {
            if (_btn == EALGUIOpButtonType.OP_BTN)
                return _m_vPreTagScreenPos;
            else if (_btn == EALGUIOpButtonType.ASSIST_BTN)
                return _m_vSecPreTagScreenPos;
            else
                return Vector2.zero;
        }
        public override Vector2 getBtnScreenPos(EALGUIOpButtonType _btn)
        {
            if (_btn == EALGUIOpButtonType.OP_BTN)
                return _m_vTagScreenPos;
            else if (_btn == EALGUIOpButtonType.ASSIST_BTN)
                return _m_vSecTagScreenPos;
            else
                return Vector2.zero;
        }
        public override Vector2 getBtnUnityPos(EALGUIOpButtonType _btn)
        {
            if (_btn == EALGUIOpButtonType.OP_BTN)
                return _m_vTagUnityPos;
            else if (_btn == EALGUIOpButtonType.ASSIST_BTN)
                return _m_vSecTagUnityPos;
            else
                return Vector2.zero;
        }
        public override Vector2 getBtnFrameMoveVector(EALGUIOpButtonType _btn)
        {
            if (_btn == EALGUIOpButtonType.OP_BTN)
                return _m_vTagMoveVector;
            else if (_btn == EALGUIOpButtonType.ASSIST_BTN)
                return _m_vSecTagMoveVector;
            else
                return Vector2.zero;
        }
        public override Vector2 getBtnFrameMoveUnityVector(EALGUIOpButtonType _btn)
        {
            if (_btn == EALGUIOpButtonType.OP_BTN)
                return _m_vTagMoveUnityVector;
            else if (_btn == EALGUIOpButtonType.ASSIST_BTN)
                return _m_vSecTagMoveUnityVector;
            else
                return Vector2.zero;
        }

        /*********************
         * 获取本帧的缩放尺寸变更大小
         **/
        public override float getScaleChg()
        {
            return _m_fScaleSizeChgValue;
        }
        /// <summary>
        /// 获取本帧的缩放中心点
        /// </summary>
        /// <returns></returns>
        public override Vector2 getScaleCenterUnityPos()
        {
            return _m_vScaleCenterUnityPos;
        }
        public override Vector2 getScaleCenterScreenPos()
        {
            return _m_vScaleCenterScreenPos;
        }
    }
}
