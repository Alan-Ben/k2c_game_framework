using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 好友推荐页面
    /// </summary>
    public class GGUIMonoFriendRecommendPage : _AALBasicUIWndMono
    {
        [ALHeader("搜索按钮")]
        public GameObject btnSearch;
        [ALHeader("清空搜索按钮")]
        public GameObject btnClearSearch;
        [ALHeader("搜索输入框")]
        public InputField inputSearch;
        [ALHeader("刷新推荐列表按钮")]
        public GameObject btnRefreshRecommend;
        [ALHeader("推荐或者搜索得到的好友列表")]
        public GGUIMonoFriendAddFriendListGrid friendListGrid;
        [ALHeader("搜索好友的时候显示的列表，否则隐藏")]
        public List<GameObject> searchShowList;
        [ALHeader("推荐好友的时候显示的列表，否则隐藏")]
        public List<GameObject> recommendShowList;
        

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1317); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1317); } }
    }
}