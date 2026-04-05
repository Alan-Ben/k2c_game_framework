using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家赚速详情弹窗
    /// </summary>
    public class GGUIMonoPlayerEarningsDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("特权额外加成")]
        public Text txtAddition;
        [ALHeader("特权额外加成翻译key")]
        public string additionTransKey;
        [ALHeader("当前总收益")]
        public Text txtCurEarnings;
        [ALHeader("当前建筑总收益")]
        public Text txtCurBuildingEarnings;
        [ALHeader("当前学生总收益")]
        public Text txtCurChildEarnings;
        [ALHeader("历史最高总收益")]
        public Text txtMaxEarnings;
        [ALHeader("历史最高建筑总收益")]
        public Text txtMaxBuildingEarnings;
        [ALHeader("历史最高学生总收益")]
        public Text txtMaxChildEarnings;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1515); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1515); } }
    }
}