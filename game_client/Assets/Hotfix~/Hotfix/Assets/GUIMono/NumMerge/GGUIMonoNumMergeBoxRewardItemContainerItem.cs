
using GOE;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeBoxRewardItemContainerItem : _AHotfixBaseMono
    {
        [HotfixMono("物品")]
        public NPGGUIMonoCommonItem monoItem;
        [HotfixMono("概率文本")]
        public Text txtProbability;
    }
}