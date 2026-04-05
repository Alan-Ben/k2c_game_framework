using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoSevenDayGoalsScoreRewardBar : _AALBasicUIWndMono
    {
        [ALHeader("当前的积分")]
        public Text txtCurProcess;
        [ALHeader("进度条")] 
        public Slider sldProcess;
        [ALHeader("奖励列表")]
        public GGUIMonoSevenDayGoalsScoreRewardBarContainer monoRewardContainer;
        [ALHeader("最后一个特殊的奖励 item ")]
        public GGUIMonoSevenDayGoalsScoreRewardBarContainerItem monoLastSpecItem;
    }
}