using System.Collections.Generic;
using UnityEngine;

namespace GOE.MiniGame
{
    /// <summary>
    /// 箱子Mono抽象脚本
    /// </summary>
    public class _ADragBoxGameBoxMono : MonoBehaviour
    {
        [ALHeader("可拖动方向")]
        public List<EDragBoxGameBoxDragDirection> canDragDirection;

        [ALHeader("箱子的Animator")]
        public Animator boxAnimator;
    }
    
    /// <summary>
    /// 箱子Mono脚本
    /// </summary>
    public class GTDMonoDragBoxGameBox : _ADragBoxGameBoxMono
    {
        [ALHeader("点击脚本")]
        public GTDCommonPosClickDragMono clickMono;
    }
}