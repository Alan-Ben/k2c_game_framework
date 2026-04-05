using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoTreasureHuntCatalogTabWnd : _ANPBasicUIWndResBarMono
    {
        [ALHeader("选择tab的banner")]
        public RawImage selectedTabImgBanner;
        [ALHeader("选择tab的名称")]
        public TextEx txtSelectedTabName;

        [ALHeader("tab列表")]
        public GGUIMonoTreasureHuntCatalogTabContainer tabContainer;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
    }
}