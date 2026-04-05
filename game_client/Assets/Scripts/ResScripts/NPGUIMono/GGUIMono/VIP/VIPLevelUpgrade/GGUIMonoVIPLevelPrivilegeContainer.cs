using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// VIP等级特权描述列表
    /// </summary>
    public class GGUIMonoVIPLevelPrivilegeContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoVIPLevelPrivilegeContainerItem>
    {
        [ALHeader("列表为空时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("列表为空时隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}
