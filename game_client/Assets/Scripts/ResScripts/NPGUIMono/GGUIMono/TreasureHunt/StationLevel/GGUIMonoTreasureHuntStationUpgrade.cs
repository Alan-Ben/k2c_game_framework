using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空舱升级弹窗
    /// </summary>
    public class GGUIMonoTreasureHuntStationUpgrade : _AALBasicUIWndMono
    {
        [ALHeader("太空舱等级文本")]
        public TextEx txtLevel;
        
        [ALHeader("每次拾取经验值显示")]
        public CommonUpgradePropertyShow<Text> pickupExpUpgradeShow;

        [ALHeader("自动飞行距离显示")]
        public CommonUpgradePropertyShow<Text> autoFlyDistanceUpgradeShow;

        [ALHeader("最大飞行距离显示")]
        public CommonUpgradePropertyShow<Text> maxFlyDistanceUpgradeShow;

        [ALHeader("飞行保护次数显示")]
        public CommonUpgradePropertyShow<Text> flyProtectTimesUpgradeShow;

        [ALHeader("有区域解锁时显示物体列表")]
        public List<GameObject> hasUnlockAreaShowGoList;
        [ALHeader("解锁区域图标")]
        public RawImage imgUnlockAreaIcon;
        [ALHeader("解锁区域名称文本")]
        public TextEx txtUnlockAreaName;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6832); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6832); } }
    }
}