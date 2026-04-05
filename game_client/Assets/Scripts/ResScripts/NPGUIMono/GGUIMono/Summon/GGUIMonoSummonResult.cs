using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 召唤结果弹窗
    /// </summary>
    public class GGUIMonoSummonResult : _AALBasicUIWndMono
    {
        [ALHeader("奖励列表")]
        public NPGGUIMonoGetItemContainer monoRewardContainer;
        [ALHeader("全部物品显示前的物体列表")]
        public List<GameObject> beforeAllItemShowShowGoList;
        [ALHeader("全部物品显示后的物体列表")]
        public List<GameObject> afterAllItemShowShowGoList;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        [ALHeader("单抽显示物体列表")]
        public List<GameObject> oneDrawShowGoList;
        [ALHeader("十抽显示物体列表")]
        public List<GameObject> tenDrawShowGoList;
        
        [ALHeader("再抽一次消耗道具")]
        public NPGGUIMonoCommonItem monoDrawAgainCostItem;
        [ALHeader("再抽一次按钮")]
        public GameObject btnDrawAgain;

        [ALHeader("免费抽卡固定CD展示")]
        public GGUIMonoCommonFixedCd freeDrawFixedCd;
        [ALHeader("免费抽卡按钮")]
        public GameObject btnFreeDraw;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2202); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2202); } }
    }
}