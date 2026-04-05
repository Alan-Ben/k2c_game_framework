Shader "Hidden/Universal Render Pipeline/SeparableSubsurfaceScatterV3" {
    Properties 
    {
        _RefValue ("Ref Value", Float) = 2
    	_SSSColorTex("_SSSColorTex", 2D) = "white" {}

    }
    
	HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

	#define nSamples 9

	float _SSSSDepthFalloff;
	float _DistanceToProjectionWindow;
	float2 _SSSSDirection;
	float4 _Kernel[nSamples];
	
	TEXTURE2D_X(_SourceTex);
	float4 _SourceTex_TexelSize;
	TEXTURE2D_X(_SSSColorTex);


    float4 SampleTexture(float2 uv)
    {
		float4 tex = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_PointClamp,uv);
    	tex.a *= 93	;
    	return tex;
    }

	void ClipDepth(float depth)
    {
	    clip(depth/93 -0.001 );
    }
    float4 sampleSSS(float2 uv, float2 _uvOffset, float4 sssCol, float _depthM)
    {
    	// 模糊操作中，正反颜色是相同，所以取样一次sssColor,可以用两次
	    float2 uvOffset = _uvOffset * sssCol.w;
		float4 texCol = SampleTexture(uv + uvOffset * float2(-1.0, -1.0));
		texCol.w = step(0.0, texCol.w) * (1.0 - saturate(0.03 * _SSSSDepthFalloff * abs(_depthM - texCol.w)));
		texCol.xyz = texCol.xyz * texCol.www;

		float4 texCol2 = SampleTexture( uv + uvOffset);
		texCol2.w = step(0.0, texCol2.w) * (1.0 - saturate(0.03 * _SSSSDepthFalloff * abs(_depthM - texCol2.w)));
		texCol2.xyz = texCol2.xyz * texCol2.www;
    	return texCol + texCol2;
    }

	
    #pragma target 3.0
    ENDHLSL

    SubShader 
	{
		ZTest Always ZWrite Off Cull Off
        Pass 
		{
            Name "Separable Pass 1"

            Stencil
            {
                Ref [_RefValue]
                Comp equal
            }

            HLSLPROGRAM
            
            #pragma multi_compile _ SSSS_FOLLOW_SURFACE
            #pragma vertex FullscreenVert
            #pragma fragment frag

            float4 frag(Varyings i) : SV_TARGET {

                float2 texcoord = i.uv.xy;
                float4 colorM = SampleTexture( texcoord);

            	if (colorM.a <= 0.0)
				{            	
					float4 tex1 = SampleTexture(texcoord + float2(-1.0, 0.0) * _SourceTex_TexelSize.xy);
					float4 tex2 = SampleTexture(texcoord + float2(1.0, 0.0) * _SourceTex_TexelSize.xy);
					float4 tex3 = SampleTexture(texcoord + float2(0.0, -1.0) * _SourceTex_TexelSize.xy);
					float4 tex4 = SampleTexture(texcoord + float2(0.0, 1.0) * _SourceTex_TexelSize.xy);
		
					colorM = (tex1 + tex2 + tex3 + tex4) / max(4 - step(tex1.w, 0.0) - step(tex2.w, 0.0) - step(tex3.w, 0.0) - step(tex4.w, 0.0), 0.0099999998);
				}
            	float depthM = colorM.a ;
            	ClipDepth(depthM);

				float2 uvOffset =  _DistanceToProjectionWindow * (1.0 / depthM ) * _SSSSDirection.xy ;

            	float4 sssCol  = _Kernel[0];
				float3 colSum = colorM.xyz * sssCol.xyz;
				float3 sssColSum = sssCol.xyz;

            	sssCol = _Kernel[1];
				float4 texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[2];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[3];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[4];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[5];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[6];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;
            	
            	sssCol = _Kernel[7];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;
            	
            	sssCol = _Kernel[8];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	float4 result = float4(colSum / max(sssColSum, 0.001), depthM/93);
            	return result;
            }
            ENDHLSL
        } 
	
		Pass 
		{
            Name "Separable Pass 2"

            Stencil
            {
                Ref [_RefValue]
                Comp equal
            }

            HLSLPROGRAM
            
            #pragma multi_compile _ SSSS_FOLLOW_SURFACE
            #pragma vertex FullscreenVert
            #pragma fragment frag

            
            float4 frag(Varyings i) : SV_TARGET {

                float2 texcoord = i.uv.xy;

            	
                float4 colorM = SampleTexture( texcoord);

				float depthM = colorM.a;
            	ClipDepth(depthM);

				float2 uvOffset =  _DistanceToProjectionWindow * (1.0 / depthM ) * _SSSSDirection.xy ;
            	
          		float4 sssCol  = _Kernel[0];
				float3 colSum = colorM.xyz * sssCol.xyz;
				float3 sssColSum = sssCol.xyz;

             	sssCol = _Kernel[1];
				float4 texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[2];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[3];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[4];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[5];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	sssCol = _Kernel[6];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;
            	
            	sssCol = _Kernel[7];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;
            	
            	sssCol = _Kernel[8];
				texSSS = sampleSSS(texcoord, uvOffset, sssCol, depthM);
            	colSum.xyz += sssCol.xyz * texSSS.xyz;
				sssColSum.xyz += sssCol.xyz * texSSS.www;

            	float4 result = float4(colSum / max(sssColSum, 0.001), depthM/93);
            	return result;
            }
            ENDHLSL
        } 
    }
}
