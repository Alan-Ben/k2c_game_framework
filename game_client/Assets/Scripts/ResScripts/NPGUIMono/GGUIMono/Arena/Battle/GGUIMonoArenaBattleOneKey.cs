using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场一键谈判弹窗
    /// </summary>
    public class GGUIMonoArenaBattleOneKey : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("开始谈判按钮")]
        public GameObject btnFight;
        [ALHeader("购买水晶临时增益，不足时购买2硬币加成")]
        public NPGGUIMonoCommonToggleEx monoBuyBuffByCrystalToggle;
        [ALHeader("购买2银币增益，不足时购买1硬币增益")]
        public NPGGUIMonoCommonToggleEx monoBuyBuffByTwoCoinToggle;
        [ALHeader("购买1银币增益")]
        public NPGGUIMonoCommonToggleEx monoBuyBuffByOneCoinToggle;
        [ALHeader("不购买临时增益")]
        public NPGGUIMonoCommonToggleEx monoNotToBuyBuffToggle;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5210); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5210); } }
    }
}