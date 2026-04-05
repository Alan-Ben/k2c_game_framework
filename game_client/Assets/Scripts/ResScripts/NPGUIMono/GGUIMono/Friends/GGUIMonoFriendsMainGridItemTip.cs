using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 好友列表额外按钮tip
    /// </summary>
    public class GGUIMonoFriendsMainGridItemTip : NPGGUIMonoCommonToolTip
    {
        [ALHeader("删除按钮")]
        public GameObject deleteBtn;

        [ALHeader("屏蔽按钮")]
        public GameObject blockBtn;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2604); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2604);} }
    }
}