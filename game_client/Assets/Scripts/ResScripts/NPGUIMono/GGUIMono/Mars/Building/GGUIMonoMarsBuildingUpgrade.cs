using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingUpgrade : _AALBasicUIWndMono
    {
        [ALHeader("建筑聚焦的设置")]
        public Vector2 focusViewportPos = new Vector2(0.5f, 0.7f);
        public float focusScale = 1.2f;
        public float focusTime = 0.5f;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnCloseAdditional;
        [ALHeader("建筑名字")]
        public Text txtName;
        [ALHeader("建筑等级")]
        public Text txtLevel;
        [ALHeader("建筑等级key")]
        public string txtLevelKey;
        [ALHeader("切换按钮")]
        public GameObject btnBack;
        [ALHeader("火星属性显示")]
        public GGUIMonoMarsPropertyShowItemContainer monoMarsPropertyShowContainer;
        [ALHeader("升级条件容器")]
        public GGUIMonoConditionDescContainer monoConditionContainer;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级耗时")]
        public Text txtUpgradeTime;
        [ALHeader("原始时间")]
        public Text txtOriginTime; // 原始时间：{0}
        [ALHeader("增益说明按钮")]
        public GameObject btnBuffExplain;
        [ALHeader("立即完成")]
        public GameObject btnCompleteNow;
        [ALHeader("立即完成的消耗")]
        public NPGGUIMonoCommonItem monoCompleteNowCostItem;
        [ALHeader("可否建造相关显示内容")]
        public List<GameObject> listCanUpgradeShow;
        public List<GameObject> listCannotUpgradeShow;
        public List<MaskableGraphic> listCannotUpgradeGray;
        [ALHeader("等级已达上限显示内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;
        
        
#if NP_GAME
        public void setUpgradeState(bool _canUpgrade)
        {
            ALUGUICommon.setGameObjEnable(listCanUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listCannotUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(_canUpgrade ? listCanUpgradeShow : listCannotUpgradeShow, true);
            if (_canUpgrade)
                GGameCommonInfo.disgrayImage(listCannotUpgradeGray);
            else
                GGameCommonInfo.grayImage(listCannotUpgradeGray);
        }
        public void setLevelMaxState(bool _isMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_isMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
#endif
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7110); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7110); } }
    }
}