namespace GOE
{
    public class GGUIWndMailDetail_Normal : GGUIWndMailDetail_Base<GGUIMonoMailDetail_Normal>
    {
        protected GGUIWndMailRewardItemContainer _m_wndRewardItemContainer; //奖励列表

        public GGUIWndMailDetail_Normal(GMailDataInfo _info)
            : base(_info)
        {
        }

        protected override void _onWndInitDoneEx()
        {
            base._onWndInitDoneEx();

            if (null == wnd)
                return;
            if (null != wnd.monoItemContainer)
            {
                _m_wndRewardItemContainer = new GGUIWndMailRewardItemContainer(wnd.monoItemContainer);
            }
        }

        protected override void _onDiscardEx()
        {
            base._onDiscardEx();
            
            _m_wndRewardItemContainer?.discard();
            _m_wndRewardItemContainer = null;
        }

        protected override void _refreshWndEx()
        {
            if (null == wnd)
                return;
            if (null == _m_miMailDataInfo)
                return;
            bool hasItem = _m_miMailDataInfo.getHasItem();
            bool hasTaken = _m_miMailDataInfo.getHasTaken();
            if (hasItem)
            {
                //有奖励，展示奖励列表
                if (null != _m_wndRewardItemContainer)
                {
                    _m_wndRewardItemContainer.showItemList(hasTaken,_m_miMailDataInfo.mailDetailInfo.getItemList());
                    _m_wndRewardItemContainer.showWnd();
                }
            }
            else
            {   
                //没有奖励的时候隐藏奖励列表
                if (null != _m_wndRewardItemContainer)
                {
                    _m_wndRewardItemContainer.hideWnd();
                }
            }
        }
        
    }
}