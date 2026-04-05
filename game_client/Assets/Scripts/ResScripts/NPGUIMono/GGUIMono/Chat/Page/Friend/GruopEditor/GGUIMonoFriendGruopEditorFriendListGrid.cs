using System.Collections.Generic;
using ALPackage;
using UnityEngine;


//分组编辑界面好友列表grid
public class GGUIMonoFriendGruopEditorFriendListGrid : _TALUGUIMonoGridWnd<GGUIMonoFriendGruopEditorFriendListGridItem>
{
    [ALHeader("空列表显示")]
    public List<GameObject> emptyShow;
}
