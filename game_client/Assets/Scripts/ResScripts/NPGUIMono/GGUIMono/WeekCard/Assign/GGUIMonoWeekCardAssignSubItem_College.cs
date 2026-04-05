using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 委派列表item--大学
    /// </summary>
    public class GGUIMonoWeekCardAssignSubItem_College : _AGGUIMonoWeekCardAssignSubItemBase
    {
        [ALHeader("选中开关")]
        public NPGGUIMonoCommonToggleEx togSelected;
        [ALHeader("委派数量")]
        public TextEx txtAssignCount;
        [ALHeader("选择骑士按钮")]
        public GameObject btnSelectedHero;
    }
}
