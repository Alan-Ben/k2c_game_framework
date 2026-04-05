using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟成员列表界面
    /// </summary>
    public class GGUIMonoGuildMemberList : _AALBasicUIWndMono
    {
        [ALHeader("联盟盟主信息")]
        public GGUIMonoGuildMemberListGridItem monoLeader;
        [ALHeader("联盟成员列表")]
        public GGUIMonoGuildMemberListGrid monoMemberListGrid;
        [ALHeader("联盟成员数量")]
        public Text txtMemberCount;
        [ALHeader("通知按钮")]
        public GameObject btnNotice;
        [ALHeader("退出联盟按钮")]
        public GameObject btnQuit;
        [ALHeader("弹劾按钮")]
        public GameObject btnImpeach;
        [ALHeader("弹劾倒计时")]
        public NPGGUIMonoCommonCountDown monoImpeachCD;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4911); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4911); } }
    }
}
