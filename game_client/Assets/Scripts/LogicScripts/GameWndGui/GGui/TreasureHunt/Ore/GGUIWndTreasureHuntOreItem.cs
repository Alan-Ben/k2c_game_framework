using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石Item
    /// </summary>
    public class GGUIWndTreasureHuntOreItem : _ANPGGUIBasicGridItemWnd<GGUIMonoTreasureHuntOreItem>
    {
        private _ITreasureHuntOreInfo _m_iOreInfo;
        
        private GGUIWndTreasureHuntOreInfo _m_wOreInfo;
        private NPGGUIWndCommonRedTip _m_wRedTip;
        
        public GGUIWndTreasureHuntOreItem(GGUIMonoTreasureHuntOreItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        public event Action<_ITreasureHuntOreInfo> onOreClick; // 当矿石被点击 

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.oreInfoMono != null)
            {
                _m_wOreInfo = new GGUIWndTreasureHuntOreInfo(wnd.oreInfoMono);
                _m_wOreInfo.onOreClick += _onClickOre;
            }
            
            if(wnd.monoRedTip != null)
                _m_wRedTip = new NPGGUIWndCommonRedTip(wnd.monoRedTip);
        }
        
        protected override void _onDiscard()
        {
            onOreClick = null;

            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.onOreClick -= _onClickOre;
                _m_wOreInfo.discard();
                _m_wOreInfo = null;
            }
            
            _m_wRedTip?.discard();
            _m_wRedTip = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wRedTip?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreInfo?.hideWnd();
            _m_wRedTip?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreInfo?.resetWnd();
            _m_wRedTip?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wOreInfo?.resetWnd();
            _m_wRedTip?.resetWnd();
        }

        public void setData(_ITreasureHuntOreInfo _oreInfo)
        {
            _m_iOreInfo = _oreInfo;
            
            _refreshWnd();
        }

        public void setShowRedTip(bool _show)
        {
            if (_m_wRedTip != null)
            {
                _m_wRedTip.showWnd();
                _m_wRedTip?.showRedTipNum(_show ? 1 : 0);
            }
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_iOreInfo == null)
                return;

            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.showWnd();
                _m_wOreInfo.setData(_m_iOreInfo);
            }
        }
        
        private void _onClickOre(_ITreasureHuntOreInfo _oreInfo)
        {
            onOreClick?.Invoke(_oreInfo);
        }
    }
}