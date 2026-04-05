using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 召唤概率item列表
    /// </summary>
    public class GGUIWndSummonRewardProbabilityItemContainer : _ATNPGGUIWndSizeChangeableContainer<GGUIMonoSummonRewardProbabilityItem, GGUIMonoSummonRewardProbabilityItemContainer, GGUIWndSummonRewardProbabilityItem>
    {
        private List<GachaItemShowInfoRefObj> _m_lGachaItemShowInfoList;
        
        public GGUIWndSummonRewardProbabilityItemContainer(GGUIMonoSummonRewardProbabilityItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndSummonRewardProbabilityItem _createItemWnd(GGUIMonoSummonRewardProbabilityItem _itemMono)
        {
            GGUIWndSummonRewardProbabilityItem itemWnd = new GGUIWndSummonRewardProbabilityItem(_itemMono);
            return itemWnd;
        }

        protected override bool _refreshItemWnd(int _index, GGUIWndSummonRewardProbabilityItem _itemWnd)
        {
            if (_itemWnd == null || _m_lGachaItemShowInfoList == null || _index < 0 || _index >= _m_lGachaItemShowInfoList.Count)
                return false;

            _itemWnd.setData(_m_lGachaItemShowInfoList[_index]);            
            
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

        public void setData(List<GachaItemShowInfoRefObj> _gachaItemShowInfoList)
        {
            _m_lGachaItemShowInfoList = _gachaItemShowInfoList;
            
            showItemList(_m_lGachaItemShowInfoList?.Count ?? 0);
        }
    }
}