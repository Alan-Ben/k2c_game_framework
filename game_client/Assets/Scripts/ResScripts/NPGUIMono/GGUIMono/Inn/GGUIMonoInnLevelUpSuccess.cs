using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnLevelUpSuccess : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("奖牌图标")]
        public RawImage imgMedalIcon;
        [ALHeader("最大招待上限的提升")]
        public CommonUpgradePropertyShow<Text> txtMaxStamina;
        [ALHeader("升级提示")]
        public Text txtTip;
        [ALHeader("星级图标容器")]
        public GGUIMonoInnLevelIconContainer monoStarIconContainer;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6428); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6428); } }
    }
}