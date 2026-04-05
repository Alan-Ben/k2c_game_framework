using System.Collections.Generic;
using ALPackage;
using TMPro;

namespace GOE
{
    /// <summary>
    /// 晚间活动状态时间
    /// </summary>
    public class GGUISubMonoEveningDungeonStateTimeCountDown : _AALBasicUIWndMono
    {
        [ALHeader("活动时间倒计时文本")]
        public List<TextEx> txtActivityTimeCountDownList;
        public List<TMP_Text> txtMeshProActivityTimeCountDownList;
        [ALHeader("是否需要用上显示活动状态的key")]
        public bool needUseShowActivityStateDescKey;

        [ALHeader("活动展示状态配置列表")]
        public List<GGUIEveningDungeonActivityStateShow> showStateList;
    }
}