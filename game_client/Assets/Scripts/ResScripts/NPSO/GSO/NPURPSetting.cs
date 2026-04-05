using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GOE
{
   
    /// <summary>
    /// URP相关的设置
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "NP/URP Setting", fileName = "np_urp_setting", order = 800)]
    public class NPURPSetting : ScriptableObject
    {
        /// <summary>
        /// UI相机的URP渲染设置
        /// </summary>
        [ALHeader(" UI相机的URP渲染设置")]
        public UniversalRendererData uiRenderData;
        
        [ALHeader("默认相机设置")]
        public UniversalRendererData defaultRenderData;
        [ALHeader("极致渲染配置")]
        public RenderPipelineAsset ultraRenderPipelineAsset;
        [ALHeader("高端机渲染配置")]
        public RenderPipelineAsset highRenderPipelineAsset;
        [ALHeader("中端机渲染配置")]
        public RenderPipelineAsset mediumRenderPipelineAsset;
        [ALHeader("低端机渲染配置")]
        public RenderPipelineAsset lowRenderPipelineAsset;
        [ALHeader("最低端渲染配置")]
        public RenderPipelineAsset veryLowRenderPipelineAsset;
    }
}