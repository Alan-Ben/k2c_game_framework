using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴分享弹窗
    /// </summary>
    public class GGUIMonoShareHero : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject closeBtn;
        [ALHeader("分享按钮")]
        public GameObject shareBtn;
        [ALHeader("伙伴信息卡片")]
        public GGUIMonoHeroCommonCardItem monoHeroCardItem;
        [ALHeader("伙伴列表")]
        public GGUIMonoShareIconItemGrid shareIconGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1319); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1319);} }
    }
}