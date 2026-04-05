using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnMedalInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("当前等级的名字")]
        public Text txtName;
        [ALHeader("当前等级")]
        public Text txtLevel;
        [ALHeader("升级进度")]
        public Slider sldLevelProgress;
        public Text txtLevelProgress;
        [ALHeader("等级列表按钮")]
        public GameObject btnLevelList;
        [ALHeader("当前人气值")]
        public Text txtCurPopularity;
        [ALHeader("迎宾次数上限")]
        public Text txtCurReceiveGuestLimit;
        [ALHeader("当前解锁客人数量")]
        public Text txtCurGuestUnlockNum;
        [ALHeader("满级时展示的内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;
        [ALHeader("奖牌名字")]
        public Text txtMedalName;
        [ALHeader("奖牌等级")]
        public Text txtMedalLevel;
        [ALHeader("奖牌图标")]
        public RawImage imgMedalIcon;
        [ALHeader("奖牌描述")]
        public Text txtMedalDesc;
        [ALHeader("获得时间")]
        public Text txtFirstObtainTime;
        [ALHeader("加成类型")]
        public EBonusPropertyType bonusPropertyType;
        [ALHeader("加成值是否展示为百分比")]
        public bool isPercentage = false;
        [ALHeader("加成值是否是金币")]
        public bool isGold = true;
        [ALHeader("当前的加成")]
        public Text txtCurrentBonusPlus;
        [ALHeader("下一级的加成")]
        public Text txtNextBonusPlus;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级特效")]
        public Transform transEffectUpgrade;
        public long effectIdUpgrade = 0;
        [ALHeader("可升级次数")]
        public Text txtCanUpgradeCount;
        [ALHeader("未激活和已激活的展示")]
        public List<GameObject> listNoActivateShow;
        public List<GameObject> listActivateShow;
        public List<MaskableGraphic> listNoActivateGray;
        [ALHeader("可否升级的展示")]
        public List<GameObject> listCanUpgradeShow;
        [ALHeader("满级时的展示")]
        public List<GameObject> listMedalLevelMaxShow;
        public List<GameObject> listMedalLevelMaxHide;
        [ALHeader("星级图标容器")]
        public GGUIMonoInnLevelIconContainer monoStarIconContainer;
        [ALHeader("等级为0时的展示")]
        public List<GameObject> listLevelZeroShow;
        public List<GameObject> listLevelZeroHide;


#if NP_GAME
        public void setActivate(bool _isActivate)
        {
            ALUGUICommon.setGameObjEnable(listNoActivateShow, false);
            ALUGUICommon.setGameObjEnable(listActivateShow, false);
            ALUGUICommon.setGameObjEnable(_isActivate ? listActivateShow : listNoActivateShow, true);
            if (_isActivate)
                GGameCommonInfo.disgrayImage(listNoActivateGray);
            else
                GGameCommonInfo.grayImage(listNoActivateGray);
        }
        public void setCanUpgrade(bool _canUpgrade)
        {
            ALUGUICommon.setGameObjEnable(listCanUpgradeShow, _canUpgrade);
        }
        public void setLevelMax(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        public void setLevelZero(bool _isLevelZero)
        {
            ALUGUICommon.setGameObjEnable(listLevelZeroShow, false);
            ALUGUICommon.setGameObjEnable(listLevelZeroHide, false);
            ALUGUICommon.setGameObjEnable(_isLevelZero ? listLevelZeroShow : listLevelZeroHide, true);
        }
        public void setMedalLevelMax(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listMedalLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listMedalLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listMedalLevelMaxShow : listMedalLevelMaxHide, true);
        }
#endif
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6407); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6407); } }
    }
}