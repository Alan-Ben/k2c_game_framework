using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFundPreview : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("基金横幅")]
        public RawImage imgBanner;
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("激活提示")]
        public Text txtActivateTip;
        [ALHeader("所有基础奖励列表")]
        public NPGGUIMonoCommonItemContainer monoFreeRewardContainer;
        [ALHeader("所有付费奖励列表")]
        public NPGGUIMonoCommonItemContainer monoPaidRewardContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8303); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8303);} }
    }
}
