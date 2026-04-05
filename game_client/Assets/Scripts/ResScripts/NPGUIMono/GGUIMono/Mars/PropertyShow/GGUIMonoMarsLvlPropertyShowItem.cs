using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技等级属性展示Item
    /// </summary>
    public class GGUIMonoMarsLvlPropertyShowItem : _TALUGUIMonoGridItem
    {
        [ALHeader("等级文本")]
        public TextEx txtLvl;
        [ALHeader("等级Key")]
        public string txtLvlKey;

        [ALHeader("在列表中不同index显示物体, 例如:" +
                  "列表里配置了两个, 则在item列表中, 第一个显示inListCyclicIndexShowGoList[0], 第二个显示inListCyclicIndexShowGoList[1], 第三个显示inListCyclicIndexShowGoList[0], 以此类推")]
        public List<GameObject> inListCyclicIndexShowGoList;
        
        [ALHeader("属性值Item加载父物体")]
        public Transform propertyValueItemParent;
        
        [ALHeader("属性值预制体")]
        public NPGGUIMonoCommonTextItem monoPropertyValueItemPrefab;

        [ALHeader("当前等级显示物体列表")]
        public List<GameObject> curLvlShow;
        [ALHeader("当前等级隐藏物体列表")]
        public List<GameObject> curLvlHide;

        [ALHeader("是否需要改变等级文本颜色")]
        public bool needChgLvlColor;
        [ALHeader("当前等级文本颜色配置")]
        public Color curLvlTxtColor;
        [ALHeader("其他等级文本颜色配置")]
        public Color otherLvlTxtColor;
    }
}