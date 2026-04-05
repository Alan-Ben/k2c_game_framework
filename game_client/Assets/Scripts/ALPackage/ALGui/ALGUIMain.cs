using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public class ALGUIMain
    {
        private static ALGUIMain g_instance = new ALGUIMain();

        public static ALGUIMain instance
        {
            get
            {
                if (null == g_instance)
                    g_instance = new ALGUIMain();

                return g_instance;
            }
        }

        /** 大小更改事件 */
        public delegate void OnSizeEvent(Rect _oldRect, Rect _newRect);

        /** 当前鼠标点击状态存储，如果是LEFT+RIGHT则为 LEFT | RIGHT */
        public int opBtnPressState;

        /** 顶层窗口队列，按照此队列进行相关绘制 */
        protected List<_AALBaseGuiStage> _m_lStageList;
        /** 上次鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wPreMouseWnd;
        /** 当前鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wCurMouseWnd;
        /** 当前焦点窗口对象 */
        protected ALGUIBaseWnd _m_wCurFocusWnd;
        /** 上帧的窗口大小 */
        public int _m_iPreWndWidth;
        public int _m_iPreWndHeight;

        /** 拖拽信息的鼠标操作相关结构体 */
        /** 上次鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wPreDragMouseWnd;
        /** 当前鼠标位置所在窗口对象 */
        protected ALGUIBaseWnd _m_wCurDragMouseWnd;

        /** 是否在独占鼠标状态 */
        protected bool _m_bMouseCaptureAble;
        /** 当前独占鼠标消息的窗口对象 */
        protected ALGUIBaseWnd _m_wCaptureWnd;

        /** 鼠标点击对象的窗口集合存储 */
        protected ALGUIBaseWnd[] _m_dMousePressWnd;

        /** 最后一个单击的窗口对象 */
        protected ALGUIBaseWnd[] _m_dLastMouseClickWnd;
        /** 最后一次单击的时间 */
        protected long[] _m_dLastMouseClickTime;
        /** 文字部分隐藏背景贴图的样式 */
        protected internal GUIStyle _m_sTextAreaStyle;

        /** 屏幕尺寸变动回调 */
        protected Action<int, int> _m_dScreenSizeChgDelegate;

        /***********
         * 构造函数不对外开放
         ***********/
        protected ALGUIMain()
        {
            _m_lStageList = new List<_AALBaseGuiStage>();

            _m_bMouseCaptureAble = false;
            _m_wCaptureWnd = null;

            _m_wPreMouseWnd = null;
            _m_wCurMouseWnd = null;

            _m_iPreWndWidth = 0;
            _m_iPreWndHeight = 0;

            int opBtnCount = ALCommon.getEnumCount(typeof(EALGUIOpButtonType));

            _m_dMousePressWnd = new ALGUIBaseWnd[opBtnCount];

            _m_dLastMouseClickWnd = new ALGUIBaseWnd[opBtnCount];
            _m_dLastMouseClickTime = new long[opBtnCount];

            _m_sTextAreaStyle = new GUIStyle();

            _m_dScreenSizeChgDelegate = default(Action<int, int>);
        }

        public Action<int, int> screenSizeChgDelegate { get { return _m_dScreenSizeChgDelegate; } set { _m_dScreenSizeChgDelegate = value; } }

        /**********************
         * 部分数据初始化，用于在脚本开始的时候使用的
         **/
        public void init()
        {
            _m_sTextAreaStyle.border = new RectOffset(0, 0, 0, 0);
        }

        /************************
         * 执行主GUI任务的函数
         * 返回值表明是否在此拦截鼠标消息
         * 
         * 鼠标操作的捕获函数示例如下
         *     if(Input.GetMouseButtonDown(0))
         *         Debug.Log("Pressed left click.");
         *     if(Input.GetMouseButtonDown(1))
         *         Debug.Log("Pressed right click.");
         *     if(Input.GetMouseButtonDown(2))
         *         Debug.Log("Pressed middle click.");
         *         
         ************************/
        public bool ALGUIExec()
        {
            //判断窗口大小
            int curWndWidth = Screen.width;
            int curWndHeight = Screen.height;

            if (curWndWidth != _m_iPreWndWidth || curWndHeight != _m_iPreWndHeight)
            {
                if (null != _m_dScreenSizeChgDelegate)
                    _m_dScreenSizeChgDelegate(curWndWidth, curWndHeight);

                //当窗口总尺寸更改时触发GUI更改尺寸通知
                _ALGUIMainDefalutOnSize(new Rect(0, 0, _m_iPreWndWidth, _m_iPreWndHeight), new Rect(0, 0, curWndWidth, curWndHeight));

                //重新设置之前窗口尺寸
                _m_iPreWndWidth = curWndWidth;
                _m_iPreWndHeight = curWndHeight;
            }

            //使用输入对象进行检测
            ALInputControl.instance.frameCheck();

            //是否捕获消息的消息处理
            bool interruptMes = false;
            //鼠标移动进行处理，返回是否捕获了鼠标移动消息
            interruptMes = _ALGUIMouseMove(ALInputControl.instance.getBtnFrameMoveVector(EALGUIOpButtonType.OP_BTN), ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN));

            //获取当前正在进行的拖拽信息对象
            if (ALGUIDragItemMgr.instance.isDragging)
            {
                //调用拖拽时的移动操作，此操作与正常的移动操作区分开
                _ALGUIDragMouseMove(ALGUIDragItemMgr.instance.dragItemInfo, ALInputControl.instance.getBtnFrameMoveVector(EALGUIOpButtonType.OP_BTN), ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN));
            }

            //判断是否有按键按下消息，且原先本窗口未捕获到对应事件
            if (EALGUIOpButtonType.NONE != ALInputControl.instance.btnDownEvent && !ALGUIGetMouseBtnPressStat(ALInputControl.instance.btnDownEvent))
            {
                //当有鼠标按键按下时触发相关操作时，根据鼠标当前位置坐按下处理
                if (_ALGUIMouseDown(ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN), ALInputControl.instance.btnDownEvent))
                {
                    //档鼠标点击事件被捕获则阻断鼠标消息的传递处理
                    interruptMes = true;
                }
                else
                {
                    //此时设置鼠标的焦点窗口对象
                    _ALGUISetFocusWnd(null);
                }
            }

            //判断是否有按键弹起
            if (EALGUIOpButtonType.NONE != ALInputControl.instance.btnUpEvent && ALGUIGetMouseBtnPressStat(ALInputControl.instance.btnUpEvent))
            {
                //当有鼠标按键按下时触发相关操作时，根据鼠标当前位置坐按下处理
                if (_ALGUIMouseUp(ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN), ALInputControl.instance.btnUpEvent))
                {
                    //档鼠标点击事件被捕获则阻断鼠标消息的传递处理
                    interruptMes = true;
                }

                //清除记录的鼠标操作状态
                _ALGUIRemoveMouseDownMes(ALInputControl.instance.btnUpEvent);
            }

            return interruptMes;
        }

        /*************************
         * 拖拽信息时，释放鼠标操作，将信息丢弃到当前鼠标所在位置对象中
         *         
         *************************/
        public bool ALGUIDropDragItem(_AALGUIDragItemInfo _dragItem)
        {
            bool interruptMes = false;
            //获取当前鼠标所在窗口位置
            ALGUIBaseWnd curWnd = ALGUIHitTestStage(ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN));

            //处理原先移入窗口的移出操作
            if (null != _m_wPreDragMouseWnd)
            {
                //当之前窗口不为空时，需要执行鼠标移出操作
                _m_wPreDragMouseWnd._ALGUIDragMouseOut(_dragItem);
            }

            //重置鼠标对应窗口对象
            _m_wPreDragMouseWnd = null;
            _m_wCurDragMouseWnd = null;

            //优先处理子窗口的接收函数
            if (null != curWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标移动处理
                interruptMes = curWnd._ALGUIDropDragItem(_dragItem, curWnd.ALGUIGetParentMousePositionInWnd(ALInputControl.instance.getBtnScreenPos(EALGUIOpButtonType.OP_BTN)));
            }

            //如无人接收对象，则调用拖拽信息中的无处理函数
            if (null != _dragItem && !interruptMes)
                _dragItem._onDropNoReceiver();

            return interruptMes;
        }

        /**************************
         * GUI部分绘制函数
         **************************/
        public void ALGUIPain()
        {
            //循环绘制所有非锁定顶层窗口
            foreach (_AALBaseGuiStage wnd in _m_lStageList)
            {
                if (wnd.alwaysTop)
                    continue;

                if (!wnd.visible)
                    continue;

                wnd._ALGUIPain();
            }

            //循环绘制所有锁定顶层窗口
            foreach (_AALBaseGuiStage wnd in _m_lStageList)
            {
                if (!wnd.alwaysTop)
                    continue;

                if (!wnd.visible)
                    continue;

                wnd._ALGUIPain();
            }
        }

        /*************************
         * 尝试设置某一窗口为鼠标独占窗口
         *************************/
        public bool ALGUISetCapture(ALGUIBaseWnd _wnd)
        {
            if (null == _wnd)
                return false;

            if (_m_bMouseCaptureAble && _m_wCaptureWnd != _wnd)
                return false;

            _m_wCaptureWnd = _wnd;
            _m_bMouseCaptureAble = true;
            return true;
        }

        /*************************
         * 尝试释放某一窗口对鼠标的独占
         *************************/
        public bool ALGUIReleaseCapture(ALGUIBaseWnd _wnd)
        {
            if (!_m_bMouseCaptureAble)
                return true;

            if (_m_wCaptureWnd != _wnd)
                return false;

            _m_bMouseCaptureAble = false;
            _m_wCaptureWnd = null;
            return true;
        }

        /*******************************
         * 获取鼠标按键是否按下的状态
         *******************************/
        public bool ALGUIGetMouseBtnPressStat(EALGUIOpButtonType _btnType)
        {
            if ((opBtnPressState & (int)_btnType) != 0)
            {
                return true;
            }

            return false;
        }

        /**********************
         * 注册一个GUI场景，带入场景的深度，越深显示的层次就越后，在队列的位置就越前
         **********************/
        public bool ALGUIRegStage(_AALBaseGuiStage _stage)
        {
            //判断是否重复注册
            if (_m_lStageList.Contains(_stage))
            {
                return false;
            }

            _stage.parentWnd = null;

            int i = 0;
            for (i = 0; i < _m_lStageList.Count; i++)
            {
                if (_m_lStageList[i].deep < _stage.deep)
                    break;
            }
            //在对应位置插入场景
            _m_lStageList.Insert(i, _stage);

            //刷新窗口位置相关信息
            _stage.ALGUIResetWndPos();

            return true;
        }

        /**********************
         * 注销一个显示的窗口
         **********************/
        public void ALGUIUnregWnd(ALGUIBaseWnd _wnd)
        {
            if (null == _wnd.parentWnd && _wnd is _AALBaseGuiStage)
                _m_lStageList.Remove((_AALBaseGuiStage)_wnd);
            else
                _wnd.parentWnd.ALGUIUnregChildWnd(_wnd);

            _wnd.parentWnd = null;
        }

        /*********************
         * 根据鼠标在屏幕位置，检测在GUI场景中的哪个窗口
         *********************/
        public _AALBaseGuiStage ALGUIHitTestStage(Vector2 _pos)
        {
            //倒序计算，因为顺序最后的显示次序最高，先对always top的窗口处理
            for (int i = _m_lStageList.Count - 1; i >= 0; i--)
            {
                _AALBaseGuiStage stage = _m_lStageList[i];

                if (!stage.alwaysTop)
                    continue;

                //窗口无法显示则直接过滤
                if (!stage.visible || !stage.isEnable)
                    continue;

                //当坐标在该窗口内则直接返回该窗口
                if (null != stage.ALGUIHitTestWnd(_pos))
                    return stage;
            }

            //对非always top的窗口处理
            for (int i = _m_lStageList.Count - 1; i >= 0; i--)
            {
                _AALBaseGuiStage stage = _m_lStageList[i];

                if (stage.alwaysTop)
                    continue;

                //窗口无法显示则直接过滤
                if (!stage.visible || !stage.isEnable)
                    continue;

                //当坐标在该窗口内则直接返回该窗口
                if (null != stage.ALGUIHitTestWnd(_pos))
                    return stage;
            }

            return null;
        }

        /*************************
         * 鼠标移动操作
         * 返回值表示是否允许在此中断鼠标消息，不传入3D模块中
         *         
         *************************/
        protected bool _ALGUIMouseMove(Vector2 _moveVector, Vector2 _pos)
        {
            //当有窗口独占鼠标时单独进行处理
            if (_m_bMouseCaptureAble)
            {
                //独占模式下才判断是否有窗口捕获，如窗口为null则直接不进行处理
                if (null != _m_wCaptureWnd)
                {
                    _m_wCurMouseWnd = _m_wCaptureWnd.getTopWnd();

                    //计算在该窗口中的当前鼠标坐标
                    Vector2 posInWnd = _m_wCaptureWnd.ALGUIGetMousePositionInWnd(_pos);
                    //执行该窗口的鼠标移动操作
                    _m_wCaptureWnd._ALGUIMouseMove(_moveVector, posInWnd);

                    _m_wPreMouseWnd = _m_wCurMouseWnd;

                    //直接返回不对最外层窗口相关变量进行变更
                    //此时不将鼠标消息传递到3D模块中
                    return true;
                }
                else
                {
                    //返回false 不进行处理
                    _m_wCurMouseWnd = null;
                    _m_wPreMouseWnd = null;
                    return false;
                }
            }

            //本层初始不拦截鼠标事件
            bool interruptMes = false;

            //获取当前鼠标所在窗口位置
            _m_wCurMouseWnd = ALGUIHitTestStage(_pos);

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

            if (null != _m_wCurMouseWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标移动处理
                interruptMes = _m_wCurMouseWnd._ALGUIMouseMove(_moveVector, _m_wCurMouseWnd.ALGUIGetParentMousePositionInWnd(_pos));
            }

            //设置之前窗口对象
            _m_wPreMouseWnd = _m_wCurMouseWnd;

            return interruptMes;
        }

        /********************
         * 在鼠标按下时执行的相关函数
         ********************/
        protected bool _ALGUIMouseDown(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            //当有窗口独占鼠标时单独进行处理
            if (_m_bMouseCaptureAble)
            {
                //独占模式下才判断是否有窗口捕获，如窗口为null则直接不进行处理
                if (null != _m_wCaptureWnd)
                {
                    //计算在该窗口中的当前鼠标坐标
                    Vector2 posInWnd = _m_wCaptureWnd.ALGUIGetMousePositionInWnd(_pos);
                    //执行该窗口的鼠标移动操作
                    return _m_wCaptureWnd._ALGUIMouseDown(posInWnd, _btnType);
                }
            }

            if (null != _m_wCurMouseWnd)
            {
                //计算在该窗口中的当前鼠标坐标
                Vector2 posInWnd = _m_wCurMouseWnd.ALGUIGetMousePositionInWnd(_pos);
                //执行该窗口的鼠标移动操作
                return _m_wCurMouseWnd._ALGUIMouseDown(posInWnd, _btnType);
            }
            else
            {
                return false;
            }
        }

        /********************
         * 在鼠标弹起时执行的相关函数
         ********************/
        protected bool _ALGUIMouseUp(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            //判断鼠标按下状态对象是否存在此鼠标按键，此时直接返回不进行处理
            if ((opBtnPressState & (int)_btnType) == 0)
                return false;

            //当有窗口独占鼠标时单独进行处理
            if (_m_bMouseCaptureAble)
            {
                //独占模式下才判断是否有窗口捕获，如窗口为null则直接不进行处理
                if (null != _m_wCaptureWnd)
                {
                    //计算在该窗口中的当前鼠标坐标
                    Vector2 posInWnd = _m_wCaptureWnd.ALGUIGetMousePositionInWnd(_pos);
                    //执行该窗口的鼠标移动操作
                    return _m_wCaptureWnd._ALGUIMouseUp(posInWnd, _btnType);
                }
            }

            //由于独占模式在Move处理中就进行了处理，因此这里只是将之前的结果拿来处理
            if (null != _m_wCurMouseWnd)
            {
                //计算在该窗口中的当前鼠标坐标
                Vector2 posInWnd = _m_wCurMouseWnd.ALGUIGetMousePositionInWnd(_pos);
                //执行该窗口的鼠标移动操作
                return _m_wCurMouseWnd._ALGUIMouseUp(posInWnd, _btnType);
            }
            else
            {
                return false;
            }
        }

        /*************************
         * 拖拽信息时，鼠标移动操作
         * 返回值表示是否允许在此中断鼠标消息，不传入3D模块中
         *         
         *************************/
        protected void _ALGUIDragMouseMove(_AALGUIDragItemInfo _dragItem, Vector2 _moveVector, Vector2 _pos)
        {
            //获取当前鼠标所在窗口位置
            _m_wCurDragMouseWnd = ALGUIHitTestStage(_pos);

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

            if (null != _m_wCurDragMouseWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标移动处理
                _m_wCurDragMouseWnd._ALGUIDragMouseMove(_dragItem, _moveVector, _m_wCurDragMouseWnd.ALGUIGetParentMousePositionInWnd(_pos));
            }

            //设置之前窗口对象
            _m_wPreDragMouseWnd = _m_wCurDragMouseWnd;
        }

        /**********************
         * 触发窗口单击操作，记录单击对象，进行双击处理
         **********************/
        protected internal void _ALGUIWndMouseClick(ALGUIBaseWnd _clickWnd, Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            long nowTime = System.DateTime.Now.Ticks;
            //获取最后一次单击时间
            long lastClickTime = _ALGUIPopLastClickTime(_btnType);
            //获取最后一次单击的窗口对象
            ALGUIBaseWnd lastClickWnd = _ALGUIPopLastClickWnd(_btnType);

            if (null == _clickWnd)
                return;

            if (0 == lastClickTime || null == lastClickWnd      //单击属性无效
                || lastClickWnd != _clickWnd                    //或之前单击窗口并非本单击窗口
                || Math.Abs(nowTime - lastClickTime) > 5000000)     //或者单击事件间隔超过500毫秒
            {
                //直接进行单击处理
                _clickWnd._ALGUIOnMouseClick(_pos, _btnType);

                //设置单击对象信息
                _ALGUISetLastClickWnd(_btnType, _clickWnd);
                _ALGUISetLastClickTime(_btnType, nowTime);
            }
            else if (lastClickWnd == _clickWnd)
            {
                //当点击窗口与当前点击窗口一致时，表明需要进行双击处理
                //当双击处理被接受时表明不需要保存单击信息，避免连续的双击处理
                if (!_clickWnd._ALGUIOnMouseDoubleClick(_pos, _btnType))
                {
                    //当未对双击处理进行处理时，直接继续单击处理
                    _clickWnd._ALGUIOnMouseClick(_pos, _btnType);

                    //设置单击对象信息
                    _ALGUISetLastClickWnd(_btnType, _clickWnd);
                    _ALGUISetLastClickTime(_btnType, nowTime);
                }
            }
        }

        /*******************
         * 设置鼠标操作对象以及操作窗口属性
         *******************/
        protected internal void _ALGUISetMouseDownWnd(EALGUIOpButtonType _btnType, ALGUIBaseWnd _wnd)
        {
            //判断是否出错
            if (null != _m_dMousePressWnd[(int)_btnType])
            {
                UnityEngine.Debug.LogError(_btnType.ToString() + " Mouse Button Press Info Set Error!");
                //出错情况下，需要移出出错数据
                _m_dMousePressWnd[(int)_btnType] = null;
            }

            //设置鼠标按下对象窗口
            _m_dMousePressWnd[(int)_btnType] = _wnd;
            //设置鼠标按下类型
            opBtnPressState = opBtnPressState | (int)_btnType;

            //此时设置鼠标的焦点窗口对象
            _ALGUISetFocusWnd(_wnd);
        }

        /*******************
         * 设置鼠标操作对象以及操作窗口属性
         *******************/
        protected internal void _ALGUIRemoveMouseDownMes(EALGUIOpButtonType _btnType)
        {
            //判断是否出错
            if (null != _m_dMousePressWnd[(int)_btnType])
            {
                //当包含对象时，移除对应的窗口对象
                _m_dMousePressWnd[(int)_btnType] = null;
            }

            //设置鼠标按下类型
            opBtnPressState = opBtnPressState & ~(int)_btnType;
        }

        /***********************
         * 获取鼠标操作须向
         ***********************/
        protected internal ALGUIBaseWnd _ALGUIGetMousePressWnd(EALGUIOpButtonType _btnType)
        {
            return _m_dMousePressWnd[(int)_btnType];
        }

        /***********************
         * 获取鼠标某一按钮最后一次单击的窗口对象
         ***********************/
        protected ALGUIBaseWnd _ALGUIPopLastClickWnd(EALGUIOpButtonType _btnType)
        {
            //获取最后一次点击的窗口
            ALGUIBaseWnd lastClickWnd = _m_dLastMouseClickWnd[(int)_btnType];

            if (null != lastClickWnd)
            {
                //在存储信息里删除最后一次点击的窗口信息
                _m_dLastMouseClickWnd[(int)_btnType] = null;

                return lastClickWnd;
            }

            return null;
        }

        /***********************
         * 获取鼠标某一按钮最后一次单击窗口的时间
         ***********************/
        protected long _ALGUIPopLastClickTime(EALGUIOpButtonType _btnType)
        {
            //获取最后一次点击的时间
            long lastClickTime = _m_dLastMouseClickTime[(int)_btnType];

            if (0 != lastClickTime)
            {
                //在存储信息里删除最后一次点击的时间信息
                _m_dLastMouseClickTime[(int)_btnType] = 0;

                return lastClickTime;
            }

            return 0;
        }

        /***********************
         * 设置鼠标某一按钮最后一次单击的窗口对象
         ***********************/
        protected void _ALGUISetLastClickWnd(EALGUIOpButtonType _btnType, ALGUIBaseWnd _clickWnd)
        {
            if (null != _m_dLastMouseClickWnd[(int)_btnType])
            {
                UnityEngine.Debug.LogError(_btnType.ToString() + " Mouse Button Last Click Wnd Set Error!");
                //在存储信息里删除最后一次点击的窗口信息
                _m_dLastMouseClickWnd[(int)_btnType] = null;
            }

            _m_dLastMouseClickWnd[(int)_btnType] = _clickWnd;
        }

        /***********************
         * 设置鼠标某一按钮最后一次单击窗口的时间
         ***********************/
        protected void _ALGUISetLastClickTime(EALGUIOpButtonType _btnType, long _clickTime)
        {
            if (0 != _m_dLastMouseClickTime[(int)_btnType])
            {
                UnityEngine.Debug.LogError(_btnType.ToString() + " Mouse Button Last Click Time Set Error!");
                //在存储信息里删除最后一次点击的时间信息
                _m_dLastMouseClickTime[(int)_btnType] = 0;
            }

            _m_dLastMouseClickTime[(int)_btnType] = _clickTime;
        }

        /*********************
         * 设置焦点窗口对象
         **/
        protected internal void _ALGUISetFocusWnd(ALGUIBaseWnd _wnd)
        {
            if (null != _m_wCurFocusWnd)
            {
                _m_wCurFocusWnd._unfocus();
            }

            _m_wCurFocusWnd = _wnd;
            if (null != _m_wCurFocusWnd)
                _m_wCurFocusWnd._focus();
        }

        /*******************
         * 默认的窗口大小更改函数
         * 返回false则交由父窗口处理
         *******************/
        private void _ALGUIMainDefalutOnSize(Rect _oldRect, Rect _newRect)
        {
            //遍历每个子窗体，根据子窗体的不同风格，设置子窗体新的位置
            foreach (_AALBaseGuiStage stage in _m_lStageList)
            {
                //仅档子窗体的位置样式有效才进行计算
                if (null == stage)
                    continue;

                //通知GUI场景需要重新计算窗口位置
                stage.ALGUIResetWndPos();
            }
        }

        public int width
        {
            get
            {
                return _m_iPreWndWidth;
            }
        }
        public int height
        {
            get
            {
                return _m_iPreWndHeight;
            }
        }

        public ALGUIBaseWnd focusWnd
        {
            get
            {
                return _m_wCurFocusWnd;
            }
        }
    }
}

