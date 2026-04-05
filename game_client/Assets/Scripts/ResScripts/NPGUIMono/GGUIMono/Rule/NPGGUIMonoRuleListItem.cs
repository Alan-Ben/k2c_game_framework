using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 规则列表item
    /// </summary>
    public class NPGGUIMonoRuleListItem : _TALUGUIMonoGridItem
    {
        [ALHeader("展开按钮")]
        public NPGGUIMonoCommonToggleEx btnToggle;
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        [ALHeader("名字")]
        public TextEx txtName;
        [ALHeader("图标")]
        public RawImage texIcon;
    }
}
