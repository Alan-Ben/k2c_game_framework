using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用下拉框Item
    /// </summary>
    public class NPGGUIMonoDropDownBoxItem : _AALBasicUIWndMono
    {
        [ALHeader("显示的内容文本")]
        public TextEx txtContent;
        [ALHeader("显示的图片")]
        public RawImage imgIcon;
        [ALHeader("点击选择按钮")]
        public GameObject btnSelect;
        [ALHeader("选中需要显示的GO")]
        public List<GameObject> selectedShowList;
    }
}
