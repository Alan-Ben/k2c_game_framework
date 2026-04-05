using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情排行榜页面
    /// </summary>
    public class GGUIMonoGuildRankRushDetailRankPage : _AALBasicUIWndMono
    {
        [ALHeader("分数标题")]
        public Text txtScoreTitle;
        [ALHeader("排行榜列表")]
        public GGUIMonoGuildRankRushDetailRankGrid monoRankGrid;
    }
}
