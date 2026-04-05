using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("旅店招牌")]
        public GGUIMonoInnMainSignboard monoSignboard;
        [ALHeader("心意值")]
        public Text txtAffectionNum;
        [ALHeader("迎宾按钮")]
        public GGUIMonoInnMainCreateGuest monoCreateGuest;
        [ALHeader("设施列表按钮")]
        public GameObject btnStationList;
        [ALHeader("菜单按钮")]
        public GameObject btnMenu;
        [ALHeader("珍宝按钮")]
        public GameObject btnGift;
        [ALHeader("客人列表按钮")]
        public GameObject btnGuestList;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("旅店 0 级时显示与隐藏的列表")] 
        public List<GameObject> listZeroLevelShow;
        public List<GameObject> listZeroLevelHide;
        [ALHeader("有特殊客人时显示与隐藏的内容")]
        public List<GameObject> listSpecialGuestShow;
        public List<GameObject> listSpecialGuestHide;

        [ALHeader("客户数量tip文本")]
        public TextEx txtGetCashRegisterRewardGuestNumTip;
        [ALHeader("客户数量tip显示动画")]
        public string getCashRegisterRewardGuestNumTipShowAniName;

        public void setLevel(int _innLevel)
        {
            ALUGUICommon.setGameObjEnable(listZeroLevelShow, false);
            ALUGUICommon.setGameObjEnable(listZeroLevelHide, false);
            ALUGUICommon.setGameObjEnable(_innLevel <= 0 ? listZeroLevelShow : listZeroLevelHide, true);
        }
        public void setHasSpecialGuest(bool _show)
        {
            ALUGUICommon.setGameObjEnable(listSpecialGuestShow, false);
            ALUGUICommon.setGameObjEnable(listSpecialGuestHide, false);
            ALUGUICommon.setGameObjEnable(_show ? listSpecialGuestShow : listSpecialGuestHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6400); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6400); } }
    }
}