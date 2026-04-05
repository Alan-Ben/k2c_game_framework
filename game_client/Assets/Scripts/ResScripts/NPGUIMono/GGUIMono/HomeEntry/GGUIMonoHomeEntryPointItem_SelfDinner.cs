using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 我的宴会的操作点击窗口，纯单按钮点击
    /// </summary>
    public class GGUIMonoHomeEntryPointItem_SelfDinner : _AGGUIMonoEntryPointFollowItemBase
    {
        [ALHeader("当前玩家有举办宴会的时候显示，否则隐藏")]
        public List<GameObject> hasStartDinnnerShow;
    }
}