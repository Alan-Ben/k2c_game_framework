using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用渲染表现接口_SpriteRenderer的
    /// </summary>
    public class ShowCaseCommonResObjRenderEffect_SpriteRenderer : _AShowCaseCommonResObjRenderEffect
    {
        [ALHeader("渲染器列表")]
        public List<SpriteRenderer> spriteRendererList;
        
        public override void setAlpha(float _alpha)
        {
            if(null == spriteRendererList)
                return;
            
            foreach (SpriteRenderer renderer in spriteRendererList)
            {
                if(null == renderer)
                    continue;

                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, _alpha);
            }
        }
    }
}