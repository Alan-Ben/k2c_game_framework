using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用下拉框Container
    /// </summary>
    public class NPGGUIMonoDropDownBoxContainer : _TALUGUIMonoContainerWnd<NPGGUIMonoDropDownBoxItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}

