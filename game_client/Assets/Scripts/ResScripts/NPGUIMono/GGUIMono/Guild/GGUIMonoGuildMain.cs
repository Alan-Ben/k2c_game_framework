using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟公告展开动画类型
    /// </summary>
    public enum EGuildAnnouncementExpandAniType
    {
        [InspectorName("EXPAND（展开）")]
        EXPAND,
        [InspectorName("CLOSE（关闭）")]
        CLOSE,
    }

    /// <summary>
    /// 联盟主界面
    /// </summary>
    public class GGUIMonoGuildMain : _AALBasicUIWndMono
    {
        [ALHeader("联盟基础信息")]
        public GGUIMonoGuildSubBaseInfo monoBaseInfo;
        [ALHeader("联盟等级预览按钮")]
        public GameObject btnLevelPreview;
        [ALHeader("联盟公告")]
        public Text txtAnnouncement;
        [ALHeader("展开关闭联盟公告按钮")]
        public GameObject btnExpandAnnouncement;
        [ALHeader("联盟公告展开动画列表")]
        public CommonAnimationShowTypeInfo<EGuildAnnouncementExpandAniType> aniAnnouncement;
        [ALHeader("盟主形象显示")]
        public GGUIMonoCommonShowCase monoLeaderShowcase;
        [ALHeader("联盟信息按钮")]
        public GameObject btnGuildInfo;
        [ALHeader("联盟建设按钮")]
        public GameObject btnConstruct;
        [ALHeader("联盟成员按钮")]
        public GameObject btnMember;
        // [ALHeader("联盟商店按钮")]
        // public GameObject btnShop;
        [ALHeader("联盟排行按钮")]
        public GameObject btnRank;
        [ALHeader("联盟委托按钮")]
        public GameObject btnEntrust;
        [ALHeader("联盟礼物按钮")]
        public GameObject btnGuildGift;
        [ALHeader("联盟派遣按钮")]
        public GameObject btnGuildDispatch;
        [ALHeader("联盟协作按钮")]
        public GameObject btnGuildCooperate;
        [ALHeader("互助按钮")]
        public GameObject btnHelp;
        [ALHeader("聊天入口小窗")]
        public NPGGUIMonoMiniChat chatMiniWndMono;
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4903); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4903); } }
    }
}
