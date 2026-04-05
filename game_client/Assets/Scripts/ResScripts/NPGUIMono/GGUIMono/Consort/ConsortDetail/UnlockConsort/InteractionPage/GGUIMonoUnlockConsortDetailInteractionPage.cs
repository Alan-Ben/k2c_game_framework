using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面互动page
    /// </summary>
    public class GGUIMonoUnlockConsortDetailInteractionPage : _AGGUIMonoUnLockConsortDetailTabPage
    {
        [ALHeader("页签子窗口")]
        public GGUIMonoUnlockConsortDetailInteractionPageTabWnd monoTab;

        [ALHeader("伙伴皮肤按钮item")]
        public GGUIMonoConsortSkinBtnItem monoConsortSkinItemBtn;

        [ALHeader("故事按钮")]
        public GameObject btnStory;
    }
}