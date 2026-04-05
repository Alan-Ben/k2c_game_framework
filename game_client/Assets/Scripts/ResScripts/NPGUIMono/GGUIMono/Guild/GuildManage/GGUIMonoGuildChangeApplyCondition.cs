using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟修改申请条件界面
    /// </summary>
    public class GGUIMonoGuildChangeApplyCondition : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("加入方式")]
        public Text txtJoinType;
        [ALHeader("上一个加入方式按钮")]
        public GameObject btnPreviousJoinType;
        [ALHeader("下一个加入方式按钮")]
        public GameObject btnNextJoinType;
        [ALHeader("加入等级限制")]
        public Text txtJoinLevel;
        [ALHeader("上一个加入等级限制按钮")]
        public GameObject btnPreviousJoinLevel;
        [ALHeader("下一个加入等级限制按钮")]
        public GameObject btnNextJoinLevel;
        [ALHeader("国力限制输入")]
        public InputField inputNationPowerLimit;
        [ALHeader("国力限制输入值太小提示")]
        public GameObject goNationPowerLimitHint;
        [ALHeader("保存按钮")]
        public GameObject btnSave;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4907); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4907); } }
    }
}
