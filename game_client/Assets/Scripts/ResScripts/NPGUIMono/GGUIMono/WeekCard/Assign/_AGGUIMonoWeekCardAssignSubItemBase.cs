using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 委派列表item-基类
    /// </summary>
    public class _AGGUIMonoWeekCardAssignSubItemBase : _AALBasicUIWndMono
    {
        [ALHeader("名字")]
        public TextEx txtName;
        [ALHeader("解锁状态配置")]
        public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> statInfos;
    }
}
