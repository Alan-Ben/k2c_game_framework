using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsScoreRewardBarContainer : _AGGUISubWndCommonContainer<GGUIMonoSevenDayGoalsScoreRewardBarContainerItem, GGUIMonoSevenDayGoalsScoreRewardBarContainer, GGUISubWndSevenDayGoalsScoreRewardBarContainerItem>
    {
        private float _m_width;
        private float _m_totalScore;
        
        
        public GGUISubWndSevenDayGoalsScoreRewardBarContainer(GGUIMonoSevenDayGoalsScoreRewardBarContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }
        
        
        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if (wnd.itemContainer != null && wnd.itemContainer.transform is RectTransform transform)
                _m_width = transform.rect.width;
        }
        

        protected override GGUISubWndSevenDayGoalsScoreRewardBarContainerItem _createItemWnd(GGUIMonoSevenDayGoalsScoreRewardBarContainerItem _itemMono)
        {
            return new GGUISubWndSevenDayGoalsScoreRewardBarContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndSevenDayGoalsScoreRewardBarContainerItem _itemWnd, int _index)
        {
            SevenDayGoalsStepRewardRefObj rewardRef = GRefdataCoreMgr.instance.sevenDayGoalsStepRewardRefCore.refList.SafeGet(_index);
            if (rewardRef == null)
                return;
            
            ALUGUICommon.setUIPos(_itemWnd.rectTransform, _m_totalScore == 0 ? 0 : _m_width / _m_totalScore * rewardRef.need_score, 0);
            _itemWnd.refreshWnd(rewardRef);
        }
        
        
        public new void refreshWnd()
        {
            int count = GRefdataCoreMgr.instance.sevenDayGoalsStepRewardRefCore.refList.Count;
            _m_totalScore = GRefdataCoreMgr.instance.sevenDayGoalsStepRewardRefCore.refList.GetLast()?.need_score ?? 0;
            refreshWnd(count - 1); // count - 1 因为最后一个特殊展示了，这里就剔除掉
        }
    }
}