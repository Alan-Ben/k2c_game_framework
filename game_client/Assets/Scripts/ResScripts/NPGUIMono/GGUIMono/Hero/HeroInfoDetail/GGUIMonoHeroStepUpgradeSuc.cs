using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴升阶成功弹窗
    /// </summary>
    public class GGUIMonoHeroStepUpgradeSuc : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("卡牌信息")]
        public GGUIMonoHeroCommonCardItem monoCardItem;
        [ALHeader("上个等级上限")]
        public Text txtLastLevelLimit;
        [ALHeader("当前等级上限")]
        public Text txtCurLevelLimit;
        [ALHeader("上个资质")]
        public Text txtLastTalent;
        [ALHeader("当前资质")]
        public Text txtCurTalent;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1005); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1005); } }
    }
}