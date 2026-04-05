using ALPackage;

namespace GOE
{
    /// <summary>
    /// 冲榜详情奖励页面
    /// </summary>
    public class GGUIMonoRankRushPage : _AALBasicUIWndMono
    {
        [ALHeader("冲榜列表")]
        public GGUIMonoRankRushContainer monoRankRushContainer;
        [ALHeader("有冲榜活动展示音效id")]
        public long haveListAudioId;
    }
}
