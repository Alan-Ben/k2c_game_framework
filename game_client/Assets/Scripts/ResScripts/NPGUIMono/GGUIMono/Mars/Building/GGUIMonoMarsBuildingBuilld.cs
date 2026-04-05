using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingBuilld : _AALBasicUIWndMono
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
        [ALHeader("建筑描述")]
        public Text txtDesc;
        [ALHeader("建造条件容器")]
        public GGUIMonoConditionDescContainer monoConditionContainer;
        [ALHeader("建造按钮")]
        public GameObject btnBuild;
        [ALHeader("建造耗时")]
        public Text txtBuildTime;
        [ALHeader("原始时间")]
        public Text txtOriginTime; // 原始时间：{0}
        [ALHeader("增益说明按钮")]
        public GameObject btnBuffExplain;
        [ALHeader("立即完成")]
        public GameObject btnCompleteNow;
        [ALHeader("立即完成的消耗")]
        public NPGGUIMonoCommonItem monoCompleteNowCostItem;
        [ALHeader("可否建造相关显示内容")]
        public List<GameObject> listCanBuildShow;
        public List<GameObject> listCannotBuildShow;
        public List<MaskableGraphic> listCannotBuildGray;
        
        
#if NP_GAME
        public void setBuildState(bool _canBuild)
        {
            ALUGUICommon.setGameObjEnable(listCanBuildShow, false);
            ALUGUICommon.setGameObjEnable(listCannotBuildShow, false);
            ALUGUICommon.setGameObjEnable(_canBuild ? listCanBuildShow : listCannotBuildShow, true);
            if (_canBuild)
                GGameCommonInfo.disgrayImage(listCannotBuildGray);
            else
                GGameCommonInfo.grayImage(listCannotBuildGray);
        }
#endif
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7103); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7103); } }
    }
}