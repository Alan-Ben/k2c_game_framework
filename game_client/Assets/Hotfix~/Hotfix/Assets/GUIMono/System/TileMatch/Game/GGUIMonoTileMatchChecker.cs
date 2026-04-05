using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消游戏棋子
    /// </summary>
    public class GGUIMonoTileMatchChecker : _AHotfixBaseMono
    {
        [HotfixMono("操作对象")]
        public GameObject btnOp;
        [HotfixMono("触发移动的拖拽距离")]
        public float triggerMoveDragDis;
        
        [HotfixMono("格子交换移动时间")]
        public float exChangeMoveTime;

        [HotfixMono("格子下落时间")]
        public float fallTime;

        [HotfixMono("表现子窗口")]
        public MonoSkin showSubWnd;
    }
}