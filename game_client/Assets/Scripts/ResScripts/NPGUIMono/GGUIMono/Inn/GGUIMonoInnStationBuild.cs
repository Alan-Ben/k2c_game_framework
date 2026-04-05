using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnStationBuild : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("设施名称")]
        public Text txtName;
        [ALHeader("设施图标")]
        public RawImage imgIcon;
        [ALHeader("人气值获得量")]
        public Text txtPopularityGain;
        [ALHeader("熟练度获得量")]
        public Text txtFinesseGain;
        [ALHeader("设施描述")]
        public Text txtDesc;
        [ALHeader("解锁提示")]
        public Text txtUnlockTip;
        [ALHeader("迎宾需求提示")]
        public Text txtGuestServeRequire;
        public Color notEnoughColor;
        [ALHeader("建造按钮")]
        public GameObject btnBuild;
        [ALHeader("是否是下一个可建造的建筑")]
        public List<GameObject> listCannotBuildShow;
        public List<GameObject> listNextBuildingShow;
        public List<MaskableGraphic> listCannotBuildGray;
        [ALHeader("建造所需的物品")]
        public NPGGUIMonoCommonItem monoCostItem;


#if NP_GAME

        public void setIsNextBuilding(bool _isNextBuilding)
        {
            ALUGUICommon.setGameObjEnable(listNextBuildingShow, false);
            ALUGUICommon.setGameObjEnable(listCannotBuildShow, false);
            ALUGUICommon.setGameObjEnable(_isNextBuilding ? listNextBuildingShow : listCannotBuildShow, true);
            if (_isNextBuilding)
                GGameCommonInfo.disgrayImage(listCannotBuildGray);
            else
                GGameCommonInfo.grayImage(listCannotBuildGray);
        }
        
#endif

        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6427); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6427); } }
    }
}