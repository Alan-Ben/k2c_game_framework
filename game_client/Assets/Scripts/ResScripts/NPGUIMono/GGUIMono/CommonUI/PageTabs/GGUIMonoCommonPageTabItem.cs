using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public enum GGUIMonoCommonPageTabItemShowState
    {
        UNSELECTED,
        SELECTED,
    }
    public class GGUIMonoCommonPageTabItem<T> : _AALBasicUIWndMono 
        where T : Enum
    {
        [ALHeader("按钮类型")]
        public T tabType;
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
        [ALHeader("红点GO")]
        public GameObject goRedTip;
        [ALHeader("不同选中情况下显示的对象列表")]
        public MultiStateShow<GGUIMonoCommonPageTabItemShowState> selectShow;
    }
}