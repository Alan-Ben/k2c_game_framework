using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /**********************
     * ALGUI中的拖拽信息处理以及控制对象
     **/
    public class ALGUIDragItemMgr
    {
        private static ALGUIDragItemMgr _g_instance = new ALGUIDragItemMgr();
        public static ALGUIDragItemMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALGUIDragItemMgr();

                return _g_instance;
            }
        }

        /** 是否在拖拽状态 */
        private bool _m_bIsDragging;
        /** 拖拽信息的源头窗口对象 */
        private ALGUIBaseWnd _m_wndDragItemSrcWnd;
        /** 当前拖拽信息对象 */
        private _AALGUIDragItemInfo _m_iiDragItemInfo;
        /** 拖拽的现实效果窗口 */
        private ALGUIBaseWnd _m_wndDragItemShowWnd;
        /** 拖拽的位置偏差 */
        private Vector2 _m_vDragDeltaPos;

        protected ALGUIDragItemMgr()
        {
            _m_bIsDragging = false;
            _m_wndDragItemSrcWnd = null;
            _m_iiDragItemInfo = null;
            _m_vDragDeltaPos = new Vector2();
        }

        /** 是否在拖拽状态 */
        public bool isDragging
        {
            get { return _m_bIsDragging; }
        }
        /** 拖拽信息的源头窗口对象 */
        public ALGUIBaseWnd dragItemSrcWnd
        {
            get { return _m_wndDragItemSrcWnd; }
        }
        /** 当前拖拽信息对象 */
        public _AALGUIDragItemInfo dragItemInfo
        {
            get { return _m_iiDragItemInfo; }
        }

        /**********************
         * 开始进行拖拽操作，并设置相关信息
         **/
        public void startDrag(ALGUIBaseWnd _srcWnd, _AALGUIDragItemInfo _dragItemInfo)
        {
            _m_bIsDragging = true;
            _m_wndDragItemSrcWnd = _srcWnd;
            _m_iiDragItemInfo = _dragItemInfo;

            if (null == _m_iiDragItemInfo)
                _m_iiDragItemInfo = new ALGUIEmptyDragItemInfo();

            //创建被拖拽的窗口
            if (null != _m_iiDragItemInfo)
            {
                //创建拖拽的效果窗口
                _m_wndDragItemShowWnd = _m_iiDragItemInfo._createDragWnd();
                if (null != _m_wndDragItemShowWnd)
                {
                    //获取便宜位置
                    _m_vDragDeltaPos = _m_iiDragItemInfo._getDragWndDeltaPos();
                }
            }
        }

        /********************
         * 将拖拽的信息放入UI中进行处理
         **/
        public void dropDragItem()
        {
            if (!_m_bIsDragging)
                return;

            ALGUIMain.instance.ALGUIDropDragItem(_m_iiDragItemInfo);

            //调用丢弃事件函数
            _m_iiDragItemInfo._onDrop();

            //重置拖拽信息
            _m_bIsDragging = false;
            _m_iiDragItemInfo = null;
            _m_wndDragItemSrcWnd = null;

            //释放拖拽的现实窗口
            if (null != _m_wndDragItemShowWnd)
            {
                _m_wndDragItemShowWnd.ALGUIRelease();
                _m_wndDragItemShowWnd = null;
            }
        }

        /****************
         * 绘制正在拖拽的窗口
         **/
        public void painDragWnd()
        {
            if (!_m_bIsDragging)
                return;

            Vector2 leftTopPos = ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN) + _m_vDragDeltaPos;

            if (null == _m_wndDragItemShowWnd)
            {
                Rect wndRect = new Rect(leftTopPos.x, leftTopPos.y, _m_wndDragItemSrcWnd.wndRect.width, _m_wndDragItemSrcWnd.wndRect.height);
                //开始本窗口的集合
                GUI.BeginGroup(wndRect);

                //无拖拽显示窗口则直接绘制源窗口
                _m_wndDragItemSrcWnd._ALGUIPainInGroup();

                GUI.EndGroup();
            }
            else
            {
                Rect wndRect = new Rect(leftTopPos.x, leftTopPos.y, _m_wndDragItemShowWnd.wndRect.width, _m_wndDragItemShowWnd.wndRect.height);
                //开始本窗口的集合
                GUI.BeginGroup(wndRect);

                //调用生成的窗口绘制函数
                _m_wndDragItemShowWnd._ALGUIPainInGroup();

                GUI.EndGroup();
            }
        }
    }
}

#endif
