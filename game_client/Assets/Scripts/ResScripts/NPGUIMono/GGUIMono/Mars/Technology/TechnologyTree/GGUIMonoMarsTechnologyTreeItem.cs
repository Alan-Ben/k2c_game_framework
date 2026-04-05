using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 科技数item
    /// </summary>
    public class GGUIMonoMarsTechnologyTreeItem : _AALBasicUIWndMono
    {
        [ALHeader("科技信息子窗口")]
        public GGUIMonoMarsTechnologyInfo monoTechnologyInfo;
        
        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}