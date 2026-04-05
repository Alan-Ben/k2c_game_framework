using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    /// <summary>
    /// 三消任务飞行item
    /// </summary>
    public class GGUIMonoTileMatchTaskBlockFlyItem : _AHotfixBaseMono
    {
        [HotfixMono("图标")]
        public Image icon;

        [HotfixMono("飞行需要的时间(秒)")]
        public float flyTimeS; // 飞行时间
        
        [HotfixMono("动画")]
        public Animation ani;

        [HotfixMono("飞行动画名称")]
        public string aniFlyAnimation;
    }
}