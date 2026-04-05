using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励奖池组item
    /// </summary>
    public class GGUIWndTileMatchStepRewardJackpotGroupItem : _AHotfixBaseSubWnd<GGUIMonoTileMatchStepRewardJackpotGroupItem>
    {
        private TileMatchJackpotGroupRefObj _m_rJackpotGroupRefObj;// 奖池组数据
        private int _m_iTotalWeight;// 奖池组总权重
        
        private GGUIWndTileMatchStepRewardItemContainer _m_wRewardItemContainer;// 奖励item容器
        
        public GGUIWndTileMatchStepRewardJackpotGroupItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            if (hotfixWnd.monoRewardContainer != null)
                _m_wRewardItemContainer = new GGUIWndTileMatchStepRewardItemContainer(hotfixWnd.monoRewardContainer);
        }
        
        protected override void _onDiscard()
        {
            _m_wRewardItemContainer?.discard();
            _m_wRewardItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRewardItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardItemContainer?.resetWnd();
        }

        public void setData(TileMatchJackpotGroupRefObj _jackpotGroupRefObj, int _totalWeight)
        {
            _m_rJackpotGroupRefObj = _jackpotGroupRefObj;
            _m_iTotalWeight = _totalWeight;
            
            _refreshWnd();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _refreshWnd()
        {
            if(hotfixWnd == null && _m_rJackpotGroupRefObj == null)
                return;

            if (_m_wRewardItemContainer != null)
            {
                _m_wRewardItemContainer.showWnd();
                _m_wRewardItemContainer.setData(_m_rJackpotGroupRefObj.reward_list, _m_iTotalWeight);
            }

            string groupNameAndProbabilityKey = string.IsNullOrEmpty(hotfixWnd.txtGroupNameAndProbabilityKey)
                ? "{0} {1}%" : hotfixWnd.txtGroupNameAndProbabilityKey;
            float percentage = _m_iTotalWeight == 0 ? 0 : 100.0f * _m_rJackpotGroupRefObj.weight_total / _m_iTotalWeight;

            ALUGUICommon.setLabelTxt(hotfixWnd.txtGroupNameAndProbability,
                TextTranslate.instance.getLanguage(groupNameAndProbabilityKey,
                    _m_rJackpotGroupRefObj.reward_quality_name, percentage.ToString("F2")));
        }
    }
}