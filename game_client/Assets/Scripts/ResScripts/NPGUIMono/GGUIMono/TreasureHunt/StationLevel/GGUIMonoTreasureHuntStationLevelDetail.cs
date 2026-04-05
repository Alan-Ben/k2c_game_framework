using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空舱等级详情
    /// </summary>
    public class GGUIMonoTreasureHuntStationLevelDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("当前等级")]
        public TextEx txtCurLevel;
        [ALHeader("当前等级进度条")]
        public NPGGUIMonoProgress monoLevelProgress;
        
        [ALHeader("最高等级时显示")]
        public List<GameObject> maxLevelShow;
        [ALHeader("最高等级时隐藏")]
        public List<GameObject> maxLevelHide;
        
        [ALHeader("等级提升显示")]
        public CommonUpgradePropertyShow<Text> levelUpgradeShow;

        [ALHeader("每次拾取经验值显示")]
        public CommonUpgradePropertyShow<Text> pickupExpUpgradeShow;

        [ALHeader("自动飞行距离显示")]
        public CommonUpgradePropertyShow<Text> autoFlyDistanceUpgradeShow;

        [ALHeader("最大飞行距离显示")]
        public CommonUpgradePropertyShow<Text> maxFlyDistanceUpgradeShow;

        [ALHeader("飞行保护次数显示")]
        public CommonUpgradePropertyShow<Text> flyProtectTimesUpgradeShow;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6802); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6802); } }
    }
}