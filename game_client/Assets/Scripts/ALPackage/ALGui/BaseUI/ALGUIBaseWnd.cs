using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /***************************
     * GUI模块中基础窗口类,所有控件都需要从此窗口中派生
     ***************************/
    public class ALGUIBaseWnd
    {
        private static int _g_iWndID = 1;

        protected static int MakeNewWndID() { return _g_iWndID++; }

        /** 鼠标移动数据，返回空，参数-坐标 */
        public delegate void MouseMoveAction(ALGUIBaseWnd _wnd, Vector2 _moveVector, Vector2 _pos);
        /** 鼠标移动数据，返回空，参数空 */
        public delegate void MouseMoveVoidAction(ALGUIBaseWnd _wnd);
        /** 鼠标事件函数，返回值为空 */
        public delegate void MouseVoidEvent(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _btnType);
        /** 鼠标事件函数，返回处理结果 */
        public delegate bool MouseBoolEvent(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _btnType);
        /** 大小更改事件 */
        public delegate void OnSizeEvent(ALGUIBaseWnd _wnd, Rect _oldRect, Rect _newRect);
        /** deal window drag function */
        public delegate void DealDragFunc(ALGUIBaseWnd _dragWnd, Vector2 _mouseMoveVector, Vector2 _mousePosToWnd);

        /** 拖拽操作移动数据，返回空，参数-坐标 */
        public delegate void DragMoveAction(ALGUIBaseWnd _wnd, _AALGUIDragItemInfo _dragItem, Vector2 _moveVector, Vector2 _pos);
        /** 拖拽操作移动数据，返回空，参数空 */
        public delegate void DragMoveVoidAction(ALGUIBaseWnd _wnd, _AALGUIDragItemInfo _dragItem);

        /** 窗口状态变更事件函数 */
        public delegate void WndStateVoidAction(ALGUIBaseWnd _wnd, bool _state);
        /** 窗口操作事件处理函数，需要返回处理结果 */
        public delegate bool WndBoolPressEvent(ALGUIBaseWnd _wnd, KeyCode _key);
        /** 窗口操作事件处理函数 */
        public delegate void WndAction(ALGUIBaseWnd _wnd);

        /** 窗口在拖动时产生拖拽信息的处理函数 */
        public delegate _AALGUIDragItemInfo WndCreateDragItem(ALGUIBaseWnd _wnd);
        /** 处理拖拽对象信息，返回处理成功或失败，失败将交给父容器处理 */
        public delegate bool WndBoolReceiveDragItem(ALGUIBaseWnd _wnd, _AALGUIDragItemInfo _itemInfo);

        /** 标志ID，用于查找窗口，或事件触发时使用 */
        public int wndID;
        /** 是否永久在顶层显示 */
        public bool alwaysTop;
        /** 窗口自动定位的样式对象 */
        public ALGUIWndPositionStyle positionStyle;

        /** 父窗体 */
        public ALGUIBaseWnd parentWnd;
        /** 子窗体列表 */
        public List<ALGUIBaseWnd> childrenWndList;

        /** 是否焦点对象 - 暂时不使用 */
        protected bool _m_bIsFouc;
        /** 对象是否正在进行拖拽 */
        protected bool _m_bIsDrag;
        /** 对象是否有效 */
        protected bool _m_bEnable;
        /** 是否显示 */
        protected internal bool _m_bVisible;

        /** the delegate to deal drag function */
        private DealDragFunc _m_dDealDragFunc;
        /** 开始拖拽时的鼠标位置 */
        private Vector2 _m_vDragStartMousePos;
        /** 拖拽出指定范围后才算开始拖拽，并调用拖拽处理函数的范围 */
        private Vector2 _m_vDragEnableRange;
        /** 是否已经超出拖拽感应范围的判断变量 */
        private bool _m_bIsDragEnable;


        /** 本窗口在父窗体中显示所占用的区域，此对象必须通过函数设置，因为会调用到onSize函数 */
        protected Rect _m_rWndRect;
        /** 上此鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wPreMouseWnd;
        /** 当前鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wCurMouseWnd;

        /** 拖拽信息时的窗口内操作信息存储对象 */
        /** 上此鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wPreDragMouseWnd;
        /** 当前鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wCurDragMouseWnd;

        /** 鼠标事件相关响应回调队列 */
        private List<MouseMoveAction> _m_lMouseMoveActionList = new List<MouseMoveAction>(1);
        private List<MouseMoveVoidAction> _m_lMouseInActionList = new List<MouseMoveVoidAction>(1);
        private List<MouseMoveVoidAction> _m_lMouseOutActionList = new List<MouseMoveVoidAction>(1);
        private List<MouseBoolEvent> _m_lMouseDownActionList = new List<MouseBoolEvent>(1);
        private List<MouseBoolEvent> _m_lMouseUpActionList = new List<MouseBoolEvent>(1);
        private List<MouseVoidEvent> _m_lMouseClickActionList = new List<MouseVoidEvent>(1);
        private List<MouseBoolEvent> _m_lMouseDoubleClickActionList = new List<MouseBoolEvent>(1);

        private List<OnSizeEvent> _m_lOnSizeEventList = new List<OnSizeEvent>(1);

        /** 拖拽操作相关相应回调 */
        private DragMoveAction _m_dDragMoveDelegate;
        private DragMoveVoidAction _m_dDragMoveInDelegate;
        private DragMoveVoidAction _m_dDragMoveOutDelegate;

        private WndStateVoidAction _m_dWndVisableActionDelegate;
        private WndStateVoidAction _m_dWndFocusActionDelegate;
        private WndStateVoidAction _m_dWndEnableActionDelegate;

        private WndAction _m_dWndPrintActionDelegate;
        private WndAction _m_dWndReleaseActionDelegate;

        private List<WndBoolPressEvent> _m_lWndPressActionList = new List<WndBoolPressEvent>(1);

        /** ALGUI中拖拽信息处理函数指针 */
        private WndCreateDragItem _m_dWndCreateDragItemDelegate;
        private List<WndBoolReceiveDragItem> _m_lWndReceiveDragItemActionList = new List<WndBoolReceiveDragItem>(1);

        /** tab键后的下一个窗口 */
        private ALGUIBaseWnd _m_wndTabNextWnd;

        public ALGUIBaseWnd()
        {
            wndID = MakeNewWndID();
            parentWnd = null;
            childrenWndList = new List<ALGUIBaseWnd>();
            _m_bIsDrag = false;
            _m_dDealDragFunc = null;

            _m_wPreMouseWnd = null;
            _m_wCurMouseWnd = null;
            positionStyle = new ALGUIWndPositionStyle();
            positionStyle.x = 0;
            positionStyle.y = 0;
            positionStyle.setSize(0, 0);

            _m_bIsFouc = false;
            _m_bEnable = true;
            _m_bVisible = true;
            _m_rWndRect = new Rect(0, 0, 0, 0);

            _m_wndTabNextWnd = null;
        }
        public ALGUIBaseWnd(Rect _wndRect)
        {
            wndID = MakeNewWndID();
            parentWnd = null;
            childrenWndList = new List<ALGUIBaseWnd>();
            _m_bIsDrag = false;
            _m_dDealDragFunc = null;

            _m_wPreMouseWnd = null;
            _m_wCurMouseWnd = null;
            positionStyle = new ALGUIWndPositionStyle();
            positionStyle.x = _wndRect.x;
            positionStyle.y = _wndRect.y;
            positionStyle.setSize(_wndRect.width, _wndRect.height);

            _m_bIsFouc = false;
            _m_bEnable = true;
            _m_bVisible = true;
            _m_rWndRect = new Rect(0, 0, 0, 0);

            _m_wndTabNextWnd = null;
        }
        public ALGUIBaseWnd(ALGUIWndPositionStyle _posStyle)
        {
            wndID = MakeNewWndID();
            parentWnd = null;
            childrenWndList = new List<ALGUIBaseWnd>();
            _m_bIsDrag = false;

            _m_wPreMouseWnd = null;
            _m_wCurMouseWnd = null;
            positionStyle = new ALGUIWndPositionStyle(_posStyle);

            _m_bIsFouc = false;
            _m_bEnable = true;
            _m_bVisible = true;
            _m_rWndRect = new Rect(0, 0, 0, 0);

            _m_wndTabNextWnd = null;
        }

        //tab切换的下个窗口对象
        public ALGUIBaseWnd tabNextWnd
        {
            get
            {
                return _m_wndTabNextWnd;
            }
            set
            {
                _m_wndTabNextWnd = value;
            }
        }

        /** get is this wnd dragging */
        public bool isDragState
        {
            get
            {
                return _m_bIsDrag;
            }
        }
        /** 判断当前是否是在拖拽有效状态下 */
        public bool isDragEnable
        {
            get
            {
                return _m_bIsDragEnable;
            }
        }

        /*******************
         * 在GUI机制内部调用的绘制函数，遍历各子节点进行绘制
         *******************/
        protected internal virtual void _ALGUIPain()
        {
            //开始本窗口的集合
            GUI.BeginGroup(_m_rWndRect);

            //进行具体的绘制操作
            _ALGUIPainInGroup();

            GUI.EndGroup();
        }
        protected internal virtual void _ALGUIPainInGroup()
        {
            try
            {
                //绘制本窗口
                OnPain();

                //逐个绘制非锁定顶层子窗口对象
                foreach (ALGUIBaseWnd wnd in childrenWndList)
                {
                    if (wnd.alwaysTop)
                        continue;

                    if (!wnd._m_bVisible)
                        continue;

                    //执行子窗口绘制函数
                    wnd._ALGUIPain();
                }

                //逐个绘制锁定顶层子窗口对象
                foreach (ALGUIBaseWnd wnd in childrenWndList)
                {
                    if (!wnd.alwaysTop)
                        continue;

                    if (!wnd._m_bVisible)
                        continue;

                    //执行子窗口绘制函数
                    wnd._ALGUIPain();
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
            }
        }

        /*******************************
         * 窗口内鼠标移动操作，带入参数为鼠标在窗口中的相对位置
         * 
         * 在此函数中不进行是否拦截的处理是因为假设本窗口不拦截，需要父窗口重新获取下一个需要捕获消息的窗口
         * 而这个逻辑消耗不小，同时存在一定的逻辑判断问题，因此MouseMove不做有效判断
         *******************************/
        protected internal bool _ALGUIMouseMove(Vector2 _moveVector, Vector2 _pos)
        {
            //是否将鼠标信号中断在此，默认中断，即在GUI上的鼠标消息不穿透到3D模块中
            bool interruptMes = true;

            //获取当前鼠标所在位置窗口信息
            _m_wCurMouseWnd = ALGUIHitTestWnd(_pos);

            //当前窗口与之前窗口不同时，进行之前窗口的鼠标移出操作
            if (_m_wCurMouseWnd != _m_wPreMouseWnd)
            {
                if (null != _m_wPreMouseWnd)
                {
                    //当之前窗口不为空时，需要执行鼠标移出操作
                    _m_wPreMouseWnd._ALGUIMouseOut();
                }

                if (null != _m_wCurMouseWnd)
                {
                    //执行目标窗口的鼠标移进操作
                    _m_wCurMouseWnd._ALGUIOnMouseMoveIn();
                }
            }

            //先处理父窗口的鼠标移动，再进行子窗口的处理
            _ALGUIOnMouseMove(_moveVector, _pos);

            if (null != _m_wCurMouseWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标移动处理
                interruptMes = _m_wCurMouseWnd._ALGUIMouseMove(_moveVector, _m_wCurMouseWnd.ALGUIGetParentMousePositionInWnd(_pos));
            }

            //设置之前窗口对象
            _m_wPreMouseWnd = _m_wCurMouseWnd;

            return interruptMes;
        }

        /*******************************
         * 窗口内鼠标按键按下操作，带入参数为鼠标在窗口中的相对位置
         *******************************/
        protected internal virtual bool _ALGUIMouseDown(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            //是否将鼠标信号中断在此，默认中断，即在GUI上的鼠标消息不穿透到3D模块中
            bool interruptMes = false;

            if (null != _m_wCurMouseWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标按下处理
                interruptMes = _m_wCurMouseWnd._ALGUIMouseDown(_m_wCurMouseWnd.ALGUIGetParentMousePositionInWnd(_pos), _btnType);
            }

            //先处理子窗口中的鼠标按下消息，当子窗口不拦截消息则在此窗口处理
            if (!interruptMes)
            {
                //在本窗口处理，并返回本窗口处理结果
                interruptMes = _ALGUIOnMouseDown(_pos, _btnType);

                if (interruptMes)
                {
                    //当本窗口拦截消息，表示本窗口处理该消息，因此设置该鼠标操作的窗口对象以及增加操作标志位。
                    ALGUIMain.instance._ALGUISetMouseDownWnd(_btnType, this);
                }
            }

            return interruptMes;
        }

        /*******************************
         * 窗口内鼠标按键弹起操作，带入参数为鼠标在窗口中的相对位置
         *******************************/
        protected internal bool _ALGUIMouseUp(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            //是否将鼠标信号中断在此，默认中断，即在GUI上的鼠标消息不穿透到3D模块中
            bool interruptMes = false;

            if (null != _m_wCurMouseWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标弹起处理
                interruptMes = _m_wCurMouseWnd._ALGUIMouseUp(_m_wCurMouseWnd.ALGUIGetParentMousePositionInWnd(_pos), _btnType);
            }

            //先处理子窗口中的鼠标弹起消息，当子窗口不拦截消息则在此窗口处理
            if (!interruptMes)
            {
                //获取当前是否拖拽有效状态
                bool preIsDragEnable = isDragEnable;
                //在本窗口处理，并返回本窗口处理结果
                interruptMes = _ALGUIOnMouseUp(_pos, _btnType);

                if (interruptMes)
                {
                    //当本窗口拦截消息，表示本窗口处理该消息，因此设置该鼠标操作的窗口对象以及增加操作标志位。
                    //判断释放位置是否在本窗口内，如果是在窗口内则表示进行了点击操作
                    //获取该鼠标按键按下的对象窗口
                    ALGUIBaseWnd pressWnd = ALGUIMain.instance._ALGUIGetMousePressWnd(_btnType);
                    if (pressWnd == this && _pos.x > 0 && _pos.x < _m_rWndRect.width
                        && _pos.y > 0 && _pos.y < _m_rWndRect.height        //判断是否在窗口区域内
                        && !preIsDragEnable)       //判断原先是否拖拽有效情况下
                    {
                        //当鼠标在本窗口按下，并在窗口内释放时触发鼠标的点击操作
                        ALGUIMain.instance._ALGUIWndMouseClick(this, _pos, _btnType);
                    }
                }
            }

            return interruptMes;
        }

        /*******************************
         * 窗口内鼠标移出操作，需要对之前窗口所在对象进行移出操作处理
         *******************************/
        protected internal void _ALGUIMouseOut()
        {
            //当鼠标之前在本窗口中某一窗口时，该窗口需要执行鼠标移出事件
            if (null != _m_wPreMouseWnd)
            {
                _m_wPreMouseWnd._ALGUIMouseOut();
            }

            //先执行子窗口的移出消息，后执行父窗口的移出消息
            _ALGUIOnMouseMoveOut();

            //充值鼠标对应窗口对象
            _m_wPreMouseWnd = null;
            _m_wCurMouseWnd = null;
        }

        /*******************************
         * 拖拽信息时窗口内鼠标移动的操作
         * 具体操作类似鼠标移动操作，但仅在拖拽信息时才调用
         *******************************/
        protected internal void _ALGUIDragMouseMove(_AALGUIDragItemInfo _dragItem, Vector2 _moveVector, Vector2 _pos)
        {
            //获取当前鼠标所在位置窗口信息
            _m_wCurDragMouseWnd = ALGUIHitTestWnd(_pos);

            //当前窗口与之前窗口不同时，进行之前窗口的鼠标移出操作
            if (_m_wCurDragMouseWnd != _m_wPreDragMouseWnd)
            {
                if (null != _m_wPreDragMouseWnd)
                {
                    //当之前窗口不为空时，需要执行鼠标移出操作
                    _m_wPreDragMouseWnd._ALGUIDragMouseOut(_dragItem);
                }

                if (null != _m_wCurDragMouseWnd)
                {
                    //执行目标窗口的鼠标移进操作
                    _m_wCurDragMouseWnd._ALGUIOnDragMouseMoveIn(_dragItem);
                }
            }

            //先处理父窗口的鼠标移动，再进行子窗口的处理
            _ALGUIOnDragMouseMove(_dragItem, _moveVector, _pos);

            if (null != _m_wCurDragMouseWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标移动处理
                _m_wCurDragMouseWnd._ALGUIDragMouseMove(_dragItem, _moveVector, _m_wCurDragMouseWnd.ALGUIGetParentMousePositionInWnd(_pos));
            }

            //设置之前窗口对象
            _m_wPreDragMouseWnd = _m_wCurDragMouseWnd;
        }

        /*******************************
         * 拖拽操作时，窗口内鼠标移出操作，需要对之前窗口所在对象进行移出操作处理
         *******************************/
        protected internal void _ALGUIDragMouseOut(_AALGUIDragItemInfo _dragItem)
        {
            //当鼠标之前在本窗口中某一窗口时，该窗口需要执行鼠标移出事件
            if (null != _m_wPreDragMouseWnd)
            {
                _m_wPreDragMouseWnd._ALGUIDragMouseOut(_dragItem);
            }

            //先执行子窗口的移出消息，后执行父窗口的移出消息
            _ALGUIOnDragMouseMoveOut(_dragItem);

            //重置鼠标对应窗口对象
            _m_wPreDragMouseWnd = null;
            _m_wCurDragMouseWnd = null;
        }

        /*******************************
         * 释放拖拽信息时，窗口接收拖拽信息的操作函数，返回本窗口是否接收了信息
         * 优先进行子窗口的接收操作
         *******************************/
        protected internal bool _ALGUIDropDragItem(_AALGUIDragItemInfo _dragItem, Vector2 _pos)
        {
            //是否接收成功，默认不成功
            bool interruptMes = false;

            //获取当前鼠标所在位置窗口信息
            ALGUIBaseWnd curWnd = ALGUIHitTestWnd(_pos);
            //执行拖拽信息移出操作
            _ALGUIDragMouseOut(_dragItem);

            //优先处理子窗口的接收函数
            if (null != curWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标移动处理
                interruptMes = curWnd._ALGUIDropDragItem(_dragItem, curWnd.ALGUIGetParentMousePositionInWnd(_pos));
            }

            //当子窗口未接收时，进行本窗口的接收处理
            if (!interruptMes)
                interruptMes = _ALGUIDealDelegateList(_m_lWndReceiveDragItemActionList, _dragItem);

            return interruptMes;
        }

        /**********************
         * 执行回调函数的内部归纳函数接口
         ***********************/
        private void _ALGUIDealDelegateList(List<MouseMoveVoidAction> _delegateList)
        {
            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    MouseMoveVoidAction actionDelegate = _delegateList[i];

                    if (null != actionDelegate)
                        actionDelegate(this);
                }
            }
        }
        private void _ALGUIDealDelegateList(List<MouseMoveAction> _delegateList, Vector2 _moveVector, Vector2 _pos)
        {
            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    MouseMoveAction actionDelegate = _delegateList[i];

                    if (null != actionDelegate)
                        actionDelegate(this, _moveVector, _pos);
                }
            }
        }
        private void _ALGUIDealDelegateList(List<MouseVoidEvent> _delegateList, Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    MouseVoidEvent actionDelegate = _delegateList[i];

                    if (null != actionDelegate)
                        actionDelegate(this, _pos, _btnType);
                }
            }
        }
        private bool _ALGUIDealDelegateList(List<MouseBoolEvent> _delegateList, Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            bool dealRes = false;

            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    MouseBoolEvent actionDelegate = _delegateList[i];

                    if (null != actionDelegate && actionDelegate(this, _pos, _btnType))
                    {
                        dealRes = true;
                    }
                }
            }

            return dealRes;
        }
        private void _ALGUIDealDelegateList(List<OnSizeEvent> _delegateList, Rect _oldRect, Rect _newRect)
        {
            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    OnSizeEvent actionDelegate = _delegateList[i];

                    if (null != actionDelegate)
                        actionDelegate(this, _oldRect, _newRect);
                }
            }
        }
        private bool _ALGUIDealDelegateList(List<WndBoolPressEvent> _delegateList, KeyCode _key)
        {
            bool dealRes = false;

            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    WndBoolPressEvent actionDelegate = _delegateList[i];

                    if (null != actionDelegate && actionDelegate(this, _key))
                    {
                        dealRes = true;
                    }
                }
            }

            return dealRes;
        }
        private bool _ALGUIDealDelegateList(List<WndBoolReceiveDragItem> _delegateList, _AALGUIDragItemInfo _itemInfo)
        {
            bool dealRes = false;

            if (_delegateList.Count > 0)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    WndBoolReceiveDragItem actionDelegate = _delegateList[i];

                    if (null != actionDelegate && actionDelegate(this, _itemInfo))
                    {
                        dealRes = true;
                    }
                }
            }

            return dealRes;
        }

        /*******************
         * 鼠标移动消息的处理，带入参数为本窗口相对坐标
         * 返回false则交由父窗口处理
         *******************/
        protected internal void _ALGUIOnMouseMove(Vector2 _moveVector, Vector2 _posToWnd)
        {
            _ALGUIDealDelegateList(_m_lMouseMoveActionList, _moveVector, _posToWnd);

            //调用移动函数
            _ALGUIDefalutOnMouseMove(_moveVector, _posToWnd);
        }

        /*******************
         * 鼠标移进窗口区域时处理
         * 返回false则交由父窗口处理
         *******************/
        protected internal void _ALGUIOnMouseMoveIn()
        {
            _ALGUIDealDelegateList(_m_lMouseInActionList);
        }

        /*******************
         * 鼠标移出窗口区域时处理
         * 返回false则交由父窗口处理
         *******************/
        protected internal void _ALGUIOnMouseMoveOut()
        {
            _ALGUIDealDelegateList(_m_lMouseOutActionList);
        }

        /*******************
         * 拖拽操作时，鼠标移动消息的处理，带入参数为本窗口相对坐标
         * 返回false则交由父窗口处理
         *******************/
        protected internal void _ALGUIOnDragMouseMove(_AALGUIDragItemInfo _dragItem, Vector2 _moveVector, Vector2 _posToWnd)
        {
            //调用回调
            if (null != ALGUIDragMoveDelegate)
                ALGUIDragMoveDelegate(this, _dragItem, _moveVector, _posToWnd);
        }

        /*******************
         * 拖拽操作时，鼠标移进窗口区域时处理
         * 返回false则交由父窗口处理
         *******************/
        protected internal void _ALGUIOnDragMouseMoveIn(_AALGUIDragItemInfo _dragItem)
        {
            //调用回调
            if (null != ALGUIDragMoveInDelegate)
                ALGUIDragMoveInDelegate(this, _dragItem);
        }

        /*******************
         * 拖拽操作时，鼠标移出窗口区域时处理
         * 返回false则交由父窗口处理
         *******************/
        protected internal void _ALGUIOnDragMouseMoveOut(_AALGUIDragItemInfo _dragItem)
        {
            //调用回调
            if (null != ALGUIDragMoveOutDelegate)
                ALGUIDragMoveOutDelegate(this, _dragItem);
        }

        /*******************
         * 鼠标在窗口内单击操作处理
         * 带入单击位置以及单击的鼠标按键类型
         *******************/
        protected internal void _ALGUIOnMouseClick(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            _ALGUIDealDelegateList(_m_lMouseClickActionList, _pos, _btnType);
        }

        /*******************
         * 鼠标在窗口内双击操作处理
         * 返回是否处理双击，当不处理时，转回单击操作处理
         *******************/
        protected internal bool _ALGUIOnMouseDoubleClick(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            return _ALGUIDealDelegateList(_m_lMouseDoubleClickActionList, _pos, _btnType);
        }

        /*******************
         * 鼠标按下
         * 返回false则交由父窗口处理
         *******************/
        protected internal bool _ALGUIOnMouseDown(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            return _ALGUIDealDelegateList(_m_lMouseDownActionList, _pos, _btnType);
        }

        /*******************
         * 鼠标释放
         * 返回false则交由父窗口处理
         *******************/
        protected internal bool _ALGUIOnMouseUp(Vector2 _pos, EALGUIOpButtonType _mouseBtnType)
        {
            return _ALGUIDealDelegateList(_m_lMouseUpActionList, _pos, _mouseBtnType);
        }

        /*******************
         * 窗口更改处理函数
         * 返回false则交由父窗口处理
         *******************/
        protected internal void _ALGUIOnSize(Rect _oldRect, Rect _newRect)
        {
            _ALGUIDealDelegateList(_m_lOnSizeEventList, _oldRect, _newRect);

            //调用默认函数
            _ALGUIDefalutOnSize(_oldRect, _newRect);
        }

        /*********************
         * 设置窗口大小尺寸
         *********************/
        protected internal void _ALGUISetWndSize(Rect _newWndRect)
        {
            //保存旧窗口尺寸
            Rect oldRect = _m_rWndRect;

            //设置新尺寸
            _m_rWndRect = new Rect(_newWndRect);

            if (_newWndRect.width != oldRect.width
                || _newWndRect.height != oldRect.height)
            {
                //当窗口尺寸更改时，需要进行OnSize事件处理
                _ALGUIOnSize(oldRect, _newWndRect);
            }
        }

        /*******************
         * 默认的鼠标移动函数
         *******************/
        private void _ALGUIDefalutOnMouseMove(Vector2 _moveVector, Vector2 _posToWnd)
        {
            if (isDragState && null != _m_dDealDragFunc)
            {
                //判断是否在拖拽行为下
                //判断拖拽行为是否已经超出生效范围了
                if (!isDragEnable)
                {
                    //判断当前鼠标位置是否超出范围
                    Vector2 moveRange = ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN) - _m_vDragStartMousePos;
                    if (Mathf.Abs(moveRange.x) >= _m_vDragEnableRange.x
                        || Mathf.Abs(moveRange.y) >= _m_vDragEnableRange.y)
                    {
                        //超出范围则设置窗口拖拽操作有效
                        _m_bIsDragEnable = true;
                        //设置拖拽移动的距离信息
                        _moveVector = moveRange;

                        //根据本窗口的处理函数，获取本窗口被拖拽时的拖拽信息对象
                        if (null != _m_dWndCreateDragItemDelegate)
                        {
                            //设置GUI拖拽处理器状态
                            ALGUIDragItemMgr.instance.startDrag(this, _m_dWndCreateDragItemDelegate(this));
                        }
                    }
                }

                //只有当本窗口不是拖拽源窗口，且拖拽生效时才调用拖拽的行为处理函数
                if ((!ALGUIDragItemMgr.instance.isDragging || ALGUIDragItemMgr.instance.dragItemSrcWnd != this)
                    && isDragEnable)
                    _m_dDealDragFunc(this, _moveVector, _posToWnd);
            }
        }

        /*******************
         * 默认的窗口大小更改函数
         * 返回false则交由父窗口处理
         *******************/
        private void _ALGUIDefalutOnSize(Rect _oldRect, Rect _newRect)
        {
            //遍历每个子窗体，根据子窗体的不同风格，设置子窗体新的位置
            foreach (ALGUIBaseWnd wnd in childrenWndList)
            {
                //仅档子窗体的位置样式有效才进行计算
                if (null == wnd.positionStyle)
                    continue;

                //计算新的默认区域
                Rect wndNewRect = wnd.positionStyle.ALGUIGetNewRect(_newRect);

                //设置窗口新尺寸
                wnd._ALGUISetWndSize(wndNewRect);
            }
        }

        /*******************
         * 窗口被注销的函数，在此函数内调用窗口的注销虚函数
         *******************/
        private void _ALGUIDefalutOnRelease()
        {
            //遍历每个子窗体，根据子窗体的不同风格，设置子窗体新的位置
            foreach (ALGUIBaseWnd wnd in childrenWndList)
            {
                //调用子对象的释放函数
                wnd.ALGUIRelease();

                //解除关联
                wnd.parentWnd = null;
            }

            childrenWndList.Clear();
        }

        /**************************
         * 注册子窗体
         **************************/
        public void ALGUIRegChildWnd(ALGUIBaseWnd _wnd)
        {
            childrenWndList.Add(_wnd);

            _wnd.parentWnd = this;
            //刷新窗口位置相关信息
            _wnd.ALGUIResetWndPos();
        }
        public void ALGUIRegChildWnd(ALGUIBaseWnd _wnd, bool _show)
        {
            childrenWndList.Add(_wnd);

            _wnd.parentWnd = this;

            if (_show)
                _wnd.show();
            else
                _wnd.hide();

            //刷新窗口位置相关信息
            _wnd.ALGUIResetWndPos();
        }

        /**************************
         * 注销子窗体
         **************************/
        public void ALGUIUnregChildWnd(ALGUIBaseWnd _wnd)
        {
            childrenWndList.Remove(_wnd);
        }

        /**************************
         * 释放窗体相关资源
         **************************/
        public void ALGUIRelease()
        {
            OnRelease();

            //调用内部的释放事件
            _ALGUIDefalutOnRelease();
        }

        /**********************
         * 设置本窗口为鼠标独占窗口
         **********************/
        public void ALGUISetCapture()
        {
            ALGUIMain.instance.ALGUISetCapture(this);
        }

        /****************
         * 注销本窗体的显示
         **/
        public void ALGUIUnreg()
        {
            ALGUIMain.instance.ALGUIUnregWnd(this);
        }

        /**********************
         * 解除本窗口对鼠标的独占
         **********************/
        public void ALGUIReleaseCapture()
        {
            ALGUIMain.instance.ALGUIReleaseCapture(this);
        }

        /*******************
         * set this wnd to  be drag function
         **/
        public void ALGUISetDrag(DealDragFunc _dealFunc)
        {
            ALGUISetDrag(_dealFunc, new Vector2(1, 1));
        }
        public void ALGUISetDrag(DealDragFunc _dealFunc, Vector2 _dragEnableRange)
        {
            if (null == _dealFunc)
            {
                _m_bIsDrag = false;
                return;
            }

            _m_bIsDrag = true;
            _m_dDealDragFunc = _dealFunc;

            //设置拖拽开始的鼠标位置
            _m_vDragStartMousePos = ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN);
            //设置拖拽操作的生效范围
            _m_vDragEnableRange = _dragEnableRange;
            //设置初始拖拽未生效
            _m_bIsDragEnable = false;
        }

        public void ALGUISetNotDrag()
        {
            _m_bIsDrag = false;
            _m_dDealDragFunc = null;
            _m_bIsDragEnable = false;

            //判断本窗口是否拖拽信息操作的源窗口，是则需要调用丢弃拖拽信息的操作函数
            if (ALGUIDragItemMgr.instance.dragItemSrcWnd == this)
                ALGUIDragItemMgr.instance.dropDragItem();
        }

        /******************
         * move the window distance
         **/
        public void ALGUIMoveWnd(Vector2 _moveDis)
        {
            //计算移动位移
            _m_rWndRect.x += _moveDis.x;
            _m_rWndRect.y += _moveDis.y;

            //在窗口位置信息中修改位移
            positionStyle.x += _moveDis.x;
            positionStyle.y += _moveDis.y;
        }

        /**********************
         * 根据父窗口中鼠标位置换算出本窗口内鼠标位置
         **********************/
        public Vector2 ALGUIGetParentMousePositionInWnd(Vector2 _parentMousePos)
        {
            Vector2 pos = new Vector2(_parentMousePos.x - _m_rWndRect.x, _parentMousePos.y - _m_rWndRect.y);

            return pos;
        }

        /**********************
         * 根据全局鼠标位置获取鼠标在当前窗口中的相对位置
         **********************/
        public Vector2 ALGUIGetMousePositionInWnd(Vector2 _mousePos)
        {
            /******************
             * 以下是递归方式，由于效率考虑，不考虑递归
                if (null == _m_ParentWnd)
                    return _ALGUIGetParentMousePositionInWnd(_mousePos);

                //获取鼠标位置在父窗口中的位置
                Vector2 parentMousePos = _m_ParentWnd._ALGUIGetMousePositionInWnd(_mousePos);
                //返回父亲窗口中鼠标位置在本窗口的位置
                return ALGUIGetParentMousePositionInWnd(parentMousePos);
             *******************/
            List<ALGUIBaseWnd> parentWndList = new List<ALGUIBaseWnd>();
            ALGUIBaseWnd parentWnd = this;

            //将所有父窗口取出放入队列
            while (null != parentWnd.parentWnd)
            {
                parentWnd = parentWnd.parentWnd;
                parentWndList.Add(parentWnd);
            }

            //遍历每个队列取鼠标位置在该窗口中的位置
            Vector2 pos = new Vector2(_mousePos.x, _mousePos.y);
            for (int i = parentWndList.Count - 1; i >= 0; i--)
            {
                ALGUIBaseWnd wnd = parentWndList[i];
                pos = wnd.ALGUIGetParentMousePositionInWnd(pos);
            }

            //最后得到父亲窗口中的鼠标位置
            return ALGUIGetParentMousePositionInWnd(pos);
        }

        /**********************
         * 父窗口中的位置是否在本窗口内
         **********************/
        public bool ALGUIParentPositionInWnd(Vector2 _pos)
        {
            return _m_rWndRect.Contains(_pos);
        }

        /**********************
         * 全局GUI中鼠标位置是否在窗口内
         **********************/
        public bool ALGUIPositionInWnd(Vector2 _pos)
        {
            if (null == parentWnd)
                return ALGUIParentPositionInWnd(_pos);

            //获取全局位置在本窗口中的位置
            Vector2 wndMousePos = ALGUIGetMousePositionInWnd(_pos);
            //将在本窗口中的坐标与本窗口长宽进行对比获得该坐标是否在本窗口内
            if (wndMousePos.x < _m_rWndRect.width && wndMousePos.y < _m_rWndRect.height)
                return true;
            else
                return false;
        }

        /*********************
         * 根据鼠标在当前窗口位置获取在该位置的本窗口内子窗口对象
         *********************/
        public ALGUIBaseWnd ALGUIHitTestWnd(Vector2 _pos)
        {
            //倒序计算，因为顺序最后的显示次序最高
            for (int i = childrenWndList.Count - 1; i >= 0; i--)
            {
                ALGUIBaseWnd wnd = childrenWndList[i];

                //第一次只检查顶层窗口
                if (!wnd.alwaysTop)
                    continue;

                //窗口无法显示则直接过滤
                if (!wnd._m_bVisible || !wnd._m_bEnable)
                    continue;

                //当坐标在该窗口内则直接返回该窗口
                if (wnd.ALGUIParentPositionInWnd(_pos))
                    return wnd;
            }

            for (int i = childrenWndList.Count - 1; i >= 0; i--)
            {
                ALGUIBaseWnd wnd = childrenWndList[i];

                //第二次不检查顶层窗口
                if (wnd.alwaysTop)
                    continue;

                //窗口无法显示则直接过滤
                if (!wnd._m_bVisible || !wnd._m_bEnable)
                    continue;

                //当坐标在该窗口内则直接返回该窗口
                if (wnd.ALGUIParentPositionInWnd(_pos))
                    return wnd;
            }

            return null;
        }

        /*********************
         * 获取本窗口父窗口中的顶层窗口对象
         *********************/
        public ALGUIBaseWnd getTopWnd()
        {
            //迭代寻找顶层窗口
            ALGUIBaseWnd topWnd = this;
            while (null != topWnd.parentWnd)
            {
                topWnd = topWnd.parentWnd;
            }

            return topWnd;
        }
        /**************
         * 获取本窗口作为带入根节点窗口的子窗口的最高级窗口，否则则获取最高级窗口
         **/
        public ALGUIBaseWnd getTopWnd(ALGUIBaseWnd _rootWnd)
        {
            if (_rootWnd == parentWnd)
                return this;

            //迭代寻找顶层窗口
            ALGUIBaseWnd topWnd = this;
            while (_rootWnd != topWnd.parentWnd && null != topWnd.parentWnd)
            {
                topWnd = topWnd.parentWnd;
            }

            return topWnd;
        }

        public bool visible
        {
            get
            {
                return _m_bVisible;
            }
        }

        public bool isEnable
        {
            get
            {
                return _m_bEnable;
            }
        }

        public bool isFocus
        {
            get
            {
                return _m_bIsFouc;
            }
        }

        public Rect wndRect
        {
            get { return _m_rWndRect; }
        }

        public float width
        {
            get
            {
                return _m_rWndRect.width;
            }
        }

        public float height
        {
            get
            {
                return _m_rWndRect.height;
            }
        }

        /***************
         * 显隐函数
         ***************/
        public void show()
        {
            _m_bVisible = true;

            if (null != ALGUIWndVisableActionDelegate)
                ALGUIWndVisableActionDelegate(this, true);
        }
        public void hide()
        {
            _m_bVisible = false;

            if (null != ALGUIWndVisableActionDelegate)
                ALGUIWndVisableActionDelegate(this, false);
        }

        /***************
         * 设置焦点状态函数
         ***************/
        public void focus()
        {
            ALGUIMain.instance._ALGUISetFocusWnd(this);
        }
        public void unfocus()
        {
            if (!_m_bIsFouc)
                return;

            ALGUIMain.instance._ALGUISetFocusWnd(null);
        }

        /***************
         * 设置窗口有效状态
         ***************/
        public void enable()
        {
            _m_bEnable = true;

            if (null != ALGUIWndEnableActionDelegate)
                ALGUIWndEnableActionDelegate(this, true);
        }
        public void disable()
        {
            _m_bEnable = false;

            if (null != ALGUIWndEnableActionDelegate)
                ALGUIWndEnableActionDelegate(this, false);
        }

        /*******************
         * 构建窗口界面函数
         *******************/
        public void OnPain()
        {
            if (null != ALGUIWndPrintActionDelegate)
                ALGUIWndPrintActionDelegate(this);
        }

        /****************
         * 窗口在被释放时进行的操作
         **/
        public void OnRelease()
        {
            if (null != ALGUIWndReleaseActionDelegate)
                ALGUIWndReleaseActionDelegate(this);
        }

        /********************
         * 本窗口接收拖拽物信息的处理函数，返回是否处理成功
         **/
        public bool OnReceiveDragItem(_AALGUIDragItemInfo _itemInfo)
        {
            return _ALGUIDealDelegateList(_m_lWndReceiveDragItemActionList, _itemInfo);
        }

        /****************
         * 窗口在为焦点模式下按下Enter按键时的事件函数
         * 返回值表示是否截取对应消息
         **/
        public bool OnPressKey(KeyCode _key)
        {
            //调用注册的回调函数队列
            return _ALGUIDealDelegateList(_m_lWndPressActionList, _key);
        }

        /***************************
         * 注册鼠标相关操作消息的相应函数
         * public List<MouseMoveAction> _m_lMouseMoveActionList        = new List<MouseMoveAction>(1);
         * public List<MouseMoveVoidAction> _m_lMouseInActionList      = new List<MouseMoveVoidAction>(1);
         * public List<MouseMoveVoidAction> _m_lMouseOutActionList     = new List<MouseMoveVoidAction>(1);
         * public List<MouseBoolEvent> _m_lMouseDownActionList         = new List<MouseBoolEvent>(1);
         * public List<MouseBoolEvent> _m_lMouseUpActionList           = new List<MouseBoolEvent>(1);
         * public List<MouseVoidEvent> _m_lMouseClickActionList        = new List<MouseVoidEvent>(1);
         * public List<MouseBoolEvent> _m_lMouseDoubleClickActionList  = new List<MouseBoolEvent>(1);
         ***************************/
        public void ALGUIRegMouseMoveDelegate(MouseMoveAction _delegate)
        {
            if (null != _delegate)
                _m_lMouseMoveActionList.Add(_delegate);
        }
        public void ALGUIRegMouseInDelegate(MouseMoveVoidAction _delegate)
        {
            if (null != _delegate)
                _m_lMouseInActionList.Add(_delegate);
        }
        public void ALGUIRegMouseOutDelegate(MouseMoveVoidAction _delegate)
        {
            if (null != _delegate)
                _m_lMouseOutActionList.Add(_delegate);
        }
        public void ALGUIRegMouseDownDelegate(MouseBoolEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseDownActionList.Add(_delegate);
        }
        public void ALGUIRegMouseUpDelegate(MouseBoolEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseUpActionList.Add(_delegate);
        }
        public void ALGUIRegMouseClickDelegate(MouseVoidEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseClickActionList.Add(_delegate);
        }
        public void ALGUIRegMouseDoubleClickDelegate(MouseBoolEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseDoubleClickActionList.Add(_delegate);
        }

        /********************
         * 反注册回调函数
         ********************/
        public void ALGUIUnregMouseMoveDelegate(MouseMoveAction _delegate)
        {
            if (null != _delegate)
                _m_lMouseMoveActionList.Remove(_delegate);
        }
        public void ALGUIUnregMouseInDelegate(MouseMoveVoidAction _delegate)
        {
            if (null != _delegate)
                _m_lMouseInActionList.Remove(_delegate);
        }
        public void ALGUIUnregMouseOutDelegate(MouseMoveVoidAction _delegate)
        {
            if (null != _delegate)
                _m_lMouseOutActionList.Remove(_delegate);
        }
        public void ALGUIUnregMouseDownDelegate(MouseBoolEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseDownActionList.Remove(_delegate);
        }
        public void ALGUIUnregMouseUpDelegate(MouseBoolEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseUpActionList.Remove(_delegate);
        }
        public void ALGUIUnregMouseClickDelegate(MouseVoidEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseClickActionList.Remove(_delegate);
        }
        public void ALGUIUnregMouseDoubleClickDelegate(MouseBoolEvent _delegate)
        {
            if (null != _delegate)
                _m_lMouseDoubleClickActionList.Remove(_delegate);
        }

        /********************
         * 清空回调函数
         ********************/
        public void ALGUIClearMouseMoveDelegate()
        {
            _m_lMouseMoveActionList.Clear();
        }
        public void ALGUIClearMouseInDelegate()
        {
            _m_lMouseInActionList.Clear();
        }
        public void ALGUIClearMouseOutDelegate()
        {
            _m_lMouseOutActionList.Clear();
        }
        public void ALGUIClearMouseDownDelegate()
        {
            _m_lMouseDownActionList.Clear();
        }
        public void ALGUIClearMouseUpDelegate()
        {
            _m_lMouseUpActionList.Clear();
        }
        public void ALGUIClearMouseClickDelegate()
        {
            _m_lMouseClickActionList.Clear();
        }
        public void ALGUIClearMouseDoubleClickDelegate()
        {
            _m_lMouseDoubleClickActionList.Clear();
        }

        /************************
         * 清空所有鼠标响应
         ************************/
        public void ALGUIClearAllMouseDelegate()
        {
            ALGUIClearMouseClickDelegate();
            ALGUIClearMouseDoubleClickDelegate();
            ALGUIClearMouseDownDelegate();
            ALGUIClearMouseInDelegate();
            ALGUIClearMouseMoveDelegate();
            ALGUIClearMouseOutDelegate();
            ALGUIClearMouseUpDelegate();
        }

        /***************************
         * 注册窗口更改大小消息的相应函数
         * private List<OnSizeEvent> _m_lOnSizeEventList                   = new List<OnSizeEvent>(1);
         ***************************/
        public void ALGUIRegOnSizeDelegate(OnSizeEvent _delegate)
        {
            if (null != _delegate)
                _m_lOnSizeEventList.Add(_delegate);
        }
        public void ALGUIUnregOnSizeDelegate(OnSizeEvent _delegate)
        {
            if (null != _delegate)
                _m_lOnSizeEventList.Remove(_delegate);
        }
        public void ALGUIClearOnSizeDelegate()
        {
            _m_lOnSizeEventList.Clear();
        }

        /***************************
         * 拖拽信息操作回调
         *private DragMoveAction _m_dDragMoveDelegate;
         *private DragMoveVoidAction _m_dDragMoveInDelegate;
         *private DragMoveVoidAction _m_dDragMoveOutDelegate;
         ***************************/
        public DragMoveAction ALGUIDragMoveDelegate { get { return _m_dDragMoveDelegate; } set { _m_dDragMoveDelegate = value; } }
        public DragMoveVoidAction ALGUIDragMoveInDelegate { get { return _m_dDragMoveInDelegate; } set { _m_dDragMoveInDelegate = value; } }
        public DragMoveVoidAction ALGUIDragMoveOutDelegate { get { return _m_dDragMoveOutDelegate; } set { _m_dDragMoveOutDelegate = value; } }

        /***************************
         * 注册窗口相关操作的事件函数
         *private List<WndStateVoidAction> _m_lWndVisableActionList       = new List<WndStateVoidAction>(1);
         *private List<WndStateVoidAction> _m_lWndFocusActionList         = new List<WndStateVoidAction>(1);
         *private List<WndStateVoidAction> _m_lWndEnableActionList        = new List<WndStateVoidAction>(1);
         *
         *private List<WndBoolPressEvent> _m_lWndPressActionList          = new List<WndBoolPressEvent>(1);
         *
         *private List<WndAction> _m_lWndPrintActionList                  = new List<WndAction>(1);
         *private List<WndAction> _m_lWndReleaseActionList                = new List<WndAction>(1);
         ***************************/
        public WndStateVoidAction ALGUIWndVisableActionDelegate { get { return _m_dWndVisableActionDelegate; } set { _m_dWndVisableActionDelegate = value; } }
        public WndStateVoidAction ALGUIWndFocusActionDelegate { get { return _m_dWndFocusActionDelegate; } set { _m_dWndFocusActionDelegate = value; } }
        public WndStateVoidAction ALGUIWndEnableActionDelegate { get { return _m_dWndEnableActionDelegate; } set { _m_dWndEnableActionDelegate = value; } }
        public WndAction ALGUIWndPrintActionDelegate { get { return _m_dWndPrintActionDelegate; } set { _m_dWndPrintActionDelegate = value; } }
        public WndAction ALGUIWndReleaseActionDelegate { get { return _m_dWndReleaseActionDelegate; } set { _m_dWndReleaseActionDelegate = value; } }

        public void ALGUIRegWndPressDelegate(WndBoolPressEvent _delegate)
        {
            if (null != _delegate)
                _m_lWndPressActionList.Add(_delegate);
        }
        public void ALGUIRegWndReceiveDragItemDelegate(WndBoolReceiveDragItem _delegate)
        {
            if (null != _delegate)
                _m_lWndReceiveDragItemActionList.Add(_delegate);
        }

        /********************
         * 反注册回调函数
         ********************/
        public void ALGUIUnregWndPressDelegate(WndBoolPressEvent _delegate)
        {
            if (null != _delegate)
                _m_lWndPressActionList.Remove(_delegate);
        }
        public void ALGUIUnregWndReceiveDragItemDelegate(WndBoolReceiveDragItem _delegate)
        {
            if (null != _delegate)
                _m_lWndReceiveDragItemActionList.Remove(_delegate);
        }

        /********************
         * 清空回调函数
         ********************/
        public void ALGUIClearWndPressDelegate()
        {
            _m_lWndPressActionList.Clear();
        }
        public void ALGUIClearWndReceiveDragItemDelegate()
        {
            _m_lWndReceiveDragItemActionList.Clear();
        }

        /*****************
         * 设置本窗口被拖拽时，生成拖拽物信息的函数指针
         **/
        public void setWndCreateDragItemInfoDelegate(WndCreateDragItem _delegate)
        {
            _m_dWndCreateDragItemDelegate = _delegate;
        }

        /*******************
         * 刷新窗口所在位置以及大小
         *******************/
        public virtual void ALGUIResetWndPos()
        {
            Rect parentRect;
            if (null == parentWnd)
            {
                parentRect = new Rect(0, 0, Screen.width, Screen.height);
            }
            else
            {
                parentRect = parentWnd._m_rWndRect;
            }

            //计算新的默认区域
            Rect wndNewRect = positionStyle.ALGUIGetNewRect(parentRect);

            //设置窗口新尺寸
            _ALGUISetWndSize(wndNewRect);
        }

        /***************
         * 设置焦点状态处理函数
         ***************/
        protected internal void _focus()
        {
            _m_bIsFouc = true;

            if (null != ALGUIWndFocusActionDelegate)
                ALGUIWndFocusActionDelegate(this, true);
        }
        protected internal void _unfocus()
        {
            _m_bIsFouc = false;

            if (null != ALGUIWndFocusActionDelegate)
                ALGUIWndFocusActionDelegate(this, false);
        }


        /******************
         * 从子窗口队列中拿出某一窗口
         ******************/
        protected ALGUIBaseWnd _ALGUIPopWnd(ALGUIBaseWnd _wnd)
        {
            for (int i = 0; i < childrenWndList.Count; i++)
            {
                ALGUIBaseWnd wnd = childrenWndList[i];
                if (null == wnd)
                    continue;

                if (wnd == _wnd)
                {
                    childrenWndList.RemoveAt(i);
                    return wnd;
                }
            }

            return null;
        }

        /******************
         * 向子窗口队列末尾中插入一个窗口
         * 窗口将显示在最顶层
         ******************/
        protected ALGUIBaseWnd _ALGUIPushWnd(ALGUIBaseWnd _wnd)
        {
            if (null == _wnd)
                return null;

            childrenWndList.Add(_wnd);

            return _wnd;
        }

        /******************
         * 向子窗口队列头部中插入一个窗口
         * 窗口将显示在最底层
         ******************/
        protected ALGUIBaseWnd _ALGUIPullWndToBottom(ALGUIBaseWnd _wnd)
        {
            if (null == _wnd)
                return null;

            //插入0的位置
            childrenWndList.Insert(0, _wnd);

            return _wnd;
        }
    }
}

#endif
