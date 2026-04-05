using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟通知编辑界面
    /// </summary>
    public class GGUIMonoGuildNotice : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("选择成员按钮")]
        public GameObject btnSelectMember;
        [ALHeader("选中的联盟成员数量")]
        public Text txtMemberCount;
        [ALHeader("输入的字数")]
        public Text txtMessageCount;
        [ALHeader("输入框")]
        public InputField inputMessage;
        [ALHeader("免费次数描述")]
        public Text txtFreeCount;
        [ALHeader("免费次数用完后消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("发送按钮")]
        public GameObject btnSend;
        [ALHeader("有免费次数时需要展示的GO列表")]
        public List<GameObject> goHaveFreeCountShowList;
        [ALHeader("有免费次数时需要隐藏的GO列表")]
        public List<GameObject> goHaveFreeCountHideList;
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4912); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4912); } }
    }
}
