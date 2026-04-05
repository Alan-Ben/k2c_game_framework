
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金主页面
    /// </summary>
    public class GGUIMonoFundPage : _AALBasicUIWndMono
    {
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("页签列表")]
        public GGUIMonoFundTabContainer monoTabContainer;
    }
}