using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包界面钻石礼包页面
    /// </summary>
    public class GGUIMonoActivityCrystalGiftPackPage : _AALBasicUIWndMono
    {
        [ALHeader("刷新描述")]
        public Text txtRefreshDesc;
        [ALHeader("钻石礼包列表")]
        public GGUIMonoActivityCrystalGiftPackPageContainer monoCrystalGiftPackContainer;
    }
}
