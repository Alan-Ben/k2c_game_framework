using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会好友邀请item容器
    /// </summary>
    public class GGUIMonoDinnerInviteFriendItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoDinnerInviteFriendItem>
    {
        [ALHeader("列表为空的时候显示")]
        public List<GameObject> emptyShowList;
        [ALHeader("列表为空的时候隐藏")]
        public List<GameObject> emptyHideList;
    }
}
