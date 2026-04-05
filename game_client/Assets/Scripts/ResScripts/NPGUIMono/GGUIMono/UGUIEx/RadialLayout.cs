
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 圆弧形的LayoutGroup扩展
    /// </summary>
    [AddComponentMenu("Layout/Extensions/Radial Layout")]
    public class RadialLayout : LayoutGroup
    {
        [ALHeader("离圆心距离")]
        public float fDistance;

        [ALHeader("是否需要反向排列")]
        public bool isReverse;

        [Range(0f, 360f)]
        [ALHeader("角度范围，超过这个范围会均分")]
        public float angleRange;
        [Range(0f, 360f)]
        [ALHeader("默认间隔角度大小")]
        public float angleDefault;
        [Range(-180f, 180f)]
        [ALHeader("整体偏移角度，0为居中平均分布，复数逆时针旋转，正数顺时针旋转")]
        public float angleOffset;

        protected override void OnEnable()
        {
            base.OnEnable();
            _calculateRadial();
        }
        public override void SetLayoutHorizontal()
        {
        }
        public override void SetLayoutVertical()
        {
        }
        public override void CalculateLayoutInputVertical()
        {
            _calculateRadial();
        }
        public override void CalculateLayoutInputHorizontal()
        {
            _calculateRadial();
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _calculateRadial();
        }
#endif
        
        //计算一次位置
        private void _calculateRadial()
        {
            m_Tracker.Clear();
            if (transform.childCount == 0)
                return;
            
            //需要显示的子元素个数
            int childrenToFormat = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && child.gameObject.activeSelf)
                    ++childrenToFormat;
            }

            //均分的角度间隔
            float fOffsetAngle = 0f;
            float fAngle = 0f;
            //判断是否会超出范围
            if ((childrenToFormat - 1) * angleDefault > angleRange)
            {
                //元素的角度,初始值为范围的边界
                fAngle = angleOffset + (180 - angleRange) / 2;
                //最终间隔是平局间隔
                fOffsetAngle = (angleRange) / (childrenToFormat - 1);
            }
            else
            {
                //元素的角度,初始值计算结果
                fAngle = angleOffset + 90f - (childrenToFormat - 1) * angleDefault / 2;
                //最终间隔是配置间隔
                fOffsetAngle = angleDefault;
            }

            int idx = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                idx = i;
                //反向取值
                if (isReverse)
                    idx = transform.childCount - i - 1;

                RectTransform child = (RectTransform)transform.GetChild(idx);
                if (child != null && child.gameObject.activeSelf)
                {
                    //禁止editor去改
                    m_Tracker.Add(this, child,
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.Pivot);
                        
                    Vector3 vPos = new Vector3(-Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                    child.localPosition = vPos * fDistance;
                    child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
                        
                    //每次累加角度间隔计算下一个
                    fAngle += fOffsetAngle;
                }
            }   
        }
    }
}
