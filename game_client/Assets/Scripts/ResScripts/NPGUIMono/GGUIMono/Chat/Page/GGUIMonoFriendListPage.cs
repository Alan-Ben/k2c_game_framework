using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 好友列表
    /// </summary>
    public class GGUIMonoFriendListPage : _AALBasicUIWndMono
    {
        [ALHeader("好友列表")]
        public GGUIMonoFriendListGrid friendListGrid;
        [ALHeader("好友数量")]
        public TextEx txtFriendCount;
        [ALHeader("添加分组按钮")]
        public GameObject btnAddGroup;
        [ALHeader("分组排序按钮")]
        public GameObject btnSortGroup;
        [ALHeader("添加好友按钮")]
        public GameObject btnAddFriend;
        [ALHeader("屏蔽列表按钮")]
        public GameObject btnShield;
        [ALHeader("组排序container")]
        public GGUIMonoFriendGroupSortItemContainer groupSortItemContainer;
        [ALHeader("排序过程中显示的列表，否则隐藏")]
        public List<GameObject> onSortShow;
        [ALHeader("排序过程中隐藏的列表，否则显示")]
        public List<GameObject> onSortHide;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1313); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1313);} }
    }
}