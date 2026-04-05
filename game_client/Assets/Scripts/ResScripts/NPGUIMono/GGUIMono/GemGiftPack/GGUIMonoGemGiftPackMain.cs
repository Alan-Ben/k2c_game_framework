using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EGemGiftPackMainTabType
    {
        [InspectorName("ACTIVITY_GEM（活动钻石礼包）")]
        ACTIVITY_GEM,
    }

    /// <summary>
    /// 页签配置
    /// </summary>
    [System.Serializable]
    public class GGUIGemGiftPackMainTabMono
    {
        [ALHeader("页签类型")]
        public EGemGiftPackMainTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("资源id")]
        public long resId;
    }

    /// <summary>
    /// 钻石礼包主界面
    /// </summary>
    public class GGUIMonoGemGiftPackMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("默认页签")]
        public EGemGiftPackMainTabType defaultTab;
        [ALHeader("页签列表")]
        public List<GGUIGemGiftPackMainTabMono> monoTabList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6751); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6751); } }
    }
}
