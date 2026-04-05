using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用成就窗口
    /// </summary>
    public class GGUIMonoCommonAchieve : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("成就列表")]
        public GGUIMonoAchieveGrid monoAchieveGrid;
    }
}
