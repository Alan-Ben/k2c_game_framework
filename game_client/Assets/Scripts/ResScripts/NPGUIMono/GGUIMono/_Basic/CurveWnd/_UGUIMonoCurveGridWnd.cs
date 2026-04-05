using ALPackage;
using UnityEngine;

namespace GOE
{
    public class _UGUIMonoCurveGridWnd<_T_ITEM_MONO> : _TALUGUIMonoGridWnd<_T_ITEM_MONO> 
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
    {
        [ALHeader("曲线对应的显示区域")]
        public RectTransform curveAreaObj;
        [ALHeader("列表显示区域比遮罩区域小的时候，是否居中显示content")]
        public bool isContentToCenter = true;
        [ALHeader("默认向上或向左弯曲")]
        public bool isDefaultCurve;
        [ALHeader("无物品提示")]
        public GameObject noneItemsTips;
        [ALHeader("曲线")] 
        public AnimationCurve curve;
        [ALHeader("最小高度限制")] 
        public float minHeight;
        [ALHeader("最大高度限制")] 
        public float maxHeight;
    }
}