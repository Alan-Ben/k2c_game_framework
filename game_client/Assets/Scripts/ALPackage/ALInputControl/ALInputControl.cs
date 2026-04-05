using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /**************
     * 按键类型
     * PC时：
     * 左键 - OP_BTN
     * 右键 - ASSIST_BTN
     * 
     * 手机/平板时：
     * 单指 - OP_BTN
     * 双指 - ASSIST_BTN
     **/
    public enum EALGUIOpButtonType
    {
        NONE = 0,
        OP_BTN = 1,
        ASSIST_BTN = 2,
    }

    /************************
     * 输入操作的管理控制类，可对应不同平台注册不同的处理对象
     **/
    public class ALInputControl
    {
        /** 键盘按键事件，返回处理结果 */
        public delegate bool KeyDownEvent(KeyCode _key);

        private static ALInputControl _g_instance = new ALInputControl();
        public static ALInputControl instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALInputControl();

                return _g_instance;
            }
        }

        //使用二进制对是否屏蔽输入进行判断
        private long _m_lIngnoreInput = 0;

        /** 输入的监听对象 */
        private _AALInputListener _m_ilInputListener;

        /** 当前操作按钮的状态存储，如果是 OP + ASSIST 则为 OP | ASSIST */
        private int _m_iOpButtonState;
        /** 存储本帧处理的按下的操作按钮 */
        private EALGUIOpButtonType _m_eFrameDownEventButton;
        /** 存储本帧处理的弹起的操作按钮 */
        private EALGUIOpButtonType _m_eFrameUpEventButton;

        /** 键盘事件响应函数 */
        private List<KeyDownEvent> _m_lKeyDownDelegate = new List<KeyDownEvent>(1);

        protected ALInputControl()
        {
            _m_ilInputListener = new ALEmptyInputListener();

            _m_iOpButtonState = 0;
            _m_eFrameDownEventButton = EALGUIOpButtonType.NONE;
            _m_eFrameUpEventButton = EALGUIOpButtonType.NONE;
        }

        public EALGUIOpButtonType btnDownEvent { get { return _m_eFrameDownEventButton; } }
        public EALGUIOpButtonType btnUpEvent { get { return _m_eFrameUpEventButton; } }

        /*******************
         * 获取对应按键的操作序号
         **/
        public int getOpSerialize(EALGUIOpButtonType _btn)
        {
            if (null == _m_ilInputListener)
                return 0;

            return _m_ilInputListener.getOpSerialize(_btn);
        }

        /// <summary>
        /// 判断是否可以esc回退
        /// </summary>
        /// <returns></returns>
        public bool ignoreInput()
        {
            return _m_lIngnoreInput != 0;
        }
        /// <summary>
        /// 开启回退功能
        /// </summary>
        /// <param name="_sysIndex_64">带入系统索引，最高64</param>
        public void OpenInput()
        {
            OpenInput(0);
        }
        public void OpenInput(int _sysIndex_64)
        {
            if (_sysIndex_64 > 64)
            {
                ALLog.Error("Open Input SysIndex is Bigger than 64");
                _sysIndex_64 = 64;
            }
            else if (_sysIndex_64 < 0)
            {
                ALLog.Error("Open Input SysIndex is Smaller than 0");
                _sysIndex_64 = 0;
            }

            _m_lIngnoreInput = _m_lIngnoreInput & ~(1L << _sysIndex_64);
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Qeueu] √ OpenInput({_sysIndex_64}): ignoreInput:{ignoreInput()}");
            }
        }
        /// <summary>
        /// 关闭回退功能
        /// </summary>
        /// <param name="_sysIndex_64">带入系统索引，最高64</param>
        public void CloseInput()
        {
            CloseInput(0);
        }
        public void CloseInput(int _sysIndex_64)
        {
            if (_sysIndex_64 > 64)
            {
                ALLog.Error("Close Input SysIndex is Bigger than 64");
                _sysIndex_64 = 64;
            }
            else if (_sysIndex_64 < 0)
            {
                ALLog.Error("Close Input SysIndex is Smaller than 0");
                _sysIndex_64 = 0;
            }

            _m_lIngnoreInput = _m_lIngnoreInput | (1L << _sysIndex_64);
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Qeueu] × CloseInput({_sysIndex_64}): ignoreInput:{ignoreInput()}");
            }
        }

        /********************
         * 获取相关按键的状态
         **/
        public int getBtnFingerId(EALGUIOpButtonType _btn)
        {
            if (null == _m_ilInputListener)
                return -1;

            return _m_ilInputListener.getBtnFingerId(_btn);
        }
        public Vector2 getBtnPreScreenPos(EALGUIOpButtonType _btn)
        {
            if (null == _m_ilInputListener)
                return Vector2.zero;

            return _m_ilInputListener.getBtnPreScreenPos(_btn);
        }
        public Vector2 getBtnScreenPos(EALGUIOpButtonType _btn)
        {
            if (null == _m_ilInputListener)
                return Vector2.zero;

            return _m_ilInputListener.getBtnScreenPos(_btn);
        }
        public Vector2 getBtnUnityPos(EALGUIOpButtonType _btn)
        {
            if (null == _m_ilInputListener)
                return Vector2.zero;

            return _m_ilInputListener.getBtnUnityPos(_btn);
        }
        public Vector2 getBtnFrameMoveVector(EALGUIOpButtonType _btn)
        {
            if (null == _m_ilInputListener)
                return Vector2.zero;

            return _m_ilInputListener.getBtnFrameMoveVector(_btn);
        }
        public Vector2 getBtnFrameMoveUnityVector(EALGUIOpButtonType _btn)
        {
            if (null == _m_ilInputListener)
                return Vector2.zero;

            return _m_ilInputListener.getBtnFrameMoveUnityVector(_btn);
        }

        /*******************
         * 设置使用的输入监听对象
         **/
        public void regInputListener(_AALInputListener _listener)
        {
            _m_ilInputListener = _listener;
        }

        /*****************
         * 从本控制对象中判断某一操作按钮是否按下
         **/
        public bool isBtnPress(EALGUIOpButtonType _btnType)
        {
            if ((_m_iOpButtonState & (int)_btnType) != 0)
            {
                return true;
            }

            return false;
        }

        /*****************
         * 从本控制对象中判断某一操作按钮是否在UGUI按下
         **/
        public bool isBtnPressUGUI(EALGUIOpButtonType _btnType)
        {
            return _m_ilInputListener.isBtnPressUGUI(_btnType);
        }

        /********************
         * 更新本控制对象的相关信息
         **/
        public void frameCheck()
        {
            //如果屏蔽则默认所有输入放开
            if (ignoreInput())
            {
                //重置本帧的标记操作信息
                _m_iOpButtonState = 0;
                _m_eFrameDownEventButton = EALGUIOpButtonType.NONE;
                _m_eFrameUpEventButton = EALGUIOpButtonType.NONE;

                //重置输入对象
                if (null != _m_ilInputListener)
                    _m_ilInputListener.reset();

                return;
            }

            //无输入对象则不处理
            if (null == _m_ilInputListener)
                return;

            //调用每帧检查
            _m_ilInputListener.frameCheck();

            //重置本帧的标记操作信息
            _m_eFrameDownEventButton = EALGUIOpButtonType.NONE;
            _m_eFrameUpEventButton = EALGUIOpButtonType.NONE;

            //判断并设置按下操作的按钮信息，两个操作键类型
            if (!isBtnPress(EALGUIOpButtonType.OP_BTN) && _m_ilInputListener.isBtnDown(EALGUIOpButtonType.OP_BTN))
                _m_eFrameDownEventButton = EALGUIOpButtonType.OP_BTN;
            else if (!isBtnPress(EALGUIOpButtonType.ASSIST_BTN) && _m_ilInputListener.isBtnDown(EALGUIOpButtonType.ASSIST_BTN))
                _m_eFrameDownEventButton = EALGUIOpButtonType.ASSIST_BTN;
            
            //设置按钮按下状态
            _setOpBtnPress(_m_eFrameDownEventButton);

            //分别判断鼠标状态，设置放开的操作按钮信息
            if (isBtnPress(EALGUIOpButtonType.OP_BTN) && !_m_ilInputListener.isBtnDown(EALGUIOpButtonType.OP_BTN))
                _m_eFrameUpEventButton = EALGUIOpButtonType.OP_BTN;
            else if (isBtnPress(EALGUIOpButtonType.ASSIST_BTN) && !_m_ilInputListener.isBtnDown(EALGUIOpButtonType.ASSIST_BTN))
                _m_eFrameUpEventButton = EALGUIOpButtonType.ASSIST_BTN;

            //设置按钮按下状态
            _setOpBtnFree(_m_eFrameUpEventButton);
        }

        /****************
         * 在OnGUI事件中检测键盘事件处理
         **/
        public void guiCheckKeyEvent()
        {
            //判断相关的按键事件
            if (null != Event.current
                && Event.current.type == EventType.KeyDown)
            {
                //尝试进行处理
                if (_ALGUIDealKeyDownDelegateList(_m_lKeyDownDelegate, Event.current.keyCode))
                {
                    //处理成功则重置消息
                    Event.current = null;
                }
            }
        }

        /***********************
         * 按键事件处理的相关回调函数
         **/
        public void ALGUIRegKeyDownDelegate(KeyDownEvent _delegate)
        {
            if (null != _delegate)
                _m_lKeyDownDelegate.Add(_delegate);
        }
        public void ALGUIUnregKeyDownDelegate(KeyDownEvent _delegate)
        {
            if (null != _delegate)
                _m_lKeyDownDelegate.Remove(_delegate);
        }

        /*********************
         * 获取本帧的缩放尺寸变更大小
         **/
        public float getScaleChg()
        {
            if (null != _m_ilInputListener)
                return _m_ilInputListener.getScaleChg();
            else
                return 0;
        }
        /// <summary>
        /// 获取本帧的缩放中心位置
        /// </summary>
        /// <returns></returns>
        public Vector2 getScaleCenterUnityPos()
        {
            if (null != _m_ilInputListener)
                return _m_ilInputListener.getScaleCenterUnityPos();
            else
                return new Vector2(Screen.width / 2, Screen.height / 2);
        }
        /// <summary>
        /// 获取本帧的缩放中心位置
        /// </summary>
        /// <returns></returns>
        public Vector2 getScaleCenterScreenPos()
        {
            if (null != _m_ilInputListener)
                return _m_ilInputListener.getScaleCenterScreenPos();
            else
                return new Vector2(Screen.width / 2, Screen.height / 2);
        }
        /**********************
         * 设置某操作按钮状态
         **/
        protected void _setOpBtnPress(EALGUIOpButtonType _btnType)
        {
            //设置按钮的状态值
            _m_iOpButtonState = _m_iOpButtonState | (int)_btnType;
        }

        /**********************
         * 设置某操作按钮状态
         **/
        protected void _setOpBtnFree(EALGUIOpButtonType _btnType)
        {
            //设置按钮的状态值
            _m_iOpButtonState = _m_iOpButtonState & ~(int)_btnType;
        }

        /******************
         * 处理按键的响应事件
         **/
        private bool _ALGUIDealKeyDownDelegateList(List<KeyDownEvent> _delegateList, KeyCode _key)
        {
            bool dealRes = false;

            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    KeyDownEvent actionDelegate = _delegateList[i];

                    if (null != actionDelegate && actionDelegate(_key))
                    {
                        dealRes = true;
                    }
                }
            }

            return dealRes;
        }
    }
}
