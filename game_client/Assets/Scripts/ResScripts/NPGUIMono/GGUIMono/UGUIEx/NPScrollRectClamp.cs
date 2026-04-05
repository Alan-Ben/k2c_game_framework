using UnityEngine;

namespace GOE
{
    public class NPScrollRectClamp : NPScrollRect
    {
        [Header("开启拖拽范围限制")]
        public bool clampNormalizePos = false;
        [Header("拖拽可超出Content的偏移值")]
        public RectOffset m_PaddingClamp = new RectOffset();
        protected override void SetContentAnchoredPosition(Vector2 position)
        {
            if (clampNormalizePos)
            {
                Vector2 delta = position - content.anchoredPosition;
                Vector2 offset = CalculateClampOffset(delta);
                position += offset;
            }
            base.SetContentAnchoredPosition(position);
        }
        
        /// <summary>
        /// 参考CalculateOffset方法，限制移动范围
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        private Vector2 CalculateClampOffset(Vector2 delta)
        {
            Vector2 offset = Vector2.zero;
            if (movementType == MovementType.Unrestricted)
                return offset;

            Vector2 min = m_ContentBounds.min;
            Vector2 max = m_ContentBounds.max;

            // min/max offset extracted to check if approximately 0 and avoid recalculating layout every frame (case 1010178)

            if (horizontal)
            {
                min.x += delta.x;
                max.x += delta.x;

                //参考CalculateOffset方法，限制移动范围，增加m_PaddingClamp的偏移值
                float maxOffset = viewRect.rect.max.x - max.x - m_PaddingClamp.right;
                float minOffset = viewRect.rect.min.x - min.x + m_PaddingClamp.left;

                if (minOffset < -0.001f)
                    offset.x = minOffset;
                else if (maxOffset > 0.001f)
                    offset.x = maxOffset;
            }

            if (vertical)
            {
                min.y += delta.y;
                max.y += delta.y;

                float maxOffset = viewRect.rect.max.y - max.y - m_PaddingClamp.top;
                float minOffset = viewRect.rect.min.y - min.y + m_PaddingClamp.bottom;

                if (maxOffset > 0.001f)
                    offset.y = maxOffset;
                else if (minOffset < -0.001f)
                    offset.y = minOffset;
            }

            return offset;
        }
    }
}