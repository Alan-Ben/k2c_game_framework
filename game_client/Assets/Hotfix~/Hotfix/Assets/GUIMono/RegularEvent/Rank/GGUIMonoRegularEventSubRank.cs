using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动附加排行榜页签类型
    /// </summary>
    public class RegularEventRankTabType
    {
        public const string SELF_RANK = "SELF_RANK"; //本服
        public const string CROSS_RANK = "CROSS_RANK"; //跨服
    }

    /// <summary>
    /// 万能活动附加排行榜界面
    /// </summary>
    public class GGUIMonoRegularEventSubRank : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("默认选中页签（SELF_RANK、CROSS_RANK）")]
        public string defaultTab = RegularEventRankTabType.SELF_RANK;
        [HotfixMonoAttribute("页签列表")]
        public List<GGUIMonoActivityCommonTab> monoTabList;
        [HotfixMonoAttribute("页面父节点")]
        public RectTransform pageParent;
    }
}