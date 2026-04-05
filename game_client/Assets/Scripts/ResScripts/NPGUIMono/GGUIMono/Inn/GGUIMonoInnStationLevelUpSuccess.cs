using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnStationLevelUpSuccess : _AALBasicUIWndMono
    {
        [ALHeader("设施图片")]
        public RawImage imgIcon;
        [ALHeader("设施名字")]
        public Text txtName;
        [ALHeader("等级变化")]
        public CommonUpgradePropertyShow<Text> levelUpgradeShow;
        [ALHeader("人气值变化")]
        public CommonUpgradePropertyShow<Text> popularityUpgradeShow;
        [ALHeader("熟练度变化")]
        public CommonUpgradePropertyShow<Text> finesseUpgradeShow;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6421); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6421); } }
    }
}