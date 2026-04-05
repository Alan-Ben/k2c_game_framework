using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    //页签类型
    public enum EMarsCashGiftPackMainTabType
    {
        [InspectorName("NONE 无效")]
        NONE,
        [InspectorName("CONSTRUCTION_QUEUE（建造队列礼包）")]
        CONSTRUCTION_QUEUE,
        [InspectorName("ARMY_QUEUE（行军队列礼包）")]
        ARMY_QUEUE,
        [InspectorName("PERMANENT（常驻礼包）")]
        PERMANENT,
    }
    
    /// <summary>
    /// 页签配置
    /// </summary>
    [System.Serializable]
    public class GGUIMarsCashGiftPackMainTabMono
    {
        [ALHeader("页签类型")]
        public EMarsCashGiftPackMainTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("资源id")]
        public long resId;
        [ALHeader("页签红点")]
        public NPGGUICustomMonoRedTipWnd redTip;
    }

    /// <summary>
    /// 现金礼包主界面
    /// </summary>
    public class GGUIMonoMarsCashGiftPackMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签滚动组件")]
        public ScrollRect tabScrollRect;
        [ALHeader("页签content")]
        public RectTransform tabContentTransform;
        [ALHeader("页签额外向内移动距离参数")]
        public MoveItemAdditionDistanceParam moveItemAdditionDistanceParam;
        [ALHeader("页签侧边红点提示信息")]
        public ContainerSideRedTipInfo sideRedTipInfo;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("建筑队列礼包id")]
        public long constructionQueueGiftPackId;
        [ALHeader("行军队列礼包id")]
        public long armyQueueGiftPackId;
        [ALHeader("页签列表")]
        public List<GGUIMarsCashGiftPackMainTabMono> monoTabList;
        [ALHeader("没礼包时候展示的go列表")]
        public List<GameObject> emptyShowGoList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6761); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6761); } }
    }
}