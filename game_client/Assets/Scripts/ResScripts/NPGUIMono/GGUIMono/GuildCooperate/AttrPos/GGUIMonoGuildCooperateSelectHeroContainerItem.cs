using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 派遣伙伴显示状态
    /// </summary>
    public enum EGuildCooperateHeroShowState
    {
        [InspectorName("CAN_DISPATCH（可派遣）")]
        CAN_DISPATCH,
        [InspectorName("CAN_NOT_DISPATCH（不可派遣）")]
        CAN_NOT_DISPATCH,
    }

    /// <summary>
    /// 公会协作属性据点选择伙伴列表item
    /// </summary>
    public class GGUIMonoGuildCooperateSelectHeroContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClickItem;
        [ALHeader("总共建设值")]
        public Text txtTotalConstructionValue;
        [ALHeader("恢复描述")]
        public Text txtRecoverDesc;
        [ALHeader("伙伴信息item")]
        public GGUIMonoHeroCommonCardItem monoHeroCard;
        [ALHeader("不同选中状态显示")]
        public MultiStateShow<ECommonSelectState> selectShow;
        [ALHeader("伙伴状态显示")]
        public MultiStateShow<EGuildCooperateHeroShowState> heroStateShow;
        [ALHeader("派遣动画")]
        public CommonAnimationSingleInfo aniDispatch;
        [ALHeader("恢复动画")]
        public CommonAnimationSingleInfo aniRecover;
    }
}
