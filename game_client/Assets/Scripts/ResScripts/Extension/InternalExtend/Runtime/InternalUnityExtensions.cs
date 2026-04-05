using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public static class InternalUnityExtensions
{
    #region Sprite
    
      public static unsafe void DeformPlanarShadow(Sprite sprite, NativeSlice<Vector3> vertices, NativeArray<byte> deformableVertices, float3 _shadowPlane, float3 _lightDir, float _shadowPlaneHeight)
        {
            var verticesFloat3 = vertices.SliceWithStride<float3>();

            var spriteVertexCount = sprite.GetVertexCount();
            var spriteVertexStreamSize = sprite.GetVertexStreamSize();

            byte* deformedPosOffset = (byte*)NativeArrayUnsafeUtility.GetUnsafePtr(deformableVertices);            
            NativeSlice<float3> deformableVerticesFloat3 = NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<float3>(deformedPosOffset, spriteVertexStreamSize, spriteVertexCount);
            
    #if ENABLE_UNITY_COLLECTIONS_CHECKS
            var handle2 = CreateSafetyChecks<float3>(ref deformableVerticesFloat3);
    #endif
            float planeDotDir = math.dot(_shadowPlane, _lightDir);
            for (var i = 0; i < verticesFloat3.Length; i++)
            {
                var vertex = verticesFloat3[i];
                float distance = (_shadowPlaneHeight - math.dot(_shadowPlane, vertex)) / planeDotDir; //计算阴影的长度
                vertex = vertex + distance * _lightDir.xyz; //影子的投影点

                deformableVerticesFloat3[i] = vertex;
            }
    #if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSafetyChecks(handle2);
    #endif
        }
    #if ENABLE_UNITY_COLLECTIONS_CHECKS

        private static AtomicSafetyHandle CreateSafetyChecks<T>(ref NativeSlice<T> array) where T : struct
        {
            var handle = AtomicSafetyHandle.Create();
            AtomicSafetyHandle.SetAllowSecondaryVersionWriting(handle, true);
            AtomicSafetyHandle.UseSecondaryVersion(ref handle);
            NativeSliceUnsafeUtility.SetAtomicSafetyHandle<T>(ref array, handle);
            return handle;
        }

        private static void DisposeSafetyChecks(AtomicSafetyHandle handle)
        {
            AtomicSafetyHandle.Release(handle);
        }
    #endif
    
    public static int GetVertexStreamSize(this Sprite sprite)
    {
        int vertexStreamSize = 12;
        if (sprite.HasVertexAttribute(VertexAttribute.Normal))
            vertexStreamSize = vertexStreamSize + 12;
        if (sprite.HasVertexAttribute(VertexAttribute.Tangent))
            vertexStreamSize = vertexStreamSize + 16;
        return vertexStreamSize;
    }
    #endregion
   
}
