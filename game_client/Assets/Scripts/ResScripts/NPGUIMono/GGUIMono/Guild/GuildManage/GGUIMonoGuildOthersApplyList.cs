using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 其他人申请加入联盟的申请列表界面
    /// </summary>
    public class GGUIMonoGuildOthersApplyList : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("成员数量")]
        public Text txtMemberCount;
        [ALHeader("申请列表")]
        public GGUIMonoGuildOthersApplyListGrid monoApplyListGrid;
        [ALHeader("一键拒绝")]
        public GameObject btnOneKeyRefuse;
        [ALHeader("一键同意")]
        public GameObject btnOneKeyAgree;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4908); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4908); } }
    }
}
