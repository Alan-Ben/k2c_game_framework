using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoSevenDayGoals : _AALBasicUIWndMono
    {
        [ALHeader("活动倒计时")]
        public Text txtLeftTime;
        [ALHeader("积分奖励的 bar ")]
        public GGUIMonoSevenDayGoalsScoreRewardBar monoScoreBar;
        [ALHeader("天数按钮的容器")]
        public GGUIMonoSevenDayGoalsDayBtnContainer monoDayBtnContainer;
        [ALHeader("指定天数的内容页面")]
        public GGUIMonoSevenDayGoalsDayContent monoDayContent;
    }
}