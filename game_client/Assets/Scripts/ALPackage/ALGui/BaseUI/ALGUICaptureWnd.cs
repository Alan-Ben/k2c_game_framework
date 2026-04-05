using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public class ALGUICaptureWnd : ALGUIBaseWnd
    {
        /** 对象是否允许拖拽 */
        public bool dragEnable;

        /** 拖拽操作的处理delegate，默认为本类内的函数，可通过函数调整操作函数 */
        protected DealDragFunc _m_dDragFunc;
        /** 拖拽操作有效的范围，默认为1，1 */
        protected Vector2 _m_vDragEnableRange;

        public ALGUICaptureWnd()
            : base()
        {
            dragEnable = false;
            _m_dDragFunc = _dealDefaultDragFunc;
            _m_vDragEnableRange = new Vector2(1, 1);

            //将默认的鼠标按下以及弹起消息进行注册
            ALGUIRegMouseDownDelegate(_ALGUIDefalutOnMouseDown);
            ALGUIRegMouseUpDelegate(_ALGUIDefalutOnMouseUp);
        }
        public ALGUICaptureWnd(Rect _wndRect)
            : base(_wndRect)
        {
            dragEnable = false;
            _m_dDragFunc = _dealDefaultDragFunc;
            _m_vDragEnableRange = new Vector2(1, 1);

            //将默认的鼠标按下以及弹起消息进行注册
            ALGUIRegMouseDownDelegate(_ALGUIDefalutOnMouseDown);
            ALGUIRegMouseUpDelegate(_ALGUIDefalutOnMouseUp);
        }
        public ALGUICaptureWnd(ALGUIWndPositionStyle _posStyle)
            : base(_posStyle)
        {
            dragEnable = false;
            _m_dDragFunc = _dealDefaultDragFunc;
            _m_vDragEnableRange = new Vector2(1, 1);

            //将默认的鼠标按下以及弹起消息进行注册
            ALGUIRegMouseDownDelegate(_ALGUIDefalutOnMouseDown);
            ALGUIRegMouseUpDelegate(_ALGUIDefalutOnMouseUp);
        }

        /****************
         * 将对应默认鼠标响应函数去除
         **/
        public void unloadDefaultCapture()
        {
            ALGUIUnregMouseDownDelegate(_ALGUIDefalutOnMouseDown);
            ALGUIUnregMouseDownDelegate(_ALGUIDefalutOnMouseUp);
        }

        /*****************
         * 修改本窗口拖拽的处理函数指针
         **/
        public void chgDragDealFunc(DealDragFunc _dragFunc)
        {
            _m_dDragFunc = _dragFunc;
        }

        /*****************
         * 设置本窗口拖拽有效的移动区域范围
         **/
        public void setDragEnableRange(Vector2 _enableRange)
        {
            _m_vDragEnableRange = _enableRange;
        }

        /*******************
         * 默认的鼠标按下函数
         * 返回false则交由父窗口处理
         *******************/
        private bool _ALGUIDefalutOnMouseDown(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _mouseBtnType)
        {
            //设置捕获焦点，并返回处理消息
            ALGUISetCapture();

            //判断是否可以拖拽，且是否左键按下
            if (dragEnable && _mouseBtnType == EALGUIOpButtonType.OP_BTN && null != _m_dDragFunc)
            {
                //设置拖拽状态
                ALGUISetDrag(_m_dDragFunc, _m_vDragEnableRange);
            }

            return true;
        }

        /*******************
         * 默认的鼠标释放函数
         * 返回false则交由父窗口处理
         *******************/
        private bool _ALGUIDefalutOnMouseUp(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _mouseBtnType)
        {
            //判断是否可以拖拽
            if (isDragState && _mouseBtnType == EALGUIOpButtonType.OP_BTN)
            {
                //设置拖拽状态
                ALGUISetNotDrag();
            }

            //释放捕获的鼠标焦点，并返回处理消息
            ALGUIReleaseCapture();
            return true;
        }

        /********************
         * deal drag function
         **/
        private void _dealDefaultDragFunc(ALGUIBaseWnd _dragWnd, Vector2 _mouseMoveVector, Vector2 _mousePosToWnd)
        {
            if (null == _dragWnd)
                return;

            //move the wnd
            _dragWnd.ALGUIMoveWnd(_mouseMoveVector);
        }
    }
}

#endif
