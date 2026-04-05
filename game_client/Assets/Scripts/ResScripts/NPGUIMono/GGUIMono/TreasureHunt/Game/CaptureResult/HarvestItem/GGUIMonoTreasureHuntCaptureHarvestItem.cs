using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝-打捞收获物item
    /// </summary>
    public class GGUIMonoTreasureHuntCaptureHarvestItem : _AALBasicUIWndMono
    {
        [ALHeader("item展示的父节点")]
        public Transform itemShowParent;
        
        [ALHeader("展示矿石的预制")]
        public GGUIMonoTreasureHuntCaptureHarvestItem_Ore showOrePrefab;

        [ALHeader("展示奖励的预制")]
        public GGUIMonoTreasureHuntCaptureHarvestItem_Reward showRewardPrefab;
        
        [ALHeader("展示奇物的预制")]
        public GGUIMonoTreasureHuntCaptureHarvestItem_Treasure showTreasurePrefab;
    }
}