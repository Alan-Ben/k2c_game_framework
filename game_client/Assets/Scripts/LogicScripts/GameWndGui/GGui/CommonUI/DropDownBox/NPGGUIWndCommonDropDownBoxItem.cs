using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用下拉框Item
    /// </summary>
    public class NPGGUIWndCommonDropDownBoxItem : _ATNPGGUIWndDropDownBoxItem<NPGGUIMonoDropDownBoxItem>
    {
        public NPGGUIWndCommonDropDownBoxItem(NPGGUIMonoDropDownBoxItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }
    
    }
}
