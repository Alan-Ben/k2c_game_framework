using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 累计召唤奖励领取窗口
    /// </summary>
    public class GGUIMonoSummonCumulativeNumRewardDraw : _AALBasicUIWndMono
    {
        [ALHeader("可领取奖励需要的召唤次数")]
        public TextEx txtCanDrawRewardNeedSummonCount;
        [ALHeader("可领取奖励需要的召唤次数Key(一个参数 可领奖需要的召唤次数)")]
        public string txtCanDrawRewardNeedSummonCountKey;
        
        [ALHeader("奖励道具")]
        public NPGGUIMonoCommonItem monoRewardItem;
        
        [ALHeader("可领取奖励展示物体列表")]
        public List<GameObject> canDrawRewardShowGoList;
        [ALHeader("不可领取奖励展示物体列表")]
        public List<GameObject> cannotDrawRewardShowGoList;
        [ALHeader("不可领取奖励置灰列表")]
        public List<MaskableGraphic> cannotDrawRewardGrayList;
        
        [ALHeader("领取奖励按钮")]
        public GameObject btnDrawReward;

        [ALHeader("关闭窗口按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2203); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2203); } }
    }
}