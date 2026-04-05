
using GOE;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeBoxRewardContainerItem : _AHotfixBaseMono
    {
        [HotfixMono("品质显示GO")]
        public GGUISubMonoQualityShowGo monoQualityShowGo;
        [HotfixMono("品质名称")]
        public Text txtQualityName;
        [HotfixMono("概率")]
        public Text txtProbability;
        [HotfixMono("品质名称和概率")]
        public Text txtQualityNameAndProbability;
        [HotfixMono("奖励容器")]
        public GGUIHotfixCommonMono monoItemContainer;
    }
}