
using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFarmingBuildingUpgradeSuccess : _AALBasicUIWndMono
    {
        [ALHeader("升级建筑样式变化")]
        public CommonUpgradePropertyShow<RawImage> monoPreviewTex;
        [ALHeader("等级提升的描述")]
        public Text txtUpgradeDesc;
        [ALHeader("升级建筑等级变化")]
        public CommonUpgradePropertyShow<Text> monoLevel;
        [ALHeader("升级建筑加成变化")]
        public CommonUpgradePropertyShow<Text> monoBonus;
        [ALHeader("升级建筑点击收益变化")]
        public CommonUpgradePropertyShow<Text> monoClickEarnings;
        [ALHeader("升级建筑每秒收益变化")]
        public CommonUpgradePropertyShow<Text> monoAutoClickPerS;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1107); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1107); } }
    }
}