using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Experimental.Rendering.Universal
{
    [ExcludeFromPreset]
    [MovedFrom("UnityEngine.Experimental.Rendering.LWRP")]public class RenderObjectsEx : ScriptableRendererFeature
    {
        [System.Serializable]
        public class RenderObjectsExSettings
        {
            public string passTag = "RenderObjectsExFeature";
            public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

            public FilterSettings filterSettings = new FilterSettings();

            public Material overrideMaterial = null;
            public int overrideMaterialPassIndex = 0;

            public bool overrideDepthState = false;
            public CompareFunction depthCompareFunction = CompareFunction.LessEqual;
            public bool enableWrite = true;

            public StencilStateData stencilSettings = new StencilStateData();

            public RenderObjects.CustomCameraSettings cameraSettings = new RenderObjects.CustomCameraSettings();
        }

        [System.Serializable]
        public class FilterSettings
        {
            // TODO: expose opaque, transparent, all ranges as drop down
            public RenderQueueType RenderQueueType;
            public LayerMask LayerMask;
            public bool SortingLayerAll = true;
            public int FrontSortingLayerID;
            public int BackSortingLayerID;
            public string[] PassNames;

            public FilterSettings()
            {
                RenderQueueType = RenderQueueType.Opaque;
                LayerMask = 0;
            }
        }

        public RenderObjectsExSettings settings = new RenderObjectsExSettings();

        RenderObjectsPassEx renderObjectsPass;

        public override void Create()
        {
            FilterSettings filter = settings.filterSettings;

            // Render Objects pass doesn't support events before rendering prepasses.
            // The camera is not setup before this point and all rendering is monoscopic.
            // Events before BeforeRenderingPrepasses should be used for input texture passes (shadow map, LUT, etc) that doesn't depend on the camera.
            // These events are filtering in the UI, but we still should prevent users from changing it from code or
            // by changing the serialized data.
            if (settings.Event < RenderPassEvent.BeforeRenderingPrePasses)
                settings.Event = RenderPassEvent.BeforeRenderingPrePasses;
            
            var sortingLayerRange = SortingLayerRange.all;
            if (!filter.SortingLayerAll)
            {
                int layerFront = SortingLayer.GetLayerValueFromID(filter.FrontSortingLayerID);
                int layerBack = SortingLayer.GetLayerValueFromID(filter.BackSortingLayerID);
                sortingLayerRange = new SortingLayerRange((short) layerFront, (short) layerBack);
            }
           
            renderObjectsPass = new RenderObjectsPassEx(settings.passTag, settings.Event, filter.PassNames,
                filter.RenderQueueType, filter.LayerMask, sortingLayerRange, settings.cameraSettings);

            renderObjectsPass.overrideMaterial = settings.overrideMaterial;
            renderObjectsPass.overrideMaterialPassIndex = settings.overrideMaterialPassIndex;

            if (settings.overrideDepthState)
                renderObjectsPass.SetDetphState(settings.enableWrite, settings.depthCompareFunction);

            if (settings.stencilSettings.overrideStencilState)
                renderObjectsPass.SetStencilState(settings.stencilSettings.stencilReference,
                    settings.stencilSettings.stencilCompareFunction, settings.stencilSettings.passOperation,
                    settings.stencilSettings.failOperation, settings.stencilSettings.zFailOperation);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(renderObjectsPass);
        }
    }
}

