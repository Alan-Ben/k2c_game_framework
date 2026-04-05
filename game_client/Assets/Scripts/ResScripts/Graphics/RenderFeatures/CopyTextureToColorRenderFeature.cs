using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

public class CopyTextureToColorRenderFeature : ScriptableRendererFeature
{
    public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;
        
    CopyTextureToColorPass copyPass;
    RenderTargetHandle m_CameraColorAttachment;

    public override void Create()
    {
        copyPass = new CopyTextureToColorPass(Event);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(copyPass);
    }
    public void setRt(RenderTexture _rt)
    {
        copyPass.Setup(_rt);
    }
}