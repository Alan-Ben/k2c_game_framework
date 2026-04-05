using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 子嗣分享弹窗
    /// </summary>
    public class GGUIMonoShareChild : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject closeBtn;
        [ALHeader("分享按钮")]
        public GameObject shareBtn;
        [ALHeader("子嗣信息卡片")]
        public GGUIMonoChildInfo monoCardItem;
        [ALHeader("子嗣列表")]
        public GGUIMonoShareIconItemGrid shareIconGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1323); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1323);} }
    }
}