using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 运营公告弹窗
    /// </summary>
    public class GGUIMonoAnnouncement : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("标题")]
        public TextEx txtTitle;
        [ALHeader("内容")]
        public TextEx txtContent;
        [ALHeader("内容文本的滚动")]
        public ScrollRect scContent;
        [ALHeader("公告图片")]
        public RawImage imgBanner;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("我知道了按钮")]
        public GameObject btnIKnow;
        [ALHeader("页签列表")]
        public GGUIMonoAnnouncementTabContainer consortItemContainer;
        [ALHeader("页签页码")]
        public TextEx txtTabPage;
        [ALHeader("外链跳转需要显示的GO")]
        public List<GameObject> webUrlJumpShowList;
        [ALHeader("游戏内跳转需要显示的GO")]
        public List<GameObject> gameInsideJumpShowList;
        [ALHeader("无跳转配置需要显示的GO")]
        public List<GameObject> noJumpShowList;
        [ALHeader("没有公告需要显示的GO")]
        public List<GameObject> noAnnouncementShowList;
        [ALHeader("没有公告需要隐藏的GO")]
        public List<GameObject> noAnnouncementHideList;

        /************
        * 资源加载路径
        */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4700); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4700);} }
    }
}