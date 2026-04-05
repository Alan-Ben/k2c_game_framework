using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场便捷设置弹窗
    /// </summary>
    public class GGUIMonoArenaConvenientSetting : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("自动随机翻开连胜奖励")]
        public NPGGUIMonoCommonToggleEx monoAutoGetRoundRewardToggle;
        [ALHeader("跳过单场战斗动画")]
        public NPGGUIMonoCommonToggleEx monoSkipBattleToggle;
        [ALHeader("购买水晶临时增益，不足时购买2硬币加成")]
        public NPGGUIMonoCommonToggleEx monoBuyBuffByCrystalToggle;
        [ALHeader("购买2银币增益，不足时购买1硬币增益")]
        public NPGGUIMonoCommonToggleEx monoBuyBuffByTwoCoinToggle;
        [ALHeader("购买1银币增益")]
        public NPGGUIMonoCommonToggleEx monoBuyBuffByOneCoinToggle;
        [ALHeader("不购买临时增益")]
        public NPGGUIMonoCommonToggleEx monoNotToBuyBuffToggle;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5206); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5206); } }
    }
}