using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class GGUIWndRecruitShopCardItemConsort : _AGGUIWndRecruitShopCardItem<GGUIMonoRecruitShopCardItemConsort, RecruitConsortItemInfo>
    {
        private GGUIWndConsortCardItem _m_wndConsortCardItem; 
        
        public GGUIWndRecruitShopCardItemConsort(GGUIMonoRecruitShopCardItemConsort _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if(wnd.monoConsortCardItem != null)
                _m_wndConsortCardItem = new GGUIWndConsortCardItem(wnd.monoConsortCardItem, _onItemClick);
        }

        protected override void _onDiscardSub()
        {
            if(_m_wndConsortCardItem != null)
                _m_wndConsortCardItem.discard();
            _m_wndConsortCardItem = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wndConsortCardItem?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wndConsortCardItem?.resetWnd();
        }

        protected override void _onSetData()
        {
        }

        protected override void _onRefreshWnd()
        {
            if(_m_rRecruitItemInfo == null)
                return;
            
            if (_m_wndConsortCardItem != null && _m_rRecruitItemInfo.consortInfo != null)
            {
                _m_wndConsortCardItem.showWnd();
                _m_wndConsortCardItem.setInfo(_m_rRecruitItemInfo.consortInfo);
            }
        }

        private void _onItemClick(_IConsortShowInfo _consortShowInfo)
        {
            QueueMgr.instance.AddNode(new GNodeConsortRecruit(_m_iRecruitShopInfo, _m_rRecruitItemInfo));
        }
    }
}