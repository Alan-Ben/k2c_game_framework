#ifndef OUTLINE
#define OUTLINE

float4 TransformObjectToHClip_OutlineScreenNDC(float4 positionOS, float3 normalOS, float width)
{
    float4 pos = mul(UNITY_MATRIX_MV, positionOS);//将坐标从模型空间转换到视角空间
    float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, normalOS);//将法线从模型空间转换到视角空间
    normal.z = -0.5;//让法线的z分量等于一个定值，避免背面扩张后的顶点挡住正面的面片
    pos = pos + float4(normalize(normal), 0) * width;//将顶点沿法线方向扩张
    return mul(UNITY_MATRIX_P, pos);//将顶点从视角空间转换到裁剪空间
}


float4 TransformObjectToHClip_OutlineScreenNDCSameWidth(float4 positionOS, float3 normalOS, float width)
{
    float4 pos = TransformObjectToHClip(positionOS);
    float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, normalOS);
    float3 ndcNormal = normalize(TransformWViewToHClip(viewNormal.xyz)) * pos.w;//将法线变换到NDC空间
    float4 nearUpperRight = mul(unity_CameraInvProjection, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));//将近裁剪面右上角位置的顶点变换到观察空间
    float aspect = abs(nearUpperRight.y / nearUpperRight.x);//求得屏幕宽高比
    ndcNormal.x *= aspect;
    pos.xy += 0.01 * width * ndcNormal.xy;
    return pos;
}
#endif