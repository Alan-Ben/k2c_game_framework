using ALPackage;
using UnityEngine;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 通用下拉框
    /// </summary>
    public class NPGGUIMonoDropDownBox : _AALBasicUIWndMono
    {
        [ALHeader("当前显示")]
        public TextEx txtContent;
        
        [ALHeader("选择tab")]
        public NPGGUIMonoCommonTab monoCommonTab;
        
        [ALHeader("下拉容器")]
        public NPGGUIMonoDropDownBoxContainer monoDropDownBoxContainer;
        
        [ALHeader("默认是否展开")]
        public bool defaultIsSelect;
    }

}