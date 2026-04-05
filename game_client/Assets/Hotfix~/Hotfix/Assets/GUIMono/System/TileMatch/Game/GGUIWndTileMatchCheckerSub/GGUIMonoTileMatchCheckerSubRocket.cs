using GOE;

namespace Hotfix
{
    /// <summary>
    /// 火箭格子子窗口
    /// </summary>
    public class GGUIMonoTileMatchCheckerSubRocket : GGUIMonoTileMatchCheckerSubBase
    {
        [HotfixMono("通过彩虹格生成动画名")]
        public string createByRainbowAniName;
        
        [HotfixMono("主动联合普通格子触发时棋盘消除特效id")]
        public long proactiveUniteNormalTriggerCheckerboardClearSfxId;
        
        [HotfixMono("联合火箭触发时棋盘消除特效id(默认是横向特效)")]
        public long proactiveUniteRocketTriggerCheckerboardClearSfxId;

        [HotfixMono("联合炸弹格子触发时棋盘清除特效id(默认是横向特效)")]
        public long proactiveUniteBoomTriggerCheckerboardClearSfxId;
    }
}