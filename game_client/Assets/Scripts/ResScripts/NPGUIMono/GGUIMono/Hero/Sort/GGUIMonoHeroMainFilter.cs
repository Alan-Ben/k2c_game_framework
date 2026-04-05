using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
namespace GOE
{
    /// <summary>
    /// 伙伴列表筛选类型页签
    /// </summary>
    [System.Serializable]
    public class GGUIHeroMainFilterTabMono
    {
        [Header("排序页签类型")]
        public ESpecAttrType tabType;
        [Header("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("标签GO")]
        public GameObject goTag;
    }

    /// <summary>
    /// 伙伴列表筛选子窗口
    /// </summary>
    public class GGUIMonoHeroMainFilter : _AALBasicUIWndMono
    {
        [ALHeader("排序页签列表")]
        public List<GGUIHeroMainFilterTabMono> monoFilterTabList;
    }
}