using ALPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 礼包组详情页面-基础
    /// </summary>
    public class GGUIMonoCashGiftPackGroupDetailPage : _AALBasicUIWndMono
    {
        [ALHeader("礼包名称父节点")]
        public Transform giftPackNameParent;
        [ALHeader("剩余时间倒计时")]
        public Text txtLeftTime;
        [ALHeader("礼包横幅图片")]
        public RawImage imgBanner;
        [ALHeader("限购次数刷新描述")]
        public Text txtRefreshDesc;
    }
}
