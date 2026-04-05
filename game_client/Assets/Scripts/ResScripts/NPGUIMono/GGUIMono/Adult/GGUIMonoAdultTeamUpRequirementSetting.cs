using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 子嗣组队要求设置窗口
    /// </summary>
    public class GGUIMonoAdultTeamUpRequirementSetting : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("收益限制范围")]
        public TextEx txtLowerEarningsLimitRange;
        [ALHeader("收益限制范围Key(两个参数, 1.最低限制 2.最高限制, 程序会附上大数值单位, 如K,M等)")]
        public string lowerEarningsLimitRangeKey;

        [ALHeader("当前收益限制")]
        public TextEx txtNowLimitEarnings;
        [ALHeader("当前收益限制Key(一个参数, 当前限制值, 程序会附上大数值单位, 如K,M等)")]
        public string nowLimitEarningsKey;

        [ALHeader("增加最低收益限制按钮")]
        public GameObject btnAddLimit;
        [ALHeader("增加最低收益限制百分比文本")]
        public TextEx txtAddLimitAdjustmentPercentage;
        [ALHeader("达到最高限制提示文本")]
        public string reachMaxLimitTip;
        
        [ALHeader("减少最低收益限制按钮")]
        public GameObject btnReduceLimit;
        [ALHeader("减少最低收益限制百分比文本")]
        public TextEx txtReduceLimitAdjustmentPercentage;
        [ALHeader("达到最低限制提示文本")]
        public string reachMinLimitTip;
        
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2521); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2521); } }
    }
}