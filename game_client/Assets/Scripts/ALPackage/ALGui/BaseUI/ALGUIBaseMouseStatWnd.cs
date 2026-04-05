using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public enum ALGUIBaseMouseStat
    {
        NORMAL = 0,
        OVER = 1,
        DOWN = 2,
        FOCUS = 8,
        DRAG_OVER = 16,
    }

    public class ALGUIBaseMouseStatWnd : ALGUICaptureWnd
    {
        /** 判断拖拽信息对本对象是否有意义 */
        public delegate bool JudgeDragItemEnable(ALGUIBaseWnd _wnd, _AALGUIDragItemInfo _dragItem);

        /** 当前窗口状态 */
        private ALGUIBaseMouseStat _m_eWndStat;
        /** 判断拖拽信息是否有效的函数指针 */
        private JudgeDragItemEnable _m_dJudgeDragItemEnableDelegate;


        public ALGUIBaseMouseStatWnd(Rect _wndRect)
            : base(_wndRect)
        {
            _m_eWndStat = ALGUIBaseMouseStat.NORMAL;
            _m_dJudgeDragItemEnableDelegate = null;

            ALGUIRegMouseInDelegate(OnMouseMoveIn);
            ALGUIRegMouseOutDelegate(OnMouseMoveOut);
            ALGUIRegMouseDownDelegate(OnMouseDown);
            ALGUIRegMouseUpDelegate(OnMouseUp);

            ALGUIWndFocusActionDelegate += OnFocus;
            ALGUIDragMoveInDelegate += OnDragMouseMoveIn;
            ALGUIDragMoveOutDelegate += OnDragMouseMoveOut;
        }
        public ALGUIBaseMouseStatWnd(ALGUIWndPositionStyle _posStyle)
            : base(_posStyle)
        {
            _m_eWndStat = ALGUIBaseMouseStat.NORMAL;
            _m_dJudgeDragItemEnableDelegate = null;

            ALGUIRegMouseInDelegate(OnMouseMoveIn);
            ALGUIRegMouseOutDelegate(OnMouseMoveOut);
            ALGUIRegMouseDownDelegate(OnMouseDown);
            ALGUIRegMouseUpDelegate(OnMouseUp);

            ALGUIWndFocusActionDelegate += OnFocus;
            ALGUIDragMoveInDelegate += OnDragMouseMoveIn;
            ALGUIDragMoveOutDelegate += OnDragMouseMoveOut;
        }

        /******************
         * 设置判断拖拽信息是否有效的指针对象
         **/
        public void setJudgeDragItemEnableDelegate(JudgeDragItemEnable _delegate)
        {
            _m_dJudgeDragItemEnableDelegate = _delegate;
        }

        /*************************
         * 判断按钮是否某一状态
         *************************/
        public bool isStat(ALGUIBaseMouseStat _stat)
        {
            if ((_stat & _m_eWndStat) == _stat)
                return true;

            return false;
        }
        public bool isnotStat(ALGUIBaseMouseStat _stat)
        {
            if ((~_stat & _m_eWndStat) == 0)
                return true;

            return false;
        }

        /*******************
         * 鼠标移进窗口区域时处理
         *******************/
        public void OnMouseMoveIn(ALGUIBaseWnd _wnd)
        {
            _enterStat(ALGUIBaseMouseStat.OVER);
        }

        /*******************
         * 鼠标移出窗口区域时处理
         *******************/
        public void OnMouseMoveOut(ALGUIBaseWnd _wnd)
        {
            _quitStat(ALGUIBaseMouseStat.OVER);
        }

        /*******************
         * 拖拽信息时，鼠标移进窗口区域时处理
         *******************/
        public void OnDragMouseMoveIn(ALGUIBaseWnd _wnd, _AALGUIDragItemInfo _dragItem)
        {
            //无判断函数指针，直接不处理状态
            if (null == _m_dJudgeDragItemEnableDelegate)
                return;

            if (_m_dJudgeDragItemEnableDelegate(_wnd, _dragItem))
                _enterStat(ALGUIBaseMouseStat.DRAG_OVER);
        }

        /*******************
         * 拖拽信息时，鼠标移出窗口区域时处理
         *******************/
        public void OnDragMouseMoveOut(ALGUIBaseWnd _wnd, _AALGUIDragItemInfo _dragItem)
        {
            //无判断函数指针，直接不处理状态
            if (null == _m_dJudgeDragItemEnableDelegate)
                return;

            if (_m_dJudgeDragItemEnableDelegate(_wnd, _dragItem))
                _quitStat(ALGUIBaseMouseStat.DRAG_OVER);
        }

        /*******************
         * 窗口获取与失去时的事件函数
         *******************/
        public void OnFocus(ALGUIBaseWnd _wnd, bool _focus)
        {
            if (_focus)
                _enterStat(ALGUIBaseMouseStat.FOCUS);
            else
                _quitStat(ALGUIBaseMouseStat.FOCUS);
        }

        /*******************
         * 鼠标按下窗口区域时处理
         *******************/
        public bool OnMouseDown(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            if (_btnType != EALGUIOpButtonType.OP_BTN)
                return false;

            _enterStat(ALGUIBaseMouseStat.DOWN);

            return true;
        }

        /*******************
         * 鼠标放开窗口区域时处理
         *******************/
        public bool OnMouseUp(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            if (_btnType != EALGUIOpButtonType.OP_BTN)
                return false;

            _quitStat(ALGUIBaseMouseStat.DOWN);

            //判断鼠标位置
            if (_pos.x < 0 || _pos.x > _m_rWndRect.width
                || _pos.y < 0 || _pos.y > _m_rWndRect.height)
            {
                _quitStat(ALGUIBaseMouseStat.OVER);
            }

            return true;
        }

        /**************
         * 进入状态
         **/
        protected void _enterStat(ALGUIBaseMouseStat _stat)
        {
            _m_eWndStat |= _stat;
        }

        /**************
         * 离开状态
         **/
        protected void _quitStat(ALGUIBaseMouseStat _stat)
        {
            _m_eWndStat &= ~_stat;
        }
    }
}

#endif
