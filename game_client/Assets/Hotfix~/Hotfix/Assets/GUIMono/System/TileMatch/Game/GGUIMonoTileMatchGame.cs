using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消游戏页面
    /// </summary>
    public class GGUIMonoTileMatchGame : _AHotfixBasicUIResBarMono
    {
        [HotfixMono("游戏玩法子窗口")]
        public GGUIHotfixCommonMono monoGamePlay;

        [HotfixMono("返回按钮")]
        public GameObject btnReturn;
    }
}