using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoCommonPageTabList<T, Y> : _AALBasicUIWndMono 
        where T : Enum
        where Y : GGUIMonoCommonPageTabItem<T>
    {
        [ALHeader("页面加载出来的父节点")]
        public Transform transPageParent;
        [ALHeader("默认选中的按钮类型")]
        public T defaultTabType;
        [ALHeader("页签按钮")]
        public List<Y> pageTabs;
    }
}