using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingInfoPageEquipment : _AALBasicUIWndMono
    {
        [ALHeader("部件图标")]
        public RawImage imgIcon;
        [ALHeader("部件名称")]
        public Text txtName;
        [ALHeader("部件描述")]
        public Text txtDesc;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗")]
        public NPGGUIMonoCommonItem monoUpgradeCost;
        [ALHeader("相关显示状态的列表")] 
        public List<GameObject> listLockShow;
        public List<GameObject> listCanUpgradeShow;
        public List<GameObject> listReachLevelLimitShow;
        public List<GameObject> listLevelMaxShow;
        public List<MaskableGraphic> listCannotUpgradeGray;
        [ALHeader("当前选中的部件的属性")]
        public GGUIMonoMarsBuildingInfoEquipmentPropertyContainer monoPropertyContainer;
        [ALHeader("当前的部件列表")]
        public GGUIMonoMarsBuildingInfoEquipmentContainer monoEquipmentContainer;
        [ALHeader("扫光特效父对象和 id ")]
        public Transform upgradeEffectSfxParent;
        public long upgradeEffectSfxId;
        public float upgradeEffectSfxDelay = 0.15f;

#if NP_GAME
        
        public void setLock()
        {
            ALUGUICommon.setGameObjEnable(listCanUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listReachLevelLimitShow, false);
            ALUGUICommon.setGameObjEnable(listLockShow, true);
            GGameCommonInfo.grayImage(listCannotUpgradeGray);
        }
        public void setCanUpgrade()
        { 
            ALUGUICommon.setGameObjEnable(listReachLevelLimitShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listCanUpgradeShow, true);
            GGameCommonInfo.disgrayImage(listCannotUpgradeGray);
        }
        public void setReachLevelLimit()
        {
            ALUGUICommon.setGameObjEnable(listCanUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listReachLevelLimitShow, true);
            GGameCommonInfo.grayImage(listCannotUpgradeGray);
        }
        public void setLevelMax()
        {
            ALUGUICommon.setGameObjEnable(listCanUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listReachLevelLimitShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, true);
            GGameCommonInfo.grayImage(listCannotUpgradeGray);
        }
        
#endif
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7108); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7108); } }
    }
}