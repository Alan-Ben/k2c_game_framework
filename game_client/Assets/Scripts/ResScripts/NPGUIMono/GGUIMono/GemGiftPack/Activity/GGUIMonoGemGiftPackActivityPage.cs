using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 活动钻石礼包主页面
    /// </summary>
    public class GGUIMonoGemGiftPackActivityPage : _AALBasicUIWndMono
    {
        [ALHeader("剩余时间倒计时")]
        public Text txtLeftTime;
        [ALHeader("页面父节点")]
        public Transform pageParent;
        [ALHeader("页签列表")]
        public GGUIMonoGemGiftPackActivityPageTabContainer monoTabContainer;
        [ALHeader("没有活动时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("没有活动时隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}
