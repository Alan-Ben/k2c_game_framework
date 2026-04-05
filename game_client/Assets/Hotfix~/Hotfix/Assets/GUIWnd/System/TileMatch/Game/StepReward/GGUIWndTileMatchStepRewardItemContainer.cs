using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励itemContainer
    /// </summary>
    public class GGUIWndTileMatchStepRewardItemContainer : _AHotfixBaseSizeChangeableContainerWnd<GGUIMonoTileMatchStepRewardItemContainer, GGUIWndTileMatchStepRewardItem>
    {
        private List<TileMatchStepRewardItemInfo> _m_lRewardItemInfoList;
        private int _m_lTotalWeight;//总权重
        
        public GGUIWndTileMatchStepRewardItemContainer(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lRewardItemInfoList = null;
        }

        protected override void _onReset()
        {
            _m_lRewardItemInfoList = null;
        }

        protected override void _onDiscard()
        {
            _m_lRewardItemInfoList = null;
        }

        protected override void _onWndInitDoneHotfix()
        {
        }

        protected override GGUIWndTileMatchStepRewardItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            GGUIWndTileMatchStepRewardItem itemWnd = new GGUIWndTileMatchStepRewardItem(_itemMono);
            return itemWnd;
        }

        protected override bool _refreshItemWnd(int _index, GGUIWndTileMatchStepRewardItem _itemWnd)
        {
            if (_m_lRewardItemInfoList == null || _index < 0 || _index >= _m_lRewardItemInfoList.Count || _itemWnd == null)
                return false;
            
            _itemWnd.setData(_m_lRewardItemInfoList[_index], _m_lTotalWeight);
            return true;
        }
        
        public void setData(List<TileMatchStepRewardItemInfo> _rewardItemInfoList, int _totalWeight)
        {
            _m_lRewardItemInfoList = _rewardItemInfoList;
            _m_lTotalWeight = _totalWeight;

            showItemList(_m_lRewardItemInfoList?.Count ?? 0);
        }
    }
}