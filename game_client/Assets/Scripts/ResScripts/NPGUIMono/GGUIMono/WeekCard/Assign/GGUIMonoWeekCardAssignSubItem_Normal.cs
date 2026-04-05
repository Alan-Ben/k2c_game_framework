using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 委派列表item
    /// </summary>
    public class GGUIMonoWeekCardAssignSubItem_Normal : _AGGUIMonoWeekCardAssignSubItemBase
    {
        [ALHeader("结束才处理的开关")]
        public NPGGUIMonoCommonToggleEx togDoneDeal;
        [ALHeader("描述")]
        public TextEx txtDesc;
    }
}
