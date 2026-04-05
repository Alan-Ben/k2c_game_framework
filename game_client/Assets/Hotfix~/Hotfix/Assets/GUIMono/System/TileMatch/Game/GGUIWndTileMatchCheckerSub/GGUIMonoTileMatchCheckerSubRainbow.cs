using GOE;

namespace Hotfix
{
    /// <summary>
    /// 彩虹格
    /// </summary>
    public class GGUIMonoTileMatchCheckerSubRainbow : GGUIMonoTileMatchCheckerSubBase
    {
        [HotfixMono("从被触发到开始飞行的时间, 单位秒")]
        public float beginTriggerToFlyTimeS;
        [HotfixMono("特效的飞行时间, 单位秒")]
        public float sfxFlyTimeS;
        
        [HotfixMono("联合普通格子触发的动画名")]
        public string uniteNormalTriggerAniName;
        [HotfixMono("联合普通格子触发飞行特效id")]
        public long uniteNormalTriggerFlySfxId;
        
        [HotfixMono("联合爆炸格子触发的动画名")]
        public string uniteBoomTriggerAniName;
        [HotfixMono("联合爆炸格子触发飞行特效id")]
        public long uniteBoomTriggerFlySfxId;
        
        [HotfixMono("联合火箭格子触发的动画名")]
        public string uniteRocketTriggerAniName;
        [HotfixMono("联合火箭格子触发飞行特效id")]
        public long uniteRocketTriggerFlySfxId;
        
        [HotfixMono("联合彩虹格子触发的动画名")]
        public string uniteRainbowTriggerAniName;
        [HotfixMono("联合彩虹格子触发时, 棋盘清除特效")]
        public long uniteRainbowTriggerCheckerboardClearSfxId;
    }
}