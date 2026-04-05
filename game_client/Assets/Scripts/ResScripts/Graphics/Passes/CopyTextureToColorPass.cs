using System;

namespace UnityEngine.Rendering.Universal.Internal
{
    /// <summary>
    /// Copy the given color buffer to the given destination color buffer.
    ///
    /// You can use this pass to copy a color buffer to the destination,
    /// so you can use it later in rendering. For example, you can copy
    /// the opaque texture to use it for distortion effects.
    /// </summary>
    public class CopyTextureToColorPass : ScriptableRenderPass
    {
        private RenderTexture source { get; set; }

        /// <summary>
        /// Create the CopyColorPass
        /// </summary>
        public CopyTextureToColorPass(RenderPassEvent evt)
        {
            base.profilingSampler = new ProfilingSampler(nameof(CopyColorPass));
            renderPassEvent = evt;
        }

        /// <summary>
        /// Configure the pass with the source and destination to execute on.
        /// </summary>
        /// <param name="source">Source Render Target</param>
        /// <param name="destination">Destination Render Target</param>
        public void Setup(RenderTexture source)
        {
            this.source = source;
        }

        /// <inheritdoc/>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
#if NP_GAME

            // if(source == null || ScriptableRenderer.current == null)
            //     return;
            // CommandBuffer cmd = CommandBufferPool.Get();
            // using (new ProfilingScope(cmd, new ProfilingSampler("CopyTextureToColor")))
            // {
            //     cmd.Blit(source, ScriptableRenderer.current.cameraColorTarget);
            // }
            // context.ExecuteCommandBuffer(cmd);
            // CommandBufferPool.Release(cmd);
#endif
        }
    }
}
