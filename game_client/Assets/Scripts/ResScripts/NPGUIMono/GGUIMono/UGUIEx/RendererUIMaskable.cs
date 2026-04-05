
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasRenderer))]
    public class RendererUIMaskable : MaskableGraphic
    {
        [ALHeader("需要设置RectMask2D的Renderer")]
        public List<Renderer> renders = new List<Renderer>();

        private Rect _m_lastClipRect;
        private bool _m_hasClipRect = false;
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        public override void SetClipRect(Rect clipRect, bool validRect)
        {
            base.SetClipRect(clipRect, validRect);
            if(canvas == null)return;
            
            if (isActiveAndEnabled)
            {
                // 把UI顶点转为世界坐标顶点，匹配粒子、meshRender
                Vector3 posLB = TransformPoint(new Vector3(clipRect.xMin,clipRect.yMin,0));
                Vector3 posRU = TransformPoint(new Vector3(clipRect.xMax,clipRect.yMax,0));
                Vector4 cv = new Vector4(posLB.x, posLB.y, posRU.x, posRU.y);
                foreach (var render in renders)
                {
                    RenderMaskableMatMgr.instance.EnableRectClipping(render, clipRect, cv);
                }
                
                _m_lastClipRect = clipRect;
                _m_hasClipRect = true;
            }
            else
            {
                _disableClip();
            }
        }

        private void _disableClip()
        {
            if (renders != null)
                foreach (var render in renders)
                {
                    RenderMaskableMatMgr.instance.DisableRectClipping(render, _m_lastClipRect);
                }

            _m_hasClipRect = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _disableClip();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            // 组件销毁时，确保释放所有 Renderer 的材质引用
            // 只有在启用了裁剪的情况下才需要释放
            if (_m_hasClipRect)
            {
                _disableClip();
            }
        }

        // 把UI顶点转为世界坐标顶点，匹配粒子、meshRender
        public Vector3 TransformPoint(Vector3 lpos)
        {
            Vector3 nlpos = canvas.rootCanvas.transform.TransformPoint(lpos);
            return nlpos;
        }
    }
