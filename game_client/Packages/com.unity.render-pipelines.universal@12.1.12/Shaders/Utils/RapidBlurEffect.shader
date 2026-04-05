Shader "PostProcess/Rapid Blur Effect"
{
	//-----------------------------------【属性 || Properties】------------------------------------------  
	Properties
	{
		//主纹理
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
 
	//----------------------------------【子着色器 || SubShader】---------------------------------------  
	SubShader
	{
		ZWrite Off
		Blend Off
 
		//---------------------------------------【通道0 || Pass 0】------------------------------------
		//通道0：降采样通道 ||Pass 0: Down Sample Pass
		Pass
		{
			Name "DOWN_SAMPLE"
			ZTest Off
			Cull Off
 
			CGPROGRAM
 
			#pragma vertex vert_DownSmpl
			#pragma fragment frag_DownSmpl
 
			ENDCG
 
		}
 
		//---------------------------------------【通道1 || Pass 1】------------------------------------
		//通道1：垂直方向模糊处理通道 ||Pass 1: Vertical Pass
		Pass
		{
			Name "BLUR_VERTICAL"
			ZTest Always
			Cull Off
 
			CGPROGRAM
 
			#pragma vertex vert_BlurVertical
			#pragma fragment frag_Blur
 
			ENDCG
		}
 
		//---------------------------------------【通道2 || Pass 2】------------------------------------
		//通道2：水平方向模糊处理通道 ||Pass 2: Horizontal Pass
		Pass
		{
			Name "BLUR_HORIZONTAL"
			ZTest Always
			Cull Off
 
			CGPROGRAM
 
			#pragma vertex vert_BlurHorizontal
			#pragma fragment frag_Blur
 
			ENDCG
		}
	}
 
 
	CGINCLUDE
 
	#include "UnityCG.cginc"
 
	sampler2D _MainTex;
	//UnityCG.cginc中内置的变量，纹理中的单像素尺寸|| it is the size of a texel of the texture
	uniform half4 _MainTex_TexelSize;
	//C#脚本控制的变量 || Parameter
	uniform half _DownSampleValue;
	uniform half4 _BlurColor;
 
	struct VertexInput
	{
		float4 vertex : POSITION;
		half2 texcoord : TEXCOORD0;
	};
 
	// 降采样输出结构体 || Vertex Input Struct
	struct VertexOutput_DownSmpl
	{
		//像素位置坐标
		float4 pos : SV_POSITION;
		//（右上）
		half2 uv20 : TEXCOORD0;
		//（左下）
		half2 uv21 : TEXCOORD1;
		//（右下）
		half2 uv22 : TEXCOORD2;
		//（左上）
		half2 uv23 : TEXCOORD3;
	};
 
 
	// 准备高斯模糊权重矩阵参数7x4的矩阵 ||  Gauss Weight
	static const half4 GaussWeight[7] =
	{
		half4(0.0205,0.0205,0.0205,0),
		half4(0.0855,0.0855,0.0855,0),
		half4(0.232,0.232,0.232,0),
		half4(0.324,0.324,0.324,1),
		half4(0.232,0.232,0.232,0),
		half4(0.0855,0.0855,0.0855,0),
		half4(0.0205,0.0205,0.0205,0)
	};
 
 
	VertexOutput_DownSmpl vert_DownSmpl(VertexInput v)
	{
		VertexOutput_DownSmpl o;
 
		o.pos = UnityObjectToClipPos(v.vertex);

		o.uv20 = v.texcoord + _MainTex_TexelSize.xy* half2(0.5h, 0.5h);;
		o.uv21 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h, -0.5h);
		o.uv22 = v.texcoord + _MainTex_TexelSize.xy * half2(0.5h, -0.5h);
		o.uv23 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h, 0.5h);
 
		return o;
	}
 
	fixed4 frag_DownSmpl(VertexOutput_DownSmpl i) : SV_Target
	{
		fixed4 color = fixed4(0,0,0,0);
 
		color += tex2D(_MainTex, i.uv20);
		color += tex2D(_MainTex, i.uv21);
		color += tex2D(_MainTex, i.uv22);
		color += tex2D(_MainTex, i.uv23);
 
		return color / 4 * _BlurColor;
	}
 
	struct VertexOutput_Blur
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half2 offset : TEXCOORD1;
	};
 
	VertexOutput_Blur vert_BlurHorizontal(VertexInput v)
	{
		VertexOutput_Blur o;
 
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		o.offset = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _DownSampleValue;
 
		return o;
	}
 
	VertexOutput_Blur vert_BlurVertical(VertexInput v)
	{
		VertexOutput_Blur o;
 
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		o.offset = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _DownSampleValue;
 
		return o;
	}
 
	half4 frag_Blur(VertexOutput_Blur i) : SV_Target
	{ 
		//从中心点偏移3个间隔，从最左或最上开始加权累加
		half2 uv_withOffset = i.uv.xy - i.offset * 3.0;
 
		//循环获取加权后的颜色值
		half4 color = 0;
		for (int j = 0; j< 7; j++)
		{
			//偏移后的像素纹理值
			half4 texCol = tex2D(_MainTex, uv_withOffset);
			//待输出颜色值+=偏移后的像素纹理值 x 高斯权重
			color += texCol * GaussWeight[j];
			//移到下一个像素处，准备下一次循环加权
			uv_withOffset += i.offset;
		}
        color.a = 1;
		return color;
	}
 		
	ENDCG
 
	FallBack Off
}