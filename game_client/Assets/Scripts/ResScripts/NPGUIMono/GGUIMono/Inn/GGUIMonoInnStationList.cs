using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnStationList : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("内容列表")]
        public GGUIMonoInnStationListContainer monoContainer;
        [ALHeader("设施大图")]
        public RawImage imgStationTex;
        [ALHeader("等级名称")]
        public Text txtNameLevel;
        [ALHeader("设施描述")]
        public Text txtDesc;
        [ALHeader("提示文本")]
        public Text txtTip;
        [ALHeader("已建造时显示GO列表")]
        public List<GameObject> goAlreadyBuildShowList;
        [ALHeader("已建造时隐藏GO列表")]
        public List<GameObject> goAlreadyBuildHideList;

        [ALInfo("====未建造配置====")]
        [ALHeader("建造按钮")]
        public GameObject btnBuild;
        [ALHeader("建造初始人气值")]
        public Text txtPopularityGain;
        [ALHeader("建造初始熟练度")]
        public Text txtFinesseGain;
        [ALHeader("解锁提示")]
        public Text txtUnlockTip;
        [ALHeader("迎宾需求提示")]
        public Text txtGuestServeRequire;
        public Color notEnoughColor;
        [ALHeader("建造所需的物品")]
        public NPGGUIMonoCommonItem monoBuildCostItem;
        [ALHeader("是否是下一个可建造的建筑")]
        public List<GameObject> listCannotBuildShow;
        public List<GameObject> listNextBuildingShow;
        [ALHeader("不可建造需要置灰的列表")]
        public List<MaskableGraphic> listCannotBuildGray;


        [ALInfo("====已建造配置====")]
        [ALHeader("升级按钮")]
        public GameObject btnLevelUp;
        [ALHeader("未解锁升级按钮")]
        public GameObject btnLockLevelUp;
        [ALHeader("等级变化")]
        public CommonUpgradePropertyShow<Text> txtLevelChg;
        [ALHeader("人气值获得量变化")]
        public CommonUpgradePropertyShow<Text> txtPopularityGainChg;
        [ALHeader("熟练度获得量变化")]
        public CommonUpgradePropertyShow<Text> txtFinesseGainChg;
        [ALHeader("是否解锁升级功能的显示")]
        public List<GameObject> listUnlockUpgradeShow;
        public List<GameObject> listLockUpgradeShow;
        public List<MaskableGraphic> listLockUpgradeGray;
        [ALHeader("是否满级的显示")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelNotMaxShow;
        [ALHeader("升级所需的物品")]
        public NPGGUIMonoCommonItem monoUpgradeCostItem;
        [ALHeader("设施菜品列表")]
        public GGUIMonoInnStationDishGrid monoStationDishGrid;

#if NP_GAME
        /// <summary>
        /// 设置是否是下一个可建造的建筑
        /// </summary>
        /// <param name="_isNextBuilding"></param>
        public void setIsNextBuilding(bool _isNextBuilding, bool _canBuild)
        {
            ALUGUICommon.setGameObjEnable(listNextBuildingShow, false);
            ALUGUICommon.setGameObjEnable(listCannotBuildShow, false);
            ALUGUICommon.setGameObjEnable(_isNextBuilding ? listNextBuildingShow : listCannotBuildShow, true);
            if (_canBuild)
                GGameCommonInfo.disgrayImage(listCannotBuildGray);
            else
                GGameCommonInfo.grayImage(listCannotBuildGray);
        }

        /// <summary>
        /// 设置是否解锁升级功能
        /// </summary>
        /// <param name="_isUnlock"></param>
        public void setIsUpgradeUnlock(bool _isUnlock)
        {
            ALUGUICommon.setGameObjEnable(listUnlockUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listLockUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(_isUnlock ? listUnlockUpgradeShow : listLockUpgradeShow, true);
            if (_isUnlock)
                GGameCommonInfo.disgrayImage(listLockUpgradeGray);
            else
                GGameCommonInfo.grayImage(listLockUpgradeGray);
        }

        /// <summary>
        /// 设置是否满级
        /// </summary>
        /// <param name="_isLevelMax"></param>
        public void setIsLevelMax(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelNotMaxShow, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listLevelMaxShow : listLevelNotMaxShow, true);
        }
#endif

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6418); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6418); } }
    }
}