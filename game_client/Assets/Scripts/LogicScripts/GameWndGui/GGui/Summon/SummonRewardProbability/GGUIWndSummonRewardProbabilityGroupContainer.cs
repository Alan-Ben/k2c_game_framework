using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 召唤概率组列表
    /// </summary>
    public class GGUIWndSummonRewardProbabilityGroupContainer : _ATNPGGUIWndSizeChangeableContainer<GGUIMonoSummonRewardProbabilityGroupContainerItem, GGUIMonoSummonRewardProbabilityGroupContainer, GGUIWndSummonRewardProbabilityGroupContainerItem>
    {
        private List<List<GachaItemShowInfoRefObj>> _m_lShowItemGroupList;
        
        public GGUIWndSummonRewardProbabilityGroupContainer(GGUIMonoSummonRewardProbabilityGroupContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndSummonRewardProbabilityGroupContainerItem _createItemWnd(GGUIMonoSummonRewardProbabilityGroupContainerItem _itemMono)
        {
            GGUIWndSummonRewardProbabilityGroupContainerItem itemWnd = new GGUIWndSummonRewardProbabilityGroupContainerItem(_itemMono);
            return itemWnd;
        }
        
        protected override bool _refreshItemWnd(int _index, GGUIWndSummonRewardProbabilityGroupContainerItem _itemWnd)
        {
            if (_itemWnd == null || _m_lShowItemGroupList == null || _index < 0 || _index >= _m_lShowItemGroupList.Count)
                return false;
            
            _itemWnd.setData(_m_lShowItemGroupList[_index]);
            return true;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        public void setData(List<List<GachaItemShowInfoRefObj>> _showItemGroupList)
        {
            _m_lShowItemGroupList = _showItemGroupList;
            
            showItemList(_m_lShowItemGroupList?.Count ?? 0);
        }
    }
}