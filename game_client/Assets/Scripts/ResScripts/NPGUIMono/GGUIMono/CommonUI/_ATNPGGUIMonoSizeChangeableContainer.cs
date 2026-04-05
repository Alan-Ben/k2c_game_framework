using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    public class _ATNPGGUIMonoSizeChangeableContainer<_T_ITEM_MONO> : _TALUGUIMonoContainerWnd<_T_ITEM_MONO> where _T_ITEM_MONO : _AALBasicUIWndMono
    {
        [ALInfo("可根据itemContainer大小改变chgSizeRectTransform大小的container")]
        [Space(20)]
        [ALHeader("需要改变大小的RectTransform")]
        public RectTransform chgSizeRectTransform;
        
        [ALHeader("是否可以改变宽度")]
        public bool widthChangeable;
        [ALHeader("宽度扩展值")]
        public float widthExpand;
        [ALHeader("宽度限制范围(填-1为不限制)")]
        public Range widthRange = new Range(-1, -1);
        
        [ALHeader("是否可以改变高度")]
        public bool heightChangeable;
        [ALHeader("高度扩展值")]
        public float heightExpand;
        [ALHeader("高度限制范围(填-1为不限制)")]
        public Range heightRange = new Range(-1, -1);
        
        [ALHeader("没有数据要显示时显示列表")]
        public List<GameObject> noItemShow;
    }
}