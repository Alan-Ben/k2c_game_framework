using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 一键游历结果item
    /// </summary>
    public class _AGGUIMonoAkeyTravelResultItem : _AALBasicUIWndMono
    {
        [ALHeader("事件对象半身像")]
        public RawImage targetMidImage;

        [ALHeader("事件描述")]
        public TextEx eventDesc;

        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer rewardList;

        [ALHeader("第一个奖励描述")]
        public TextEx firstRewardDesc;
        [ALHeader("第一个奖励描述key(两个参数, 第一个是物品名, 第二个是奖励数量)")]
        public string firstRewardDescKey;
    }
}