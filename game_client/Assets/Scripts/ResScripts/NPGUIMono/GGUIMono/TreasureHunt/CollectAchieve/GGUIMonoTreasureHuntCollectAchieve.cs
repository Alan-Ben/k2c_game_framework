using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 收集成就
    /// </summary>
    public class GGUIMonoTreasureHuntCollectAchieve : _ANPBasicUIWndResBarMono
    {
        [ALHeader("成就点奖励显示")]
        public GGUISubMonoCommonRewardShow monoAchievePointRewardShow;
        [ALHeader("成就点名")]
        public TextEx txtAchievePointName;
        [ALHeader("成就点描述文本")]
        public TextEx txtAchievePointDesc;
        [ALHeader("成就点数进度条")]
        public NPGGUIMonoProgress monoAchievePointProgress;
        [ALHeader("成就点奖励领取状态显示")]
        public List<NPCommonEnumStatInfo<ENPCommonGetStat>> achievePointRewardGetStateShow;
        [ALHeader("领取成就点奖励按钮")]
        public GameObject btnDrawAchievePointReward;
        
        [ALHeader("成就步骤列表")]
        public GGUIMonoAchieveStepGrid monoAchieveStepGrid;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6824); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6824); } }
    }
}