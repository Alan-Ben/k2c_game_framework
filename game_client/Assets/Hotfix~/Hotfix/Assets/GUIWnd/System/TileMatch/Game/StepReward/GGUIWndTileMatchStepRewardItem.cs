using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励item
    /// </summary>
    public class GGUIWndTileMatchStepRewardItem : _AHotfixBaseSubWnd<GGUIMonoTileMatchStepRewardItem>
    {
        private TileMatchStepRewardItemInfo _m_iItemInfo;
        private int _m_iTotalWeight;
        
        private NPGGUIWndCommonItem _m_wCommonItem;
        
        public GGUIWndTileMatchStepRewardItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd != null)
                _m_wCommonItem = new NPGGUIWndCommonItem(hotfixWnd.monoItem);
        }
        
        protected override void _onDiscard()
        {
            _m_wCommonItem?.discard();
            _m_wCommonItem = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCommonItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCommonItem?.resetWnd();
        }

        public void setData(TileMatchStepRewardItemInfo _itemInfo, int _totalWeight)
        {
            _m_iItemInfo = _itemInfo;
            _m_iTotalWeight = _totalWeight;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(hotfixWnd == null || _m_iItemInfo == null)
                return;

            if (_m_wCommonItem != null)
            {
                _m_wCommonItem.showWnd();
                _m_wCommonItem.setItem(_m_iItemInfo.rewardItem);
            }

            if (hotfixWnd.txtProbability != null)
            {
                float percentage = _m_iTotalWeight == 0 ? 0 : 100.0f * _m_iItemInfo.weight / _m_iTotalWeight;
                ALUGUICommon.setLabelTxt(hotfixWnd.txtProbability,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, percentage.ToString("F2")));
            }
        }
    }
}