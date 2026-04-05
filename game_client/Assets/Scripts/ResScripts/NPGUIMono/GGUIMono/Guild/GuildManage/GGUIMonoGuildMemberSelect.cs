using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟成员选择列表界面
    /// </summary>
    public class GGUIMonoGuildMemberSelect : _AALBasicUIWndMono
    {
        [ALHeader("联盟盟主信息")]
        public GGUIMonoGuildMemberListGridItem monoLeader;
        [ALHeader("联盟成员列表")]
        public GGUIMonoGuildMemberListGrid monoMemberListGrid;
        [ALHeader("联盟成员数量")]
        public Text txtMemberCount;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4913); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4913); } }
    }
}
