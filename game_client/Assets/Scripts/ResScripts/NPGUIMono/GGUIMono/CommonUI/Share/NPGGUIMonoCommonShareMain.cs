using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum ENPGGUICommonShareTabType
    {
        FRIEND,//好友
        ALLIANCE,//联盟
    }

    [System.Serializable]
    public struct NPGGUICommonShareTabParam
    {
        [ALHeader("页签类型")]
        public ENPGGUICommonShareTabType shareTabType;
        [ALHeader("页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("资源路径id")]
        public long assetPathId;
    }

    /// <summary>
    /// 机关分享界面
    /// </summary>
    public class NPGGUIMonoCommonShareMain : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页面加载父节点")]
        public Transform pageParent;
        [ALHeader("页签列表")]
        public List<NPGGUICommonShareTabParam> tabList;
        [ALHeader("默认选中的页签")]
        public ENPGGUICommonShareTabType defaultShareTab = ENPGGUICommonShareTabType.FRIEND;
        [ALHeader("标题文本")]
        public TextEx txtTitle;
        [ALHeader("每次私聊分享的共享cd（秒）")]
        public long perShareCD = 1;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_SHARE_MAIN); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_SHARE_MAIN); } }
    }
}
