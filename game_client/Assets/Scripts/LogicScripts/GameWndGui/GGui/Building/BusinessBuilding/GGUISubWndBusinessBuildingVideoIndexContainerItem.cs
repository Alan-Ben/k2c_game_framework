using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndBusinessBuildingVideoIndexContainerItem : _ATALBasicUISubWnd<GGUIMonoBusinessBuildingVideoIndexContainerItem>
    {
        private readonly Action<int> _m_onItemClick;
        private int _m_index;
        private bool _m_enable;

        public GGUISubWndBusinessBuildingVideoIndexContainerItem(GGUIMonoBusinessBuildingVideoIndexContainerItem _wnd, Action<int> _onItemClick) 
            : base(_wnd)
        {
            _m_onItemClick = _onItemClick;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }

        
        public void refreshWnd(int _index, bool _enable)
        {
            _m_index = _index;
            _m_enable = _enable;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            wnd.setEnable(_m_enable);
        }
        
        
        private void _onBtnClick(GameObject _)
        {
            _m_onItemClick?.Invoke(_m_index);
        }
    }
}