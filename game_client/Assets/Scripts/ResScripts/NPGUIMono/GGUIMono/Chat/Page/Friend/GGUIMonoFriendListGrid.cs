using System.Collections.Generic;
using ALPackage;
using UnityEngine;


//好友列表grid
public class GGUIMonoFriendListGrid : _TALUGUIMonoGridWnd<GGUIMonoFriendListGridItem>
{
    [ALHeader("空列表显示的内容")]
    public List<GameObject> emptyShowList;
}
