using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


/// <summary>
/// 分组编辑界面好友列表item
/// </summary>
public class GGUIMonoFriendGruopEditorFriendListGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("玩家头像")]
    public NPGGUIMonoPlayerIcon playerIcon;
    [ALHeader("选中tog")]
    public NPGGUIMonoCommonToggleEx selectedTog;
}
