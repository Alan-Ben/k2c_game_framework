using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Experimental.Rendering.Universal
{
    // 把颜色图结果拷贝
    public class CopyColorSimplePass : ScriptableRenderPass
    {
        private RenderTargetIdentifier source { get; set; }
        private RenderTargetIdentifier destination { get; set; }
        Material m_BlitMaterial;
        string m_ShaderKeyword;

        private bool m_ScreenToTarget = false;

        private RenderTargetIdentifier m_target;

        public CopyColorSimplePass(RenderPassEvent evt, Material blitMaterial, string shaderKeyword)
        {
            base.profilingSampler = new ProfilingSampler(nameof(CopyColorPass));
            renderPassEvent = evt;
            m_BlitMaterial = blitMaterial;
            m_ShaderKeyword = shaderKeyword;
        }

        /// <summary>
        /// 把屏幕拷贝到目标RenderTexture
        /// </summary>
        /// <param name="_target">Destination Render Target</param>
        public void SetupScreenToTarget(RenderTargetIdentifier _target)
        {
            m_ScreenToTarget = true;
            m_target = _target;
        }
        /// <summary>
        /// 把目标RenderTexture拷贝到屏幕
        /// </summary>
        /// <param name="_target">Source Render Target</param>
        public void SetupTargetToScreeen(RenderTargetIdentifier _target)
        {
            m_ScreenToTarget = false;
            m_target = _target;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // On Metal iOS, prevent camera attachments to be bound and cleared during this pass.
            ConfigureTarget(destination);
            ConfigureClear(ClearFlag.None, Color.black);
        }
        /// <inheritdoc/>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if(ScriptableRenderer.current == null)
                return;

            if (m_ScreenToTarget)
            {
                source = ScriptableRenderer.current.cameraColorTarget;
                destination = m_target;
            }
            else
            {
                source = m_target;
                destination = ScriptableRenderer.current.cameraColorTarget;
            }
            
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("CopyColorSimplePass")))
            {
                // cmd.SetRenderTarget(destination);
                cmd.EnableShaderKeyword(m_ShaderKeyword);
                cmd.SetGlobalTexture(ShaderPropertyId.sourceTex, source);
                if(m_BlitMaterial == null)
                    cmd.Blit(source, destination);
                else
                    cmd.Blit(source, destination, m_BlitMaterial);
                cmd.DisableShaderKeyword(m_ShaderKeyword);
                // cmd.SetRenderTarget(source);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
