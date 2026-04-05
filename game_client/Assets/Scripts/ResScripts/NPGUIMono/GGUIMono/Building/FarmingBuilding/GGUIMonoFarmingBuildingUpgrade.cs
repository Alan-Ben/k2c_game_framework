using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFarmingBuildingUpgrade : _AALBasicUIWndMono
    {
        [ALHeader("建筑名字")]
        public Text txtName;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("升级建筑样式变化")]
        public CommonUpgradePropertyShow<RawImage> monoPreviewTex;
        [ALHeader("升级建筑等级变化")]
        public CommonUpgradePropertyShow<Text> monoLevel;
        [ALHeader("升级建筑加成变化")]
        public CommonUpgradePropertyShow<Text> monoBonus;
        [ALHeader("升级建筑点击收益变化")]
        public CommonUpgradePropertyShow<Text> monoClickEarnings;
        [ALHeader("升级建筑每秒收益变化")]
        public CommonUpgradePropertyShow<Text> monoAutoClickPerS;
        [ALHeader("升级建筑的消耗")]
        public NPGGUIMonoCommonItem monoUpgradeCost;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("满级时展示的内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;


        public void setLevelMax(bool _levelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_levelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1106); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1106); } }
    }
}