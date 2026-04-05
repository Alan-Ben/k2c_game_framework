using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoTileMatchCheckerSubBase : _AHotfixBaseMono
    {
        [HotfixMono("格子图标")]
        public Image blockIcon;
        
        [HotfixMono("表现使用的动画")]
        public Animation ani;

        [HotfixMono("生成格子动画名")]
        public string createAniName;
        
        [HotfixMono("被消除动画名")]
        public string byClearAniName;

        [HotfixMono("下落动画名")]
        public string fallAniName;
        
        [HotfixMono("idle动画")]
        public string idleAniName;
    }
}