#else

namespace ALPackage
{
    public class ALGUIMain
    {
        private static ALGUIMain g_instance = new ALGUIMain();

        public static ALGUIMain instance
        {
            get
            {
                if(null == g_instance)
                    g_instance = new ALGUIMain();

                return g_instance;
            }
        }

        /** 上帧的窗口大小 */
        public int _m_iPreWndWidth;
        public int _m_iPreWndHeight;
        
        /** 屏幕尺寸变动回调 */
        protected Action<int, int> _m_dScreenSizeChgDelegate;

        public Action<int, int> screenSizeChgDelegate { get { return _m_dScreenSizeChgDelegate; } set { _m_dScreenSizeChgDelegate = value; } }

        /***********
         * 构造函数不对外开放
         ***********/
        protected ALGUIMain()
        {
            _m_iPreWndWidth = 0;
            _m_iPreWndHeight = 0;

            _m_dScreenSizeChgDelegate = default(Action<int, int>);
        }

        /************************
         * 执行主GUI任务的函数
         * 此处不做原生GUI处理，所以没有返回值。进行基础的分辨率监控以及每帧的输入处理
         *         
         ************************/
        public void ALGUIExec()
        {
            //判断窗口大小
            int curWndWidth = Screen.width;
            int curWndHeight = Screen.height;

            if(curWndWidth != _m_iPreWndWidth || curWndHeight != _m_iPreWndHeight)
            {
                if(null != _m_dScreenSizeChgDelegate)
                    _m_dScreenSizeChgDelegate(curWndWidth, curWndHeight);

                //重新设置之前窗口尺寸
                _m_iPreWndWidth = curWndWidth;
                _m_iPreWndHeight = curWndHeight;
            }

            //使用输入对象进行检测
            ALInputControl.instance.frameCheck();
        }
    }
}

#endif
