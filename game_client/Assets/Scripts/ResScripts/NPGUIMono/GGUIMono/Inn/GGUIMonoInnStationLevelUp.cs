using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnStationLevelUp : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("设施等级和名称")]
        public Text txtLevelAndName;
        [ALHeader("设施图标")]
        public RawImage imgIcon;
        [ALHeader("等级变化")]
        public CommonUpgradePropertyShow<Text> txtLevelChg;
        [ALHeader("人气值获得量变化")]
        public CommonUpgradePropertyShow<Text> txtPopularityGainChg;
        [ALHeader("熟练度获得量变化")]
        public CommonUpgradePropertyShow<Text> txtFinesseGainChg;
        [ALHeader("设施描述")]
        public Text txtDesc;
        [ALHeader("升级按钮")]
        public GameObject btnLevelUp;
        [ALHeader("是否解锁升级功能的显示")]
        public List<GameObject> listUnlockUpgradeShow;
        public List<GameObject> listLockUpgradeShow;
        public List<MaskableGraphic> listLockUpgradeGray;
        [ALHeader("是否满级的显示")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelNotMaxShow;
        [ALHeader("升级所需的物品")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("下一个建筑的按钮")]
        public GameObject btnNextBuilding;
        public List<GameObject> listHasNextBuildingShow;
        [ALHeader("上一个建筑的按钮")]
        public GameObject btnPrevBuilding;
        public List<GameObject> listHasPrevBuildingShow;


#if NP_GAME
        public void setIsUnlock(bool _isUnlock)
        {
            ALUGUICommon.setGameObjEnable(listUnlockUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listLockUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(_isUnlock ? listUnlockUpgradeShow : listLockUpgradeShow, true);
            if (_isUnlock)
                GGameCommonInfo.disgrayImage(listLockUpgradeGray);
            else
                GGameCommonInfo.grayImage(listLockUpgradeGray);
        }
#endif

        public void setIsLevelMax(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelNotMaxShow, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listLevelMaxShow : listLevelNotMaxShow, true);
        }
        public void setHasNextBuilding(bool _hasNextBuilding)
        {
            ALUGUICommon.setGameObjEnable(listHasNextBuildingShow, _hasNextBuilding);
        }
        public void setHasPrevBuilding(bool _hasPrevBuilding)
        {
            ALUGUICommon.setGameObjEnable(listHasPrevBuildingShow, _hasPrevBuilding);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6419); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6419); } }
    }
}