using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地事件Buff项窗口
    /// </summary>
    public class GGUIWndMarsEventBuffItem : _ANPGGUIBasicSubWnd<GGUIMonoMarsEventBuffItem>
    {
        private _IMarsEventInfo _m_iEventInfo;
        
        private GGUIWndPlayerBuffItem _m_wBuffItem;
        
        public GGUIWndMarsEventBuffItem(GGUIMonoMarsEventBuffItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化buff项窗口
            if (wnd.buffItem != null)
                _m_wBuffItem = new GGUIWndPlayerBuffItem(wnd.buffItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onDiscard()
        {
            _m_iEventInfo = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
            }
            
            // 销毁buff项窗口
            _m_wBuffItem?.discard();
            _m_wBuffItem = null;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            // 隐藏子窗口
            _m_wBuffItem?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wBuffItem?.resetWnd();
        }
        
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_eventInfo">事件信息</param>
        public void setData(_IMarsEventInfo _eventInfo)
        {
            _m_iEventInfo = _eventInfo;
            refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iEventInfo == null)
                return;

            // 设置buff数据
            if (_m_wBuffItem != null)
            {
                _m_wBuffItem.showWnd();
                _m_wBuffItem.setData(_m_iEventInfo.buffInfo);
            }
        }
        
        private void _onClick(GameObject _go)
        {
            if(_m_iEventInfo == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsEventDetail.instance, () =>
            {
                GGUIWndMarsEventDetail.instance.setData(_m_iEventInfo);
                GGUIWndMarsEventDetail.instance.showWnd();
            }, UINodeTagConst.C_MARS_EVENT_DETAIL);
        }
    }
}