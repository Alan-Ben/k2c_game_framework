using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 排序类型
    /// </summary>
    public enum EHeroListSortTabType
    {
        [InspectorName("DEFAULT（默认）")]
        DEFAULT,
        [InspectorName("POWER（实力）")]
        POWER,
        [InspectorName("TALENT（资质）")]
        TALENT,
        [InspectorName("STAR（觉醒）")]
        STAR,
    }

    /// <summary>
    /// 大臣列表排序类型页签
    /// </summary>
    [System.Serializable]
    public class GGUIHeroMainSortTabMono
    {
        [Header("排序页签类型")]
        public EHeroListSortTabType tabType;
        [Header("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
    }

    public class GGUIMonoHeroMainSort : _AALBasicUIWndMono
    {
        [ALHeader("选中隐藏切换控件")]
        public NPGGUIMonoCommonToggleEx monoToggle;
        [ALHeader("按钮的名称")]
        public Text txtButtonName;
        [ALHeader("排序页签列表")]
        public List<GGUIHeroMainSortTabMono> monoSortTabList;
    }
}