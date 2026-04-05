using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Effects/ImageRectMask", 14)]
    public class ImageRectMask : BaseMeshEffect
    {
        [ALHeader("是否根据RectTransform的Size来控制遮挡")]
        public bool m_byRectTransform = true;
        [Range(0,1)]
        public float  m_rangeLeft;
        [Range(0,1)]
        public float  m_rangeRight = 1;
        
        [Range(0,1)]
        public float  m_rangeUp;
        [Range(0,1)]
        public float  m_rangeBottom = 1;
        
        [NonSerialized] private RectTransform m_RectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (ReferenceEquals(m_RectTransform, null))
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        /// <summary>
        /// 设置进度显示 左
        /// </summary>
        public void SetRangeLeft(float _range)
        {
            m_rangeLeft = _range;
            if(null != graphic)
                graphic.SetVerticesDirty();
        }
        /// <summary>
        /// 设置进度显示 右
        /// </summary>
        public void SetRangeRight(float _range)
        {
            m_rangeRight = _range;
            if(null != graphic)
                graphic.SetVerticesDirty();
        }
        /// <summary>
        /// 设置进度显示 上
        /// </summary>
        public void SetRangeUp(float _range)
        {
            m_rangeUp = _range;
            if(null != graphic)
                graphic.SetVerticesDirty();
        }
        /// <summary>
        /// 设置进度显示 下
        /// </summary>
        public void SetRangeBottom(float _range)
        {
            m_rangeBottom = _range;
            if(null != graphic)
                graphic.SetVerticesDirty();
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if(m_rangeUp > m_rangeBottom)
            {
                float temp = m_rangeUp;
                m_rangeUp = m_rangeBottom;
                m_rangeBottom = temp;
            }
            if(m_rangeLeft > m_rangeRight)
            {
                float temp = m_rangeLeft;
                m_rangeLeft = m_rangeRight;
                m_rangeRight = temp;
            }
        }
        [HideInInspector]public Rect _m_curRect = new Rect();

#endif
        void ModifyLocalRange(ref UIVertex _uiVertex0, ref UIVertex _uiVertex1, ref UIVertex _uiVertex2, ref UIVertex _uiVertex3)
        {
            Vector4 v = new Vector4();
            v.x = _uiVertex0.position.x;
            v.y = _uiVertex0.position.y;
            v.z = _uiVertex2.position.x;
            v.w = _uiVertex2.position.y;
            Vector4 nv = new Vector4();
            
            float tx0 = _uiVertex0.uv0.x;
            float ty0 = _uiVertex0.uv0.y;
            float tx1 = _uiVertex2.uv0.x;
            float ty1 = _uiVertex2.uv0.y;
            
            float ntx0 = _uiVertex0.uv0.x;
            float nty0 = _uiVertex0.uv0.y;
            float ntx1 = _uiVertex2.uv0.x;
            float nty1 = _uiVertex2.uv0.y;
            // 局部部分
            {
                //横坐标
                {
                    //Min
                    {
                        float fill = (tx1 - tx0) * (1-m_rangeRight);
                        nv.x = v.z - (v.z - v.x) * (1-m_rangeRight);
                        ntx0 = tx1 - fill;
                    }
                    //Max
                    {
                        float fill = (tx1 - tx0) * (1- m_rangeLeft -1);
                        nv.z = v.x - (v.z - v.x) * (1- m_rangeLeft - 1);
                        ntx1 = tx0 - fill;
                    }
                }
                //纵坐标
                {
                    //Min
                    {
                        float fill = (ty1 - ty0) * m_rangeUp;
                        nv.y = v.w - (v.w - v.y) * m_rangeUp;
                        nty0 = ty1 - fill;
                    }
                    //Max
                    {
                        float fill = (ty1 - ty0) * (m_rangeBottom-1);
                        nv.w = v.y - (v.w - v.y) * (m_rangeBottom-1);
                        nty1 = ty0 - fill;
                    }
                }
            }
            //全局部分
            
            #if UNITY_EDITOR
            _m_curRect = new Rect(nv.x, nv.y,nv.z -nv.x, nv.w-nv.y);
            #endif
            _uiVertex0.position = new Vector2(nv.x, nv.y);
            _uiVertex1.position = new Vector2(nv.x, nv.w);
            _uiVertex2.position = new Vector2(nv.z, nv.w);
            _uiVertex3.position = new Vector2(nv.z, nv.y);
            
            _uiVertex0.uv0 = new Vector2(ntx0, nty0);
            _uiVertex1.uv0 = new Vector2(ntx0, nty1);
            _uiVertex2.uv0 = new Vector2(ntx1, nty1);
            _uiVertex3.uv0 = new Vector2(ntx1, nty0);
        }
        
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            if(m_byRectTransform)
            {
                CullByRectTransform(vh);
            }
            else
            {
                CullByVectex(vh);
            }
          
        }

        //根据Vertex的范围来遮挡
        public void CullByVectex(VertexHelper vh)
        {
            int count = vh.currentVertCount;
            if(count <= 0)
                return;
            for (int index = 0; index < count/4; index++)
            {
                int i0 = index * 4 + 0;
                int i1 = index * 4 + 1;
                int i2 = index * 4 + 2;
                int i3 = index * 4 + 3;
                
                UIVertex uiVertex0 = new UIVertex();
                UIVertex uiVertex1 = new UIVertex();
                UIVertex uiVertex2 = new UIVertex();
                UIVertex uiVertex3 = new UIVertex();
                
                vh.PopulateUIVertex(ref uiVertex0, i0);
                vh.PopulateUIVertex(ref uiVertex1, i1);
                vh.PopulateUIVertex(ref uiVertex2, i2);
                vh.PopulateUIVertex(ref uiVertex3, i3);
                
                ModifyLocalRange(ref uiVertex0, ref uiVertex1, ref uiVertex2, ref uiVertex3);

                vh.SetUIVertex(uiVertex0, i0);
                vh.SetUIVertex(uiVertex1, i1);
                vh.SetUIVertex(uiVertex2, i2);
                vh.SetUIVertex(uiVertex3, i3);
            }
        }

        //根据RectTransform 的范围来遮挡
        public void CullByRectTransform(VertexHelper vh)
        {
            int count = vh.currentVertCount;
            if(count <= 0)
                return;

            Rect rect = rectTransform.rect;

            float minX = rect.xMin + m_rangeLeft * rect.width;
            float maxX = rect.xMax - (1 - m_rangeRight) * rect.width;
            float minY = rect.yMin + (1- m_rangeBottom) * rect.height;
            float maxY = rect.yMax - (m_rangeUp) * rect.height;

            Vector2 min = new Vector2(minX, minY);
            Vector2 max = new Vector2(maxX, maxY);

            for (int index = 0; index < count / 4; index++)
            {
                int i0 = index * 4 + 0;
                int i1 = index * 4 + 1;
                int i2 = index * 4 + 2;
                int i3 = index * 4 + 3;

                UIVertex uiVertex0 = new UIVertex();
                UIVertex uiVertex1 = new UIVertex();
                UIVertex uiVertex2 = new UIVertex();
                UIVertex uiVertex3 = new UIVertex();

                vh.PopulateUIVertex(ref uiVertex0, i0);
                vh.PopulateUIVertex(ref uiVertex1, i1);
                vh.PopulateUIVertex(ref uiVertex2, i2);
                vh.PopulateUIVertex(ref uiVertex3, i3);

                Vector2 size = (uiVertex2.position - uiVertex0.position);
                Vector2 uvSize = (uiVertex2.uv0 - uiVertex0.uv0);

                Vector2 param = uvSize / size;

                Vector2 uiVertexPos = new Vector2(uiVertex0.position.x, uiVertex0.position.y);
                Vector2 vertexUV = new Vector2(uiVertex0.uv0.x, uiVertex0.uv0.y);
                Vector2 uvMin = (min - uiVertexPos) * param + vertexUV;
                Vector2 uvMax = (max - uiVertexPos) * param + vertexUV;

                if(uiVertex0.position.x < min.x)
                {
                    uiVertex0.position.x = min.x;
                    uiVertex1.position.x = min.x;

                    uiVertex0.uv0.x = uvMin.x;
                    uiVertex1.uv0.x = uvMin.x;
                }
                if(uiVertex0.position.y < min.y)
                {
                    uiVertex0.position.y = min.y;
                    uiVertex3.position.y = min.y;

                    uiVertex0.uv0.y = uvMin.y;
                    uiVertex3.uv0.y = uvMin.y;
                }
                if(uiVertex2.position.x > max.x)
                {
                    uiVertex2.position.x = max.x;
                    uiVertex3.position.x = max.x;

                    uiVertex2.uv0.x = uvMax.x;
                    uiVertex3.uv0.x = uvMax.x;
                }
                if(uiVertex2.position.y > max.y)
                {
                    uiVertex1.position.y = max.y;
                    uiVertex2.position.y = max.y;

                    uiVertex1.uv0.y = uvMax.y;
                    uiVertex2.uv0.y = uvMax.y;
                }

                vh.SetUIVertex(uiVertex0, i0);
                vh.SetUIVertex(uiVertex1, i1);
                vh.SetUIVertex(uiVertex2, i2);
                vh.SetUIVertex(uiVertex3, i3);
            }
        }
    }
}
