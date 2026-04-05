using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包界面现金礼包页面
    /// </summary>
    public class GGUIMonoActivityCashGiftPackPage : _AALBasicUIWndMono
    {
        [ALHeader("礼包名称")]
        public Text txtGiftPackName;
        [ALHeader("礼包横幅图片")]
        public RawImage imgBanner;
        [ALHeader("刷新描述")]
        public Text txtRefreshDesc;
        [ALHeader("现金礼包列表")]
        public GGUIMonoCashGiftPackPageGoodsContainer monoCrystalGiftPackContainer;
    }
}
