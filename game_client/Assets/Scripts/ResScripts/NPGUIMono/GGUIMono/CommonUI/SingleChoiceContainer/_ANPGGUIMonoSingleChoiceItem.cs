using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 单选列表item基类
    /// </summary>
    public class _ANPGGUIMonoSingleChoiceItem : _AALBasicUIWndMono
    {
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
        [ALHeader("选中时 显示的物体")]
        public List<GameObject> goListShowOnSelect;
        [ALHeader("选中时 隐藏的物体")]
        public List<GameObject> goListHideOnSelect;
    }
}
