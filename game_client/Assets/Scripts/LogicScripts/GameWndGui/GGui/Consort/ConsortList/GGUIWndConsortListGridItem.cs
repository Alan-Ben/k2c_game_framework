using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子列表item
    /// </summary>
    public class GGUIWndConsortListGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoConsortListGridItem>
    {
        private _IConsortShowInfo _m_iConsortShowInfo;
        
        private GGUIWndConsortCardItem _m_wConsortCardItemWnd;
        private GGUIWndConsortSystemRedTip _m_wRedTip;
        private Vector2 _m_vInitContentAnchoredPos;
        
        public GGUIWndConsortListGridItem(GGUIMonoConsortListGridItem _wnd) : base(_wnd)
        {
        }

        public _IConsortShowInfo consortShowInfo { get { return _m_iConsortShowInfo; } }
        public event Action<GGUIWndConsortListGridItem> clickDelegate;
        public Vector3 consortHeadIconCenterWorldPos { get { return wnd == null || wnd.consortIconCenterTransform == null ? Vector3.zero : wnd.consortIconCenterTransform.position;} }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoCardItem != null)
                _m_wConsortCardItemWnd = new GGUIWndConsortCardItem(wnd.monoCardItem, _onClickItem);

            if (wnd.consortShowContentParent != null)
                _m_vInitContentAnchoredPos = wnd.consortShowContentParent.anchoredPosition;
            else
                _m_vInitContentAnchoredPos = Vector2.zero;
            
            if(wnd.redTipMono != null)
                _m_wRedTip = new GGUIWndConsortSystemRedTip(wnd.redTipMono);
        }
        
        protected override void _onDiscard()
        {
            clickDelegate = null;
            
            _m_wConsortCardItemWnd?.discard();
            _m_wConsortCardItemWnd = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wConsortCardItemWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortCardItemWnd?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wConsortCardItemWnd?.resetWnd();
        }

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_consortShowInfo"></param>
        private void _onClickItem(_IConsortShowInfo _consortShowInfo)
        {
            if(_consortShowInfo == null || _m_iConsortShowInfo == null || _consortShowInfo.consortId != _m_iConsortShowInfo.consortId)
                return;
            
            clickDelegate?.Invoke(this);
        }

        public void setData(_IConsortShowInfo _consortShowInfo)
        {
            _m_iConsortShowInfo = _consortShowInfo;

            _refreshWnd();
        }
        
        /// <summary>
        /// 设置是否被邀约
        /// </summary>
        /// <param name="_byInvite"></param>
        public void setByInvite(bool _byInvite, Action _complete = null)
        {
            if (wnd == null || wnd.byInviteConsortItemAnim == null || string.IsNullOrEmpty(wnd.byInviteConsortItemAnimName))
            {
                _complete?.Invoke();
                return;
            }

            if (_byInvite)
            {
                wnd.byInviteConsortItemAnim.Play(wnd.byInviteConsortItemAnimName, _complete);
            }
            else
            {
                wnd.byInviteConsortItemAnim.Sample(wnd.byInviteConsortItemAnimName, 0f);
                _complete?.Invoke();
            }
        }

        private void _refreshWnd()
        {
            if(_m_iConsortShowInfo == null || wnd == null)
                return;

            if (_m_wConsortCardItemWnd != null)
            {
                _m_wConsortCardItemWnd.showWnd();
                _m_wConsortCardItemWnd.setInfo(_m_iConsortShowInfo);
            }

            if (_m_wRedTip != null)
            {
                _m_wRedTip.showWnd();
                _m_wRedTip.setConsortId(_m_iConsortShowInfo.consortId);
            }
        }

        public void setContentPositionOffset(Vector2 _offset)
        {
            if(wnd == null || wnd.consortShowContentParent == null)
                return;

            wnd.consortShowContentParent.anchoredPosition = _m_vInitContentAnchoredPos + _offset;
        }
    }
}