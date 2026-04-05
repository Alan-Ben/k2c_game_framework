using System;
using ALPackage;
using Common.EventObj;

namespace GOE
{
    /// <summary>
    /// 通用事件处理窗口抽象类
    /// </summary>
    public abstract class _AGGUIWndCommonSimpleEvent<T> : _ANPGGUIBasicWnd<T> where T : _AGGUIMonoCommonSimpleEvent
    {
        protected _ACommonSimpleEventAgent _m_ACommonSimpleEventAgent;//事件处理代理 
        protected Action _m_aOnDealDone;//事件处理完成回调

        public _AGGUIWndCommonSimpleEvent(EALUIWndLayer _layer) : base(_layer)
        {
        }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {

            _onInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            _onDiscardSub();

            _m_aOnDealDone = null;
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _onHideWndSub();
            
            _m_aOnDealDone = null;
        }

        protected override void _onReset()
        {
            _onResetSub();
            
            _m_aOnDealDone = null;
        }

        public void setEventAgent(_ACommonSimpleEventAgent _commonSimpleEventInfo, Action _onDealDone)
        {
            if (_commonSimpleEventInfo == null)
            {
                Debug.LogError("[_AGGUIWndCommonSimpleEvent setEventAgent] 传入参数_commonSimpleEventInfo == null");
                return;
            }

            _m_ACommonSimpleEventAgent = _commonSimpleEventInfo;
            _m_aOnDealDone = _onDealDone;
            
            _onSetEventData(_m_ACommonSimpleEventAgent);
            
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || _m_ACommonSimpleEventAgent == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtEventName, TextTranslate.instance.getLanguage(_m_ACommonSimpleEventAgent.eventName));
            ALUGUICommon.setLabelTxt(wnd.txtEventDetailDesc, TextTranslate.instance.getLanguage(_m_ACommonSimpleEventAgent.eventDetailDesc));

            _refreshWndSub();
        }

        /// <summary>
        /// 触发处理事件完成
        /// </summary>
        protected virtual void _triggerEventDealDone()
        {
            Action _dealDone = _m_aOnDealDone;
            _m_aOnDealDone = null;
            
            _dealDone?.Invoke();
        }

        #region 子类接口

        /// <summary>
        /// _onWndInitDone时子类调用
        /// </summary>
        protected abstract void _onInitDoneSub();

        /// <summary>
        /// _onDiscard时子类调用
        /// </summary>
        protected abstract void _onDiscardSub();
        
        /// <summary>
        /// _onShowWnd时子类调用
        /// </summary>
        protected abstract void _onShowWndSub();

        /// <summary>
        /// _onHideWnd时子类调用
        /// </summary>
        protected abstract void _onHideWndSub();

        /// <summary>
        /// _onReset时子类调用
        /// </summary>
        protected abstract void _onResetSub();

        /// <summary>
        /// setEventInfo时子类调用
        /// </summary>
        protected abstract void _onSetEventData(_ACommonSimpleEventAgent _commonEventAgent);
        
        /// <summary>
        /// refreshWnd时子类调用
        /// </summary>
        protected abstract void _refreshWndSub();

        /// <summary>
        /// 执行CloseNode的真正语句, 因为不同事件打开窗口不同, 所以具体退出操作由子类实现, 父类只请求发生完成事件协议
        /// </summary>
        protected abstract void _doCloseNode();

        #endregion
    }
}