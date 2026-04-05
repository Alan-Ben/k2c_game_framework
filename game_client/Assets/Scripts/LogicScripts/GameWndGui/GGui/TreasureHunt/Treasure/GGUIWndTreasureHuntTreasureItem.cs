using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物Item
    /// </summary>
    public class GGUIWndTreasureHuntTreasureItem : _ANPGGUIBasicGridItemWnd<GGUIMonoTreasureHuntTreasureItem>
    {
        private _ITreasureHuntTreasureInfo _m_iTreasureInfo;
        
        private GGUIWndTreasureHuntTreasureInfo _m_wTreasureInfo;
        private NPGGUIWndCommonRedTip _m_wRedTip;
        
        public GGUIWndTreasureHuntTreasureItem(GGUIMonoTreasureHuntTreasureItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        public event Action<_ITreasureHuntTreasureInfo> onTreasureClick; // 当奇物被点击 

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.treasureInfoMono != null)
            {
                _m_wTreasureInfo = new GGUIWndTreasureHuntTreasureInfo(wnd.treasureInfoMono);
                _m_wTreasureInfo.onTreasureClick += _onClickTreasure;
            }
            
            if(wnd.monoRedTip != null)
                _m_wRedTip = new NPGGUIWndCommonRedTip(wnd.monoRedTip);
        }
        
        protected override void _onDiscard()
        {
            onTreasureClick = null;

            if (_m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.onTreasureClick -= _onClickTreasure;
                _m_wTreasureInfo.discard();
                _m_wTreasureInfo = null;
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
            _m_wTreasureInfo?.hideWnd();
            _m_wRedTip?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureInfo?.resetWnd();
            _m_wRedTip?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wTreasureInfo?.resetWnd();
            _m_wRedTip?.resetWnd();
        }

        public void setData(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            _m_iTreasureInfo = _treasureInfo;
            
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
            if(wnd == null || _m_iTreasureInfo == null)
                return;

            if (_m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.showWnd();
                _m_wTreasureInfo.setData(_m_iTreasureInfo);
            }
        }
        
        private void _onClickTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            onTreasureClick?.Invoke(_treasureInfo);
        }
    }
}