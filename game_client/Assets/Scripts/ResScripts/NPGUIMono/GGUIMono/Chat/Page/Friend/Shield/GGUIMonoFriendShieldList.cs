using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 屏蔽列表
    /// </summary>
    public class GGUIMonoFriendShieldList : _AALBasicUIWndMono
    {
        [ALHeader("屏蔽列表")]
        public GGUIMonoFriendShieldGrid friendListGrid;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("屏蔽数量文本")]
        public TextEx txtShieldCount;
        

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1362); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1362);} }
    }
}