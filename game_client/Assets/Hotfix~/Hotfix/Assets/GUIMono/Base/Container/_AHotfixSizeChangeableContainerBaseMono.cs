using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public class _AHotfixSizeChangeableContainerBaseMono : _AHotfixContainerBaseMono
    {
        [ALInfo("可根据itemContainer大小改变chgSizeRectTransform大小的container")]
        [Space(20)]
        [HotfixMono("需要改变大小的RectTransform")]
        public RectTransform chgSizeRectTransform;
        
        [HotfixMono("是否可以改变宽度")]
        public bool widthChangeable;
        [HotfixMono("宽度扩展值")]
        public float widthExpand;
        [HotfixMono("宽度最小限制范围(填-1为不限制)")]
        public int widthRangeMin = -1;
        [HotfixMono("宽度最大限制范围(填-1为不限制)")]
        public int widthRangeMax = -1;
        
        [HotfixMono("是否可以改变高度")]
        public bool heightChangeable;
        [HotfixMono("高度扩展值")]
        public float heightExpand;
        [HotfixMono("高度最小限制范围(填-1为不限制)")]
        public int heightRangeMin = -1;
        [HotfixMono("高度最大限制范围(填-1为不限制)")]
        public int heightRangeMax = -1;
        
        [HotfixMono("没有数据要显示时显示列表")]
        public List<GameObject> noItemShow;
    }
}