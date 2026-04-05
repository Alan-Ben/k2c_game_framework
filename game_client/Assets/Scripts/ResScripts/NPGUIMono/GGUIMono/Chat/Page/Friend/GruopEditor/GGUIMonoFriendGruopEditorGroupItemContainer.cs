using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

//好友列表分组编辑的分组item 容器
public class GGUIMonoFriendGruopEditorGroupItemContainer:_ATNPGGUIMonoShowAnimContainer<GGUIMonoFriendGruopEditorGroupItem>
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("空列表的时候显示")]
    public List<GameObject> emptyShow;
}