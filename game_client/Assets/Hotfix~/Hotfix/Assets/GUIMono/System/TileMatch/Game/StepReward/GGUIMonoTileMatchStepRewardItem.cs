using GOE;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励item
    /// </summary>
    public class GGUIMonoTileMatchStepRewardItem : _AHotfixBaseMono
    {
        [HotfixMono("MonoCommonItem")]
        public NPGGUIMonoCommonItem monoItem;

        [HotfixMono("概率文本")]
        public TextEx txtProbability;
    }
}