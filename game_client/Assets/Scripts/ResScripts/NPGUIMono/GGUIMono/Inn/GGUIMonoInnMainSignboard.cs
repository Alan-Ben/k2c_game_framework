using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnMainSignboard : _AALBasicUIWndMono
    {
        [ALHeader("当前等级的名字")]
        public Text txtName;
        [ALHeader("当前等级")]
        public Text txtLevel;
        [ALHeader("奖牌图标")]
        public RawImage imgMedalIcon;
        [ALHeader("奖牌等级")]
        public Text txtMedalLevel;
        [ALHeader("升级进度")]
        public Slider sldLevelProgress;
        public Text txtLevelProgress;
        [ALHeader("等级详情按钮")]
        public GameObject btnDetail;
        [ALHeader("满级时展示的内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;
        [ALHeader("零级时的展示和隐藏")]
        public List<GameObject> listZeroLevelShow;
        public List<GameObject> listZeroLevelHide;
        public List<MaskableGraphic> listZeroLevelGray;
        [ALHeader("星级图标容器")]
        public GGUIMonoInnLevelIconContainer monoStarIconContainer;


#if NP_GAME
        public void setLevelMax(bool _levelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_levelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        public void setIsZeroLevel(bool _isZeroLevel)
        {
            ALUGUICommon.setGameObjEnable(listZeroLevelShow, false);
            ALUGUICommon.setGameObjEnable(listZeroLevelHide, false);
            ALUGUICommon.setGameObjEnable(_isZeroLevel ? listZeroLevelShow : listZeroLevelHide, true);
            if (_isZeroLevel)
                GGameCommonInfo.disgrayImage(listZeroLevelGray);
            else
                GGameCommonInfo.grayImage(listZeroLevelGray);
        }  
#endif
    }
}