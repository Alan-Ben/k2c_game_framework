using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 屏蔽列表item
/// </summary>
public class GGUIMonoFriendShieldGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("玩家头像")]
    public NPGGUIMonoPlayerIcon playerIcon;
    [ALHeader("取消屏蔽按钮")]
    public GameObject btnRemove;
}
