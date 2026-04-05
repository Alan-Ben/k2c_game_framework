using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠
    /// </summary>
    public class GGUIMonoGuildDonate : _ANPBasicUIWndResBarMono
    {
        [ALHeader("今日捐赠进度进度条")]
        public GGUIMonoCommonRewardSlider monoDonateProgress;

        [ALHeader("捐赠进度奖励预览pathId")]
        public long donateProgressRewardPreviewToolTipResPathId;
        [ALHeader("捐赠进度奖励预览弹窗位置偏移")]
        public Vector2 donateProgressRewardPreviewToolTipOffset;
        
        [ALHeader("捐赠进度规则说明按钮")]
        public GameObject btnDonateProgressSpecification;
        [ALHeader("捐赠进度规则说明弹窗pathid")]
        public long donateProgressSpecificationToolTipResPathId;
        [ALHeader("捐赠进度规则弹窗位置偏移")]
        public Vector2 donateProgressSpecificationToolTipOffset;

        [ALHeader("捐赠详情按钮")]
        public GameObject btnDonateDetailInfo;
        [ALHeader("捐赠详情弹窗pathId")]
        public long donateDetailInfoToolTipResPathId;
        [ALHeader("捐赠详情弹窗位置偏移")]
        public Vector2 donateDetailInfoToolTipOffset;
        
        [ALHeader("捐赠列表")]
        public GGUIMonoGuildDonateItemContainer monoDonateContainer;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4917); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4917); } }
    }
}