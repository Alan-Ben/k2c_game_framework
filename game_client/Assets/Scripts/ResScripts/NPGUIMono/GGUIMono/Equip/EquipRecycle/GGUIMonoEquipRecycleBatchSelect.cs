using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 选择类型
    /// </summary>
    public enum EEquipRecycleBatchSelectTabType
    {
        NONE,
        [InspectorName("GREEN_AND_BELOW（绿色及以下）")]
        GREEN_AND_BELOW,
        [InspectorName("BLUE_AND_BELOW（蓝色及以下）")]
        BLUE_AND_BELOW,
        [InspectorName("PURPLE_AND_BELOW（紫色及以下）")]
        PURPLE_AND_BELOW,
        [InspectorName("ORANGE_AND_BELOW（橙色及以下）")]
        ORANGE_AND_BELOW,
    }

    /// <summary>
    /// 藏品分解选择类型页签
    /// </summary>
    [System.Serializable]
    public class GGUIMonoEquipRecycleBatchSelectTabMono
    {
        [Header("排序页签类型")]
        public EEquipRecycleBatchSelectTabType tabType;
        [Header("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
    }

    /// <summary>
    /// 藏品分解选择类型
    /// </summary>
    public class GGUIMonoEquipRecycleBatchSelect : _AALBasicUIWndMono
    {
        [ALHeader("选中隐藏切换控件")]
        public NPGGUIMonoCommonToggleEx monoToggle;
        [ALHeader("按钮的名称")]
        public Text txtButtonName;
        [ALHeader("页签列表")]
        public List<GGUIMonoEquipRecycleBatchSelectTabMono> monoSortTabList;
    }
}