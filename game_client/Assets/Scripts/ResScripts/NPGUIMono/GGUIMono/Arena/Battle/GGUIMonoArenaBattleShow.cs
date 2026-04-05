using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗展示界面
    /// </summary>
    public class GGUIMonoArenaBattleShow : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("战斗的画面")]
        public GGUIMonoCommonShowCase monoFightShowcase;
        [ALHeader("自己的伙伴信息")]
        public GGUIMonoArenaBattleShowSubHero monoSelfSubHero;
        [ALHeader("对手的伙伴信息")]
        public GGUIMonoArenaBattleShowSubHero monoOpponentSubHero;
        [ALHeader("延时关闭窗口时间秒")]
        public float autoCloseDelayTimeSec = 2;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5212); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5212); } }
    }
}