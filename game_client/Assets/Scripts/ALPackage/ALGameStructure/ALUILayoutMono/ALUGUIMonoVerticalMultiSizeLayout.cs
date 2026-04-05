
using UnityEngine;
using UnityEngine.UI;

namespace ALPackage
{
    /// <summary>
    /// 和<see cref="ALUGUIWndVerticalMultiSizeLayout{_T_ITEM_MONO,_T_CONTAINER_MONO,_T_ITEM_WND}"/>配套使用的Mono类
    /// </summary>
    public class ALUGUIMonoVerticalMultiSizeLayout : _AALBasicUIWndMono
    {
        [ALHeader("拖拽的ScrollRect对象")]
        public ScrollRect scrollRect;

        [ALHeader("对齐方向")]
        public EALVerticalLayoutChildAlignment alignment;

        [ALHeader("顶端偏移值")]
        public float topPadding;

        [ALHeader("下端偏移值")]
        public float bottomPadding;

        [ALHeader("Item的间隙")]
        public float spacing;
    }
}