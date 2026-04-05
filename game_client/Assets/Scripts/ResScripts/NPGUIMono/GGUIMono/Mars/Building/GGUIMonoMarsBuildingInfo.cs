using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingInfo : _AALBasicUIWndMono
    {
        [ALHeader("建筑聚焦的设置")]
        public Vector2 focusViewportPos = new Vector2(0.5f, 0.7f);
        public float focusScale = 1.2f;
        public float focusTime = 0.5f;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        public GameObject btnCloseAdditional;
        [ALHeader("建筑名称")]
        public Text txtName;
        [ALHeader("建筑等级")]
        public Text txtLevel;
        [ALHeader("建筑部件进度")]
        public Text txtEquipmentProgress;
        public Slider sldEquipmentProgress;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("页签按钮列表")]
        public GGUIMonoMarsBuildingInfoPageTab monoPageTab;
        [ALHeader("可以派遣居民的相关内容")]
        public List<GameObject> listCanSettleShow;
        public List<GameObject> listCannotSettleShow;
        [ALHeader("部件进度满或未满时显示的相关内容")] 
        public List<GameObject> listEquipmentCompleteShow;
        public List<GameObject> listEquipmentNotCompleteShow;
        public List<MaskableGraphic> listEquipmentNotCompleteGray;
        [ALHeader("部件升级效果相关")]
        public Transform equipmentProgressSfxParent;
        public long equipmentProgressSfxId;
        public float equipmentProgressRefreshDelay = 0;
        [ALHeader("升级飞行物父对象和终点")]
        public Transform upgradeEffectParent;
        public Transform upgradeEffectEndPos;
        public long upgradeEffectEndSfxId;
        [ALHeader("建筑满级后显示隐藏的内容")]
        public List<GameObject> listBuildingLevelMaxShow;
        public List<GameObject> listBuildingLevelMaxHide;
        
        [ALHeader("能源产出详情按钮(有产出速度时才会展示)")]
        public GameObject btnEnergyOutputDetail;


        public void setSettleState(bool _canSettle)
        {
            ALUGUICommon.setGameObjEnable(listCanSettleShow, false);
            ALUGUICommon.setGameObjEnable(listCannotSettleShow, false);
            ALUGUICommon.setGameObjEnable(_canSettle ? listCanSettleShow : listCannotSettleShow, true);
        }
        public void setEquipmentCompleteState(bool _isComplete)
        {
            ALUGUICommon.setGameObjEnable(listEquipmentCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listEquipmentNotCompleteShow, false);
            ALUGUICommon.setGameObjEnable(_isComplete ? listEquipmentCompleteShow : listEquipmentNotCompleteShow, true);
#if NP_GAME
            if (_isComplete)
                GGameCommonInfo.disgrayImage(listEquipmentNotCompleteGray);
            else
                GGameCommonInfo.grayImage(listEquipmentNotCompleteGray);
#endif
        }
        public void setBuildingLevelMaxState(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listBuildingLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listBuildingLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listBuildingLevelMaxShow : listBuildingLevelMaxHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7107); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7107); } }
    }
}