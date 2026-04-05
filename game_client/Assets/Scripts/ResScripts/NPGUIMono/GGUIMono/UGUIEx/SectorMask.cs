using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    /// <summary>
    /// 扇形遮罩
    /// </summary>
    [AddComponentMenu("UI/Effects/SectorMask", 14)]
    public class SectorMask : BaseMeshEffect
    {
        [SerializeField]
        private Vector2 m_CenterPos = new Vector2(0, -50f);
        [SerializeField]
        private Vector2 m_AngleRange = new Vector2(-45, 45);
        
        [NonSerialized] private RectTransform m_RectTransform;
        public RectTransform rectTransform
        {
            get
            {
                // The RectTransform is a required component that must not be destroyed. Based on this assumption, a
                // null-reference check is sufficient.
                if (ReferenceEquals(m_RectTransform, null))
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }
        /// <summary>
        /// 设置扇形中心
        /// </summary>
        public Vector2 centerPos
        {
            get { return m_CenterPos; }
            set
            {
                m_CenterPos = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }
        /// <summary>
        /// 设置扇形的左右角度
        /// </summary>
        public Vector2 angleRange
        {
            get { return m_AngleRange; }
            set
            {
                m_AngleRange = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }

#endif

        protected void ApplySectorMask(List<UIVertex> verts,int start, int end, Vector2 centerPos, Vector2 angleRange)
        {
            UIVertex vt;

            var neededCapacity = verts.Count + end - start;
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;
            float width = rectTransform.rect.width;
            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                
                Vector3 pos = vt.position;

                Vector4 uv = vt.uv0;
                // Debug.Log($"{i}:{pos}, {uv}");
                float angel = 0;
                switch (i)
                {
                    case 0:
                    case 1:
                    case 5:
                        angel = angleRange.x;
                        break;
                    default:
                        angel = angleRange.y;
                        break;
                }
                float nposx = centerPos.x + (pos.y - centerPos.y) * Mathf.Tan(angel/180f*Mathf.PI);
                float nuvx = (nposx - pos.x) / width + uv.x;
                pos.x = nposx;
                uv.x = nuvx;
                vt.position = pos;
                vt.uv0 = uv;
                verts[i] = vt;
            }
        }
        
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            List<UIVertex> output = new List<UIVertex>();
            vh.GetUIVertexStream(output);

            ApplySectorMask(output, 0, output.Count, m_CenterPos, m_AngleRange);
            vh.Clear();
            vh.AddUIVertexTriangleStream(output);
        }
    }
}
