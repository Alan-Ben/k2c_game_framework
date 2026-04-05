using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 事件结果弹窗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AGGUIWndEventResult<T> : _ANPGGUIBasicWnd<T> where T : _AGGUIMonoEventResult
    {
        protected _IEventResultShowInfo _m_iEventResultShowInfo;//事件结果显示信息
        
        protected _AGGUIWndEventResult(EALUIWndLayer _layer) : base(_layer)
        {
        }
        
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _closeBtnClick);
            
            _onInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _closeBtnClick);
            }
            
            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _onResetSub();
        }

        public virtual void setData(_IEventResultShowInfo _resultShowInfo)
        {
            _m_iEventResultShowInfo = _resultShowInfo;
            _onSetEventData();
            
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        protected virtual void _refreshWnd()
        {
            _refreshWndSub();
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
        protected abstract void _onSetEventData();
        
        /// <summary>
        /// refreshWnd时子类调用
        /// </summary>
        protected abstract void _refreshWndSub();

        /// <summary>
        /// 关闭窗口按钮被点击调用
        /// </summary>
        protected abstract void _closeBtnClick(GameObject _go);

        #endregion
    }
}