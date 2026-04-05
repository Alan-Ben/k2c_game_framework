using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum ELimitActivityTabType
    {
        [InspectorName("CROSS_RANK_RUSH（跨服冲榜）")]
        CROSS_RANK_RUSH,
        [InspectorName("RANK_RUSH（本服冲榜）")]
        RANK_RUSH,
        [InspectorName("STEP_REWARD（阶段奖励-限时任务）")]
        STEP_REWARD,
    }

    /// <summary>
    /// 限时活动主界面页签
    /// </summary>
    [System.Serializable]
    public class GGUILimitActivityTabMono
    {
        [ALHeader("页签类型")]
        public ELimitActivityTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 限时活动主界面
    /// </summary>
    public class GGUIMonoLimitActivityMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public List<GGUILimitActivityTabMono> monoTabList;
        [ALHeader("页面父节点")]
        public Transform pageParent;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3900); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3900); } }
    }
}
