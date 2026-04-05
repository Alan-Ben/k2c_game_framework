using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMuseumItemInfo : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("技能图标")]
        public RawImage imgSkillIcon;
        [ALHeader("品质图标")]
        public RawImage imgQualityIcon;
        [ALHeader("品质的 GO 显示")]
        public GGUISubMonoQualityShowGo monoQualityShowGo;
        [ALHeader("品质文本")]
        public Text txtQuality;
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("等级")]
        public CommonUpgradePropertyShow<Text> txtLevel;
        [ALHeader("获得时间")]
        public Text txtObtainTime;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("激活按钮和激活时的特效")]
        public GameObject btnActivate;
        public long effectIdActivate;
        public Transform transEffectActivate;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗")]
        public NPGGUIMonoCommonItem monoUpgradeCose;
        [ALHeader("当前的加成")]
        public Text txtCurrentBonusPlus;
        [ALHeader("下一级的加成")]
        public Text txtNextBonusPlus;
        [ALHeader("加成的等级")]
        public Text txtBonusLevel;
        [ALHeader("三种状态的显示内容")]
        public List<GameObject> listNoGainShow;
        public List<GameObject> listNoActivateShow;
        public List<GameObject> listActivateShow;
        public List<MaskableGraphic> listNoGainGray;
        public List<MaskableGraphic> listNoActivateGray;
        [ALHeader("是否满级的显示")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("下一个珍宝的按钮")]
        public GameObject btnNextItem;
        public List<GameObject> listHasNextItemShow;
        [ALHeader("上一个珍宝的按钮")]
        public GameObject btnPrevItem;
        public List<GameObject> listHasPrevItemShow;
        [ALHeader("升级成功特效id")]
        public long upgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform transUpgradeSfx;
        [ALHeader("升级成功特效id2")]
        public long upgradeSfxId2;
        [ALHeader("升级成功特效父节点2")]
        public Transform transUpgradeSfx2;


#if NP_GAME
        public void setState(bool _isGain, bool _isActivate, bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listNoGainShow, false);
            ALUGUICommon.setGameObjEnable(listNoActivateShow, false);
            ALUGUICommon.setGameObjEnable(listActivateShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            
            if (_isGain)
            {
                ALUGUICommon.setGameObjEnable(_isActivate ? listActivateShow : listNoActivateShow, true);
                GGameCommonInfo.disgrayImage(listNoGainGray);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(listNoGainShow, true);
                GGameCommonInfo.grayImage(listNoGainGray);
            }
            if (_isActivate)
                GGameCommonInfo.disgrayImage(listNoActivateGray);
            else
                GGameCommonInfo.grayImage(listNoActivateGray);
            
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        public void setHasNextItem(bool _hasNextItem)
        {
            ALUGUICommon.setGameObjEnable(listHasNextItemShow, _hasNextItem);
        }
        public void setHasPrevItem(bool _hasPrevItem)
        {
            ALUGUICommon.setGameObjEnable(listHasPrevItemShow, _hasPrevItem);
        }
#endif
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6423); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6423); } }
    }
}