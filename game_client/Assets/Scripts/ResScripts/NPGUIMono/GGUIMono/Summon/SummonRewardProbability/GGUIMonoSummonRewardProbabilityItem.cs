using ALPackage;

namespace GOE
{
    /// <summary>
    /// 召唤概率item
    /// </summary>
    public class GGUIMonoSummonRewardProbabilityItem : _AALBasicUIWndMono
    {
        [ALHeader("奖励道具")]
        public NPGGUIMonoCommonItem rewardItem;
        
        [ALHeader("概率文本")]
        public TextEx txtProbability;
    }
}