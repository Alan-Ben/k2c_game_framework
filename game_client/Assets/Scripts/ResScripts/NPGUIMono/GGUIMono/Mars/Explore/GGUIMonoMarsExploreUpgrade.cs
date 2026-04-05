using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreUpgrade : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("等级文本")]
        public Text txtCurLevel;
        public Text txtNextLevel;
        [ALHeader("事件品质列表")]
        public GGUIMonoMarsExploreEventQualityContainer monoCurEventQualityContainer;
        public GGUIMonoMarsExploreEventQualityContainer monoNextEventQualityContainer;
        [ALHeader("探索次数累积上限")]
        public Text txtCurExploreCountLimit;
        public Text txtNextExploreCountLimit;
        [ALHeader("升级奖励列表")]
        public NPGGUIMonoCommonItemContainer monoUpgradeRewardContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7406); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7406); } }
    }
}