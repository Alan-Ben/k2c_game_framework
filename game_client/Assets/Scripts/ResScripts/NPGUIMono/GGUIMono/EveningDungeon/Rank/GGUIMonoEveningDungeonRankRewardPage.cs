using ALPackage;

namespace GOE
{
    /// <summary>
    /// 排行奖励页面
    /// </summary>
    public class GGUIMonoEveningDungeonRankRewardPage : _AALBasicUIWndMono
    {
        [ALHeader("每轮首次击杀奖励列表")]
        public NPGGUIMonoCommonItemContainer roundFirstKillRewardContainer;
        
        [ALHeader("非首次击杀奖励列表")]
        public NPGGUIMonoCommonItemContainer notFirstKillRewardContainer;
        
        [ALHeader("排行奖励列表")]
        public GGUIMonoEveningDungeonRankRewardContainer monoRewardContainer;
    }
}