using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技加成 属性item
    /// </summary>
    public class GGUIMonoMasrTechnologyAddOverviewPropertyItem : _TALUGUIMonoGridItem
    {
        [ALHeader("属性展示")]
        public GGUIMonoCommonPropertyShow propertyShow;
        
        [ALHeader("在列表中不同index显示物体, 例如:" +
                  "列表里配置了两个, 则在item列表中, 第一个显示inListCyclicIndexShowGoList[0], 第二个显示inListCyclicIndexShowGoList[1], 第三个显示inListCyclicIndexShowGoList[0], 以此类推")]
        public List<GameObject> inListCyclicIndexShowGoList;
    }
}