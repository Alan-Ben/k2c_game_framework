using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    /// <summary>
    /// 三消任务item
    /// </summary>
    public class GGUIMonoTileMatchTaskBlockItem : _AHotfixBaseMono
    {
        [HotfixMono("图标")]
        public Image icon;

        [HotfixMono("进度文本")]
        public TextEx txtProgress;
        [HotfixMono("进度完成时颜色")]
        public Color progressCompletedColor = Color.green;
        [HotfixMono("进度未完成时颜色")]
        public Color progressUncompletedColor = Color.red;
        

        [HotfixMono("动画")]
        public Animation ani;

        [HotfixMono("收集动画名称")]
        public string collectAniName;
    }
}