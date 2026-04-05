using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 召唤概率组列表item
    /// </summary>
    public class GGUIWndSummonRewardProbabilityGroupContainerItem : _ATALBasicUISubWnd<GGUIMonoSummonRewardProbabilityGroupContainerItem>
    {
        private List<GachaItemShowInfoRefObj> _m_lGachaItemShowInfoList;

        private GGUIWndSummonRewardProbabilityItemContainer _m_wRewardProbabilityItemContainer;
        
        public GGUIWndSummonRewardProbabilityGroupContainerItem(GGUIMonoSummonRewardProbabilityGroupContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.monoRewardProbabilityItemContainer != null)
                _m_wRewardProbabilityItemContainer = new GGUIWndSummonRewardProbabilityItemContainer(wnd.monoRewardProbabilityItemContainer);
        }
        
        protected override void _onDiscard()
        {
            _m_wRewardProbabilityItemContainer?.discard();
            _m_wRewardProbabilityItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRewardProbabilityItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardProbabilityItemContainer?.resetWnd();
        }

        public void setData(List<GachaItemShowInfoRefObj> _gachaItemShowInfoList)
        {
            _m_lGachaItemShowInfoList = _gachaItemShowInfoList;

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_wRewardProbabilityItemContainer != null)
            {
                _m_wRewardProbabilityItemContainer.showWnd();
                _m_wRewardProbabilityItemContainer.setData(_m_lGachaItemShowInfoList);
            }
        }
    }
}