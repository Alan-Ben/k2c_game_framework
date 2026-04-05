using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 现金礼包主页面
    /// </summary>
    public class GGUIMonoCashGiftPackMainPage : _AALBasicUIWndMono
    {
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("页签列表")]
        public GGUIMonoCashGiftPackMainPageTabContainer monoTabContainer;
    }
}

