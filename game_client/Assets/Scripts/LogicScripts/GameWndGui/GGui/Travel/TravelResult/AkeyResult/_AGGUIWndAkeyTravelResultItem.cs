using ALPackage;

namespace GOE
{
    public interface _IAkeyTravelResultItemWnd : _IBasicDiffItemContainerItemWnd
    {
        void setData(_ITravelEventResultInfo _resultInfo);
    }
    
    public abstract class _AGGUIWndAkeyTravelResultItem<T> : _ANPGGUIBasicSubWnd<T>, _IAkeyTravelResultItemWnd
        where T : _AGGUIMonoAkeyTravelResultItem
    {
        protected _ITravelEventResultInfo _m_iResultInfo;
        
        private NPGGuiWndTexture _m_targetMidImage;//事件对象半身像
        private NPGGUIWndCommonItemContainer _m_wRewardList;//奖励列表
        
        public _AGGUIWndAkeyTravelResultItem(T _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.targetMidImage != null)
                _m_targetMidImage = new NPGGuiWndTexture(wnd.targetMidImage);

            if (wnd.rewardList != null)
                _m_wRewardList = new NPGGUIWndCommonItemContainer(wnd.rewardList);
            
            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            _m_targetMidImage?.discard();
            _m_targetMidImage = null;
            
            _m_wRewardList?.discard();
            _m_wRewardList = null;
            
            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _m_targetMidImage?.hideWnd();
            _m_wRewardList?.hideWnd();

            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _m_targetMidImage?.discardTexture();
            _m_wRewardList?.resetWnd();
            
            _onResetSub();
        }

        public void setData(_ITravelEventResultInfo _resultInfo)
        {
            _m_iResultInfo = _resultInfo;
            
            _onSetData(_m_iResultInfo);

            _refreshWnd();
        }

        protected void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_iResultInfo != null)
            {
                ALUGUICommon.setLabelTxt(wnd.eventDesc, TextTranslate.instance.getLanguage(_m_iResultInfo.eventResultDesc));

                if (_m_targetMidImage != null)
                {
                    _m_targetMidImage.showWnd();
                    _m_targetMidImage.setTexture(_m_iResultInfo.eventInfo?.getEventMainRole()?.midImage);
                }

                if (_m_wRewardList != null)
                {
                    _m_wRewardList.showWnd();
                    _m_wRewardList.showItemList(_m_iResultInfo.showRewardItemList);
                }
                
                if (_m_iResultInfo.showRewardItemList != null)
                {
                    string firstRewardDescKey = string.IsNullOrEmpty(wnd.firstRewardDescKey) ? TransKeyConst.common_str_colon_str : wnd.firstRewardDescKey;
                    foreach (_IItem itemInfo in _m_iResultInfo.showRewardItemList)
                    {
                        if (itemInfo != null)
                        {
                            ALUGUICommon.setLabelTxt(wnd.firstRewardDesc,
                                TextTranslate.instance.getLanguage(firstRewardDescKey, itemInfo.getItemName(), itemInfo.getCount()));
                            break;
                        }
                    }
                }
            }

            _onRefreshWnd();
        }

        public _AALBasicUIWndMono getWndMono() { return wnd; }

        protected abstract void _onWndInitDoneSub();

        protected abstract void _onDiscardSub();
        
        protected abstract void _onShowWndSub();

        protected abstract void _onHideWndSub();

        protected abstract void _onResetSub();

        protected abstract void _onSetData(_ITravelEventResultInfo _data);
        
        protected abstract void _onRefreshWnd();
    }
}