using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class GGUIWndRecruitShopCardItemHero : _AGGUIWndRecruitShopCardItem<GGUIMonoRecruitShopCardItemHero, RecruitHeroItemInfo>
    {
        private GGUIWndHeroCommonCardItem _m_wndHeroCommonCardItem;
        
        public GGUIWndRecruitShopCardItemHero(GGUIMonoRecruitShopCardItemHero _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoHeroCommonCardItem != null)
            {
                _m_wndHeroCommonCardItem = new GGUIWndHeroCommonCardItem(wnd.monoHeroCommonCardItem);
                _m_wndHeroCommonCardItem.ClickAction += _onCardItemClick;
            }
        }

        protected override void _onDiscardSub()
        {
            if (_m_wndHeroCommonCardItem != null)
            {
                _m_wndHeroCommonCardItem.ClickAction -= _onCardItemClick;
                _m_wndHeroCommonCardItem.discard();
            }
            _m_wndHeroCommonCardItem = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wndHeroCommonCardItem?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wndHeroCommonCardItem?.resetWnd();
        }

        protected override void _onSetData()
        {
        }

        protected override void _onRefreshWnd()
        {
            if (_m_wndHeroCommonCardItem != null)
            {
                _m_wndHeroCommonCardItem.showWnd();
                _m_wndHeroCommonCardItem.setInfo(_m_rRecruitItemInfo?.getHeroShowInfo(true));
            }
        }
        
        private void _onCardItemClick(GGUIWndHeroCommonCardItem _itemWnd)
        {
            QueueMgr.instance.AddNode(new GNodeHeroRecruit(_m_iRecruitShopInfo, _m_rRecruitItemInfo));
        }
    }
}