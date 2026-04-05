using GOE;

namespace Hotfix
{
    /// <summary>
    /// 万能活动附加排行榜本服页面
    /// </summary>
    public class GGUIMonoRegularEventSubRankSelfPage : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("排行榜列表")]
        public GGUIMonoRankRushDetailRankGrid monoRankGrid;
        [HotfixMonoAttribute("排行榜分数标题")]
        public TextEx txtRankScoreTitle;
        [HotfixMonoAttribute("我的排名")]
        public TextEx txtSelfRank;
        [HotfixMonoAttribute("我的积分")]
        public TextEx txtSelfScore;
    }
}