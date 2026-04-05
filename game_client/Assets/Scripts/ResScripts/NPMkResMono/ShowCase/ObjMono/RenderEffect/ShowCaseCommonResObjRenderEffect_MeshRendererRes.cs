using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用渲染表现接口_Spine资源的渲染修改器
    /// </summary>
    public class ShowCaseCommonResObjRenderEffect_MeshRendererRes : _AShowCaseCommonResObjRenderEffect
    {
        [ALHeader("MeshRenderer数据")]
        public List<MeshRenderer> meshRendererList;
        
        public override void setAlpha(float _alpha)
        {
            if(null == meshRendererList)
                return;

            foreach (MeshRenderer meshRenderer in meshRendererList)
            {
                if(null == meshRenderer || null == meshRenderer.materials)
                    continue;
                
                foreach (Material material in meshRenderer.materials)
                {
                    if(null == material)
                        continue;
                
                    Color color = material.color;
                    color.a = _alpha;
                    material.color = color;
                }
            }
        }
    }
}