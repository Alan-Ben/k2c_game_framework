using GOE;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励奖池组item
    /// </summary>
    public class GGUIMonoTileMatchStepRewardJackpotGroupItem : _AHotfixBaseMono
    {
        [HotfixMono("奖励Container")]
        public GGUIHotfixCommonMono monoRewardContainer;

        [HotfixMono("奖池名称和概率文本")]
        public TextEx txtGroupNameAndProbability;
        [HotfixMono("奖池名称和概率文本key(两个参数 1.奖池名称 2. 奖池概率)")]
        public string txtGroupNameAndProbabilityKey;
    }
}