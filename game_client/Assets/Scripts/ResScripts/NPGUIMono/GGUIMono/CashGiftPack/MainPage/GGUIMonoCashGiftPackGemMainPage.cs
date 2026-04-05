using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 钻石商店页面
    /// </summary>
    public class GGUIMonoCashGiftPackGemMainPage : _AALBasicUIWndMono
    {
        [ALHeader("VIP等级")]
        public Text txtVIPLevel;
        [ALHeader("VIP经验进度条")]
        public NPGGUIMonoProgress expProgress;
        [ALHeader("VIP升级描述")]
        public Text txtVIPUpgradeDesc;
        [ALHeader("VIP预览按钮")]
        public GameObject btnVIPPreview;
        [ALHeader("钻石商品列表")]
        public GGUIMonoCashGiftPackGemContainer monoGemContainer;
    }
}

