using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Experimental.Rendering.Universal
{
    public class RenderObjectNeedAssignPass : ScriptableRenderPass
    {
        string m_ProfilerTag;
        ProfilingSampler m_ProfilingSampler;

        public Material overrideMaterial { get; set; }
        public int overrideMaterialPassIndex { get; set; }
        public HashSet<SkinnedMeshRenderer> skinRenderers { get; set; }


        public RenderObjectNeedAssignPass(string profilerTag, RenderPassEvent renderPassEvent)
        {
            base.profilingSampler = new ProfilingSampler(nameof(RenderObjectsPass));

            m_ProfilerTag = profilerTag;
            m_ProfilingSampler = new ProfilingSampler(profilerTag);
            this.renderPassEvent = renderPassEvent;
            this.overrideMaterial = null;
            this.overrideMaterialPassIndex = 0;

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // NOTE: Do NOT mix ProfilingScope with named CommandBuffers i.e. CommandBufferPool.Get("name").
            // Currently there's an issue which results in mismatched markers.
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSampler))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                if (overrideMaterial != null )
                {
                    if (skinRenderers != null)
                    {
                        foreach (var skin in skinRenderers)
                        {
                            if(skin == null || skin.sharedMesh == null)
                                continue;
                            for (int i = 0; i < skin.sharedMesh.subMeshCount; i++)
                            {
                                cmd.DrawRenderer(skin, overrideMaterial, i, overrideMaterialPassIndex);
                            }
                        }
                    }
                    // case MeshRenderer mesh:
                    // {
                    //     MeshFilter filter = renderer.GetComponent<MeshFilter>();
                    //     for (int i = 0; i < filter.sharedMesh.subMeshCount; i++)
                    //     {
                    //         cmd.DrawRenderer(mesh, overrideMaterial, i, overrideMaterialPassIndex);
                    //     }
                    //     break;
                    // }
                    // default:
                    // cmd.DrawRenderer(renderer, overrideMaterial, 0, overrideMaterialPassIndex);
                    // break;
                }
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
