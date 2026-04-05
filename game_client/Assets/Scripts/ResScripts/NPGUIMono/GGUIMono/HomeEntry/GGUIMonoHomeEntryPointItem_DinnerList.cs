using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会列表的操作点击窗口，纯单按钮点击
    /// </summary>
    public class GGUIMonoHomeEntryPointItem_DinnerList : _AGGUIMonoEntryPointFollowItemBase
    {
        [ALHeader("当前有其它玩家举办宴会的时候显示，否则隐藏")]
        public List<GameObject> hasOtherDinnerShow;
    }
}