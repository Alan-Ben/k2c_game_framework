using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 规则二级列表item
    /// </summary>
    public class NPGGUIMonoRuleListSubItem : _AALBasicUIWndMono
    {
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        [ALHeader("名字")]
        public TextEx txtName;
    }
}
