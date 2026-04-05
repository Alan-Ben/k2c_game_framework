using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 属性子窗体
    /// </summary>
    public class GGUIMonoCommonAttrItem : _AALBasicUIWndMono
    {
        [ALHeader("属性图标")]
        public RawImage imgAttrIcon;
        [ALHeader("属性数值")]
        public Text txtAttrValue;
        [ALHeader("属性名字")]
        public Text txtAttrName;
        [ALHeader("属性描述按钮")]
        public GameObject btnDetail;
        [ALHeader("属性描述弹窗跟随偏移量")]
        public float attrToolTipInterval;
    }
}
