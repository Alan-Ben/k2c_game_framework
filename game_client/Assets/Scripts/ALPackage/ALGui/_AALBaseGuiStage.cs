using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    /***********************
     * GUI框架下的UI场景管理对象
     **/
    public abstract class _AALBaseGuiStage : ALGUIBaseWnd
    {
        /** 场景深度 */
        private int _m_iDeep;
        /** 是否进行了初始化 */
        private bool _m_bInit;
        /** 存储所有本场景下的UI窗口对象映射 */
        private Dictionary<object, ALGUIBaseWnd> _m_dStageWndDic;
        /** 本场景是否有效 */
        private bool _m_bStageEnable;

        public _AALBaseGuiStage(int _deep)
            : base()
        {
            _m_iDeep = _deep;
            _m_bInit = false;
            _m_dStageWndDic = new Dictionary<object, ALGUIBaseWnd>();
            _m_bStageEnable = false;
        }

        public int deep
        {
            get
            {
                return _m_iDeep;
            }
        }

        public bool stageEnable
        {
            get
            {
                return _m_bStageEnable;
            }
        }

        /**************
         * 退出本场景
         **/
        public void quitStage()
        {
            //离开场景意味着将场景从主GUI中注销
            ALGUIMain.instance.ALGUIUnregWnd(this);

            //设置场景无效
            _m_bStageEnable = false;

            //调用事件函数
            _onQuitStage();
        }

        /******************
         * 进入本场景的抽象接口
         **/
        public void enterStage()
        {
            if (!_m_bInit)
            {
                _m_bInit = true;
                _init();
            }

            //进入场景意味着将场景注册到主GUI中
            if (!ALGUIMain.instance.ALGUIRegStage(this))
                return;

            //设置场景有效
            _m_bStageEnable = true;

            //处理进入场景的函数
            _onEnterStage();
        }

        /*****************
         * 向场景中注册一个显示窗口
         **/
        public void regWnd(object _idObj, ALGUIBaseWnd _wnd)
        {
            //注册到映射表中
            _m_dStageWndDic.Add(_idObj, _wnd);

            //将窗口注册为本场景的子窗口
            ALGUIRegChildWnd(_wnd);
        }

        /*****************
         * 获取一个场景中的窗口
         **/
        public ALGUIBaseWnd getWnd(object _idObj)
        {
            if (!_m_dStageWndDic.ContainsKey(_idObj))
                return null;

            return _m_dStageWndDic[_idObj];
        }

        /******************
         * 释放所有的窗口对象
         **/
        public void releaseAllWnd()
        {
            /****************
             * 将所有窗口反注册掉
             **/
            foreach (ALGUIBaseWnd wnd in _m_dStageWndDic.Values)
            {
                //注销窗口对象
                wnd.ALGUIUnreg();

                //释放窗口对象
                wnd.ALGUIRelease();
            }

            //清空映射表
            _m_dStageWndDic.Clear();

            //释放本窗口对象
            ALGUIUnreg();
            ALGUIRelease();
        }

        /*******************
         * 重写获取窗口新位置的计算方式
         *******************/
        public override void ALGUIResetWndPos()
        {
            //设置窗口新尺寸
            _ALGUISetWndSize(new Rect(0, 0, Screen.width, Screen.height));
        }

        /*******************
         * 由于Stage是与窗口等大，因此在绘制函数中不需要使用GUI Group因此重写内部绘制函数
         *******************/
        protected internal override void _ALGUIPain()
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
         * 重写基类中的鼠标点击响应函数，在本窗口的子窗口中可通过点击消息调整窗口前后位置
         *******************************/
        protected internal override bool _ALGUIMouseDown(Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            //是否将鼠标信号中断在此，默认中断，即在GUI上的鼠标消息不穿透到3D模块中
            bool interruptMes = false;

            if (null != _m_wCurMouseWnd)
            {
                //将当前坐标转换为窗口内坐标进行鼠标按下处理
                interruptMes = _m_wCurMouseWnd._ALGUIMouseDown(_m_wCurMouseWnd.ALGUIGetParentMousePositionInWnd(_pos), _btnType);

                //当窗口进行了消息处理则将窗口放置到顶层
                if (interruptMes)
                    ALGUIBringTop(_m_wCurMouseWnd);
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

        /*************************
         * 将某一窗口的顶层窗口抬到GUI最顶层
         *************************/
        public void ALGUIBringTop(ALGUIBaseWnd _wnd)
        {
            if (null == _wnd)
                return;

            //获取最顶层窗口
            ALGUIBaseWnd topWnd = _wnd.getTopWnd(this);

            //从队列中移出该窗体
            ALGUIBaseWnd wnd = _ALGUIPopWnd(topWnd);
            //将窗体插入最后一位（即是显示在最前台的窗体）
            _ALGUIPushWnd(wnd);
        }

        /****************
         * 初始化本场景的函数
         **/
        protected abstract void _init();

        /****************
         * 在进入本场景时调用的事件函数
         **/
        protected abstract void _onEnterStage();

        /****************
         * 在退出本场景时调用的事件函数
         **/
        protected abstract void _onQuitStage();
    }
}

#endif
