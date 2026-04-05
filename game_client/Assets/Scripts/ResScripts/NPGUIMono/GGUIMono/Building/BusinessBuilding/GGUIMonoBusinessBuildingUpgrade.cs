
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingUpgrade : _AALBasicUIWndMono
    { 
        [ALHeader("建筑名字")]
        public Text txtName;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("升级建筑样式变化")]
        public CommonUpgradePropertyShow<RawImage> monoPreviewTex;
        [ALHeader("升级建筑等级变化")]
        public CommonUpgradePropertyShow<Text> monoLevel;
        [ALHeader("升级建筑员工上限变化")]
        public CommonUpgradePropertyShow<Text> monoEmployeeLimit;
        [ALHeader("升级建筑收益倍率变化")]
        public CommonUpgradePropertyShow<Text> monoEmployeeEarningsRate;
        [ALHeader("升级所需的花费")]
        public NPGGUIMonoCommonItem monoUpgradeCost;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1104); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1104); } }
    }
}