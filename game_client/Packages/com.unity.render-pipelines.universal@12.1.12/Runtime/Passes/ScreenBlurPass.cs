using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Rendering.Universal
{
	public class ScreenBlurPass : ScriptableRenderPass
	{
		public static readonly int g_DownSampleValue = Shader.PropertyToID("_DownSampleValue");
		public static readonly int g_BlurTexture       = Shader.PropertyToID("_BlurTexture");
		public static readonly int g_BlurTexture2       = Shader.PropertyToID("_BlurTexture2");
		public static readonly int g_BlurColor = Shader.PropertyToID("_BlurColor");

	    private RenderTargetIdentifier source { get; set; }
    
	    private float _m_blurSpreadSize = 2.0f;
	    private int _m_blurIterations = 1;
	    private Material _m_blurMaterial;
	    private bool _m_needDownSample = false;
	    private int _m_downSampleNum = 2;
	    private Color _m_blurColor = Color.white;
	    private List<Material> _m_lIterationMats = new List<Material>();


	    RenderTextureDescriptor _m_rtDesc;
	    /// <summary>
	    /// Create the CopyColorPass
	    /// </summary>
	    public ScreenBlurPass(RenderPassEvent evt, Material _BlurMaterial)
	    {
	        base.profilingSampler = new ProfilingSampler(nameof(ScreenBlurPass));
	        renderPassEvent = evt;
	        _m_blurMaterial = _BlurMaterial;
	    }
    
	    /// <summary>
	    /// Configure the pass with the source and destination to execute on.
	    /// </summary>
	    /// <param name="source">Source Render Target</param>
	    /// <param name="destination">Destination Render Target</param>
	    public void Setup(float _BlurSpreadSize, int _BlurIterations, Material _BlurMaterial, bool _needDownSample, int _DownSampleNum, Color _blurColor)
	    {
	        _m_blurSpreadSize = _BlurSpreadSize;
	        _m_blurIterations = _BlurIterations;
	        _m_blurMaterial = _BlurMaterial;
	        _m_needDownSample = _needDownSample;
	        _m_downSampleNum = _DownSampleNum;
	        _m_blurColor = _blurColor;
	    }

	    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
	    {
		    // On Metal iOS, prevent camera attachments to be bound and cleared during this pass.
		    // ConfigureTarget(g_BlurTexture2);
		    // ConfigureClear(ClearFlag.None, Color.black);
	    }

	    /// <inheritdoc/>
	    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	    {
		    if(ScriptableRenderer.current == null)
			    return;
		    
		    source = ScriptableRenderer.current.cameraColorTarget;

		    var cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
		    int renderWidth = _m_needDownSample ? cameraTargetDescriptor.width >> _m_downSampleNum: cameraTargetDescriptor.width;
		    int renderHeight = _m_needDownSample ? cameraTargetDescriptor.height >> _m_downSampleNum:cameraTargetDescriptor.height;
		    _m_rtDesc = new RenderTextureDescriptor(renderWidth, renderHeight);

	        if (_m_blurMaterial == null)
	        {
	            Debug.LogError("ScreenBlur Render Feature Blur材质为空");
	            return;
	        }

	        CommandBuffer cmd = CommandBufferPool.Get();
			using (new ProfilingScope(cmd, profilingSampler))
	        {
	            float widthMod = 1.0f / (1.0f * (_m_needDownSample ? 1 << _m_downSampleNum: 1 ));
	            //Shader的降采样参数赋值
	            _m_blurMaterial.SetFloat(g_DownSampleValue, _m_blurSpreadSize * widthMod);
	            _m_blurMaterial.SetColor(g_BlurColor, _m_blurColor);
        
	            cmd.GetTemporaryRT(g_BlurTexture2, _m_rtDesc, FilterMode.Bilinear);
	            cmd.Blit(source, g_BlurTexture2, _m_blurMaterial, 0);

	            cmd.GetTemporaryRT(g_BlurTexture, _m_rtDesc, FilterMode.Bilinear);
	            Material lastMat = _m_blurMaterial;
	            for (int i = 0; i < _m_blurIterations; i++)
	            {
		            //迭代偏移量参数
		            float iterationOffs = (i * 1.0f);
		            lastMat = _getIterationMat(i);

		            //Shader的降采样参数赋值
		            lastMat.SetFloat(g_DownSampleValue, _m_blurSpreadSize * widthMod + iterationOffs);
		            cmd.Blit(g_BlurTexture2, g_BlurTexture, lastMat, 1);
		            
		            //最后一次直接返回值，减少Drawcall
		            if (i < _m_blurIterations - 1)
		            {
			            cmd.Blit(g_BlurTexture, g_BlurTexture2, lastMat, 2);
		            }
	            }
	            // cmd.SetRenderTarget(source);
	            cmd.Blit(g_BlurTexture, source, lastMat, 2);

	            cmd.ReleaseTemporaryRT(g_BlurTexture2);
	            cmd.ReleaseTemporaryRT(g_BlurTexture);

	        }

	        context.ExecuteCommandBuffer(cmd);
	        CommandBufferPool.Release(cmd);
	    }

	    private Material _getIterationMat(int _iteration)
	    {
		    Material mat;
		    if (_m_lIterationMats.Count <= _iteration)
		    {
			    for (int i = 0; i < _iteration - _m_lIterationMats.Count + 1; i++)
			    {
				    _m_lIterationMats.Add(new Material(_m_blurMaterial));
			    }
		    }
		    mat = _m_lIterationMats[_iteration];

		    if (mat == null)
		    {
			    _m_lIterationMats[_iteration] = new Material(_m_blurMaterial);
			    mat = _m_lIterationMats[_iteration];
		    }
		    return mat;
	    }
	}
}