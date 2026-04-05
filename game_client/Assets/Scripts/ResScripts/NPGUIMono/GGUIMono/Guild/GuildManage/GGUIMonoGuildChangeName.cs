using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟修改名称界面
    /// </summary>
    public class GGUIMonoGuildChangeName : _AALBasicUIWndMono
    {
        [ALHeader("输入名称")]
        public InputField inputName;
        [ALHeader("输入名称字数")]
        public Text txtNameCount;
        [ALHeader("输入名称提示")]
        public Text txtNameTip;
        [ALHeader("输入简称")]
        public InputField inputSimpleName;
        [ALHeader("输入简称字数")]
        public Text txtSimpleNameCount;
        [ALHeader("输入简称提示")]
        public Text txtSimpleNameTip;
        [ALHeader("取消按钮")]
        public GameObject btnCancel;
        [ALHeader("修改按钮")]
        public GameObject btnChg;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4906); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4906); } }
    }
}
