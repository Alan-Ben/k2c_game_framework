
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeBoxDetail : _AHotfixBaseMono
    {
        [HotfixMono("关闭按钮")]
        public GameObject btnClose;
        [HotfixMono("当前等级文本")]
        public Text txtCurrentLevel;
        [HotfixMono("当前奖励品质概率容器")]
        public GGUIHotfixCommonMono monoCurrentRewardQualityContainer;
        [HotfixMono("下一级等级文本")]
        public Text txtNextLevel;
        [HotfixMono("下一级奖励品质概率容器")]
        public GGUIHotfixCommonMono monoNextRewardQualityContainer;
        [HotfixMono("当前奖励内容")]
        public GGUIHotfixCommonMono monoCurrentRewardContainer;
    }
}