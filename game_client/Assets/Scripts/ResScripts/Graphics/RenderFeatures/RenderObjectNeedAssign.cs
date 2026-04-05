using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Experimental.Rendering.Universal
{
    [ExcludeFromPreset]
    public class RenderObjectNeedAssign : ScriptableRendererFeature
    {
        [System.Serializable]
        public class ModelOutlineSettings
        {
            public string passTag = "RenderObjectNeedAssign";
            public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

            public Material overrideMaterial = null;
            public int overrideMaterialPassIndex = 0;
        }

        public HashSet<SkinnedMeshRenderer> skinRenderers = new HashSet<SkinnedMeshRenderer>();

        public void AddSkinRenderer(SkinnedMeshRenderer _renderer)
        {
            if(null == skinRenderers) 
                return;
            
            skinRenderers.Add(_renderer);
        }
        public void RemoveSkinRenderer(SkinnedMeshRenderer _renderer)
        {
            if(null == skinRenderers)
                return;
            
            skinRenderers.Remove(_renderer);
        }
        public ModelOutlineSettings settings = new ModelOutlineSettings();

        RenderObjectNeedAssignPass renderObjectsPass;

        public override void Create()
        {
            // Render Objects pass doesn't support events before rendering prepasses.
            // The camera is not setup before this point and all rendering is monoscopic.
            // Events before BeforeRenderingPrepasses should be used for input texture passes (shadow map, LUT, etc) that doesn't depend on the camera.
            // These events are filtering in the UI, but we still should prevent users from changing it from code or
            // by changing the serialized data.
            if (settings.Event < RenderPassEvent.BeforeRenderingPrePasses)
                settings.Event = RenderPassEvent.BeforeRenderingPrePasses;

            renderObjectsPass = new RenderObjectNeedAssignPass(settings.passTag, settings.Event);

            renderObjectsPass.overrideMaterial = settings.overrideMaterial;
            renderObjectsPass.overrideMaterialPassIndex = settings.overrideMaterialPassIndex;

            renderObjectsPass.skinRenderers = skinRenderers;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(renderObjectsPass);
        }
    }
}

