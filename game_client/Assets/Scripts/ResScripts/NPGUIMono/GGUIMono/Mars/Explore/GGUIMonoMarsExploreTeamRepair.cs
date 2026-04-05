using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamRepair : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("所需的原始修复时间")]
        public Text txtRepairNeedOriginTime;
        [ALHeader("实际的修复时间")]
        public Text txtRepairNeedRealTime;
        [ALHeader("修复所需的资源")]
        public NPGGUIMonoCommonItem monoRepairCostItem;
        [ALHeader("修复中的剩余时间")]
        public Text txtRepairingTime;
        [ALHeader("修复进度条")]
        public Slider sldRepairingProgress;
        [ALHeader("取消修复按钮")]
        public GameObject btnCancelRepair;
        [ALHeader("队伍序号")]
        public Text txtNum;
        [ALHeader("队伍名称")]
        public Text txtName;
        [ALHeader("带兵量")]
        public Text txtSoldierNum;
        [ALHeader("士兵损耗量")]
        public Text txtSoldierLossNum;
        [ALHeader("实力加成百分比")]
        public Text txtPowerAddPercent;
        [ALHeader("大臣列表")]
        public GGUIMonoMarsExploreTeamHeroContainer monoHeroContainer;
        [ALHeader("队伍实力")] 
        public Text txtTeamPower;
        [ALHeader("修复数量的进度条")]
        public GGUIMonoBagPopCounter monoNumSlider;
        [ALHeader("立即完成按钮")]
        public GameObject btnCompleteNow;
        [ALHeader("立即完成所需的资源")]
        public NPGGUIMonoCommonItem monoCompleteNowCostItem;
        [ALHeader("修复按钮")]
        public GameObject btnRepair;
        [ALHeader("公会帮助按钮")]
        public GameObject btnGuildHelp;
        [ALHeader("加速按钮")]
        public GameObject btnSpeedUp;
        [ALHeader("正在修复中的显示列表")]
        public List<GameObject> listRepairingShow;
        public List<GameObject> listRepairingHide;
        public List<MaskableGraphic> listRepairingGray;
        [ALHeader("可以公会帮助显示列表")]
        public List<GameObject> listCanGuildHelpShow;
        public List<GameObject> listCanGuildHelpHide;


#if NP_GAME
        public void setRepairing(bool _isRepairing)
        {
            ALUGUICommon.setGameObjEnable(listRepairingShow, false);
            ALUGUICommon.setGameObjEnable(listRepairingHide, false);
            ALUGUICommon.setGameObjEnable(_isRepairing ? listRepairingShow : listRepairingHide, true);
            
            if (_isRepairing)
                GGameCommonInfo.grayImage(listRepairingGray);
            else
                GGameCommonInfo.disgrayImage(listRepairingGray);
        }
        public void setCanGuildHelp(bool _canGuildHelp)
        {
            ALUGUICommon.setGameObjEnable(listCanGuildHelpShow, false);
            ALUGUICommon.setGameObjEnable(listCanGuildHelpHide, false);
            ALUGUICommon.setGameObjEnable(_canGuildHelp ? listCanGuildHelpShow : listCanGuildHelpHide, true);
        }
#endif
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7313); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7313); } }
    }
}