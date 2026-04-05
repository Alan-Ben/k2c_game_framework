using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 支持穿插展示，间隔加点的Grid
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    public abstract class _ATGGUIMonoAlternateBasicGrid<_T_ITEM_MONO> : _TALUGUIMonoGridWnd<_T_ITEM_MONO> 
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
    {
        [ALHeader("无物品提示")]
        public GameObject noneItemsTips;
        [ALHeader("间隔模板对象")]
        public GameObject spaceTemplate;
    }
}