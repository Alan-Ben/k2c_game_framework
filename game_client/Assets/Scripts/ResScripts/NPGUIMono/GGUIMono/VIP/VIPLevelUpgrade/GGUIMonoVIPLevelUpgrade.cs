using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP等级升级界面
    /// </summary>
    public class GGUIMonoVIPLevelUpgrade : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("VIP等级文本")]
        public Text txtVIPLevel;
        [ALHeader("VIP特权标题")]
        public Text txtVIPPrivilegeTitle;
        [ALHeader("VIP特权列表")]
        public GGUIMonoVIPLevelPrivilegeContainer monoPrivilegeContainer;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8102); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8102); } }
    }

}
