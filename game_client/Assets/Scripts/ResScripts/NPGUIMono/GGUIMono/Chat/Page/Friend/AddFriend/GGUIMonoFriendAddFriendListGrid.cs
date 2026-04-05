using System.Collections.Generic;
using ALPackage;
using UnityEngine;


//添加编辑界面好友列表grid
public class GGUIMonoFriendAddFriendListGrid : _ATNPGGUIMonoShowAnimGrid <GGUIMonoFriendAddFriendListGridItem>
{
    [ALHeader("空列表显示")]
    public List<GameObject> emptyShow;
}
