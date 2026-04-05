using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟信息界面
    /// </summary>
    public class GGUIMonoGuildSelfInfo : _AALBasicUIWndMono
    {
        [ALHeader("联盟基础信息")]
        public GGUIMonoGuildSubBaseInfo monoGuildBaseInfo;
        [ALHeader("更换旗帜按钮")]
        public GameObject btnChgFlag;
        [ALHeader("修改名称")]
        public GameObject btnChgName;
        [ALHeader("盟主信息按钮")]
        public GameObject btnLeaderInfo;
        [ALHeader("盟主个人信息卡片定位用transform")]
        public RectTransform leaderInfoLocateRectTrans;
        [ALHeader("输入宣言")]
        public InputField inputDeclaration;
        [ALHeader("输入宣言字数")]
        public Text txtInputDeclarationCount;
        [ALHeader("输入公告")]
        public InputField inputAnnouncement;
        [ALHeader("输入公告字数")]
        public Text txtInputAnnouncementCount;
        [ALHeader("招募CD文本")]
        public NPGGUIMonoCommonCountDown monoRecruitCD;
        [ALHeader("公开招募按钮")]
        public GameObject btnPublicRecruit;
        [ALHeader("遣散按钮")]
        public GameObject btnDismiss;
        [ALHeader("加入条件按钮")]
        public GameObject btnJoinCondition;
        [ALHeader("联盟申请按钮")]
        public GameObject btnApply;
        [ALHeader("联盟日志按钮")]
        public GameObject btnLog;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4905); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4905); } }
    }
}
