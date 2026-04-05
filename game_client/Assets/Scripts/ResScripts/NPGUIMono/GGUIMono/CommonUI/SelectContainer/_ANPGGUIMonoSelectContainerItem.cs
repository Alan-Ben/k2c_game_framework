using UnityEngine;
using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 选择滚动条ItemMono基类
    /// </summary>
    public abstract class _ANPGGUIMonoSelectContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("item点击范围")]
        public GameObject btnItem;
        [ALHeader("选中时显示")]
        public List<GameObject> showOnSelect;
        [ALHeader("选中时隐藏")]
        public List<GameObject> hideOnSelect;
    }
}
