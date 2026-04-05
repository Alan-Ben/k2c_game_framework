using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 召唤概率弹窗
    /// </summary>
    public class GGUIMonoSummonRewardProbability : _AALBasicUIWndMono
    {
        [ALHeader("召唤奖励概率组容器")]
        public GGUIMonoSummonRewardProbabilityGroupContainer monoSummonRewardProbabilityGroupContainer;
        
        [ALHeader("可领取奖励需要的召唤次数")]
        public TextEx txtCanDrawRewardNeedSummonCount;
        [ALHeader("可领取奖励需要的召唤次数Key(三个参数 1.抽卡消耗道具名 1.需要的召唤次数 2.可领取的道具名)")]
        public string txtCanDrawRewardNeedSummonCountKey;

        [ALHeader("关闭窗口按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2204); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2204); } }
    }
}