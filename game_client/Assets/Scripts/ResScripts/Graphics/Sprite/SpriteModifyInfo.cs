using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace GOE
{
    /// <summary>
    /// Sprite 顶点信息修改，用于摇摆
    /// </summary>
    [Serializable]
    public class SpriteModifyInfo
    {
        // None: 没有
        // Base Pos Y：根据Y轴坐标值控制摆动幅度，（实现根部不动，上面摆动的效果）
        // All：所有都摆动，一般是平放再地面的花之类的 （如下图）
        public enum WaveType
        {
            None,
            BasePosY,
            All
        }
        public Sprite sprite;
        public WaveType waveType = WaveType.None;
        [ALHeader("控制摆动的强度（振幅）")]
        public float waveStrength = 1;
        [ALHeader("控制摆动的位置上的差异（位置上的频率）（值越大，同样相邻的位置的变化越大）")]
        public float waveScale = 1;
        [ALHeader("摆动速度（时间上的频率）")]
        public float waveSpeed = 1;
        [ALHeader("是否接受阴影")]
        public bool receiveShadow = false;
        [ReadOnly]
        public float waveStartPosY;
        [ReadOnly]
        public float waveHeight;

        private Sprite _m_modifySprite;

        public SpriteModifyInfo()
        {
            
        }

        public SpriteModifyInfo(SpriteModifyInfo _info)
        {
            sprite = _info.sprite;
            waveType = _info.waveType;
            waveStrength = _info.waveStrength;
            waveScale = _info.waveScale;
            waveSpeed = _info.waveSpeed;
            receiveShadow = _info.receiveShadow;
            waveStartPosY = _info.waveStartPosY;
            waveHeight = _info.waveHeight;
        }

        /// <summary>
        /// 获取修改后的sprite 信息
        /// </summary>
        /// <returns></returns>
        public Sprite getModifySprite()
        {
            if (_m_modifySprite != null)
                return _m_modifySprite;
            _m_modifySprite = UnityEngine.Object.Instantiate(sprite);
            Vector2[] vertices = _m_modifySprite.vertices;
            int vertexCount = vertices.Length;
            // NativeArray<Vector4> tangent = new NativeArray<Vector4>(vertices.Length,Allocator.Temp);
            // Vector4[] vTangent = new Vector4[vertexCount];
            
            // tangent的方式在实际使用时存在不稳定的问题（镜头移动，部分物体的数值会变小），暂不明确原因，可能是因为做了normal。所以改为用uv1 和uv2 来做
            NativeArray<Vector2> uv1 = new NativeArray<Vector2>(vertices.Length,Allocator.Temp);
            Vector2[] vUv1 = new Vector2[vertexCount];
            NativeArray<Vector2> uv2 = new NativeArray<Vector2>(vertices.Length,Allocator.Temp);
            Vector2[] vUv2 = new Vector2[vertexCount];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];

                float wave = (vertex.y - waveStartPosY) / waveHeight;
               
                wave = wave < 0 ? 0 : wave;
                switch (waveType)
                {
                    case WaveType.BasePosY:
                        break;
                    case WaveType.All:
                        wave = 1;
                        break;
                    case WaveType.None:
                        wave = 0;
                        break;
                    default:
                        wave = 0;
                        break;
                }
                float x =  waveStrength * wave;

                // vTangent[i] = new Vector4(x, waveScale, waveSpeed, receiveShadow ?  0 : 1);
                
                vUv1[i] = new Vector2(x, waveScale);
                vUv2[i] = new Vector2(waveSpeed, receiveShadow ? 0 : 1);

            }
            // tangent.CopyFrom(vTangent);
            // _m_modifySprite.SetVertexAttribute(VertexAttribute.Tangent, tangent);
            
            uv1.CopyFrom(vUv1);
            uv2.CopyFrom(vUv2);
            _m_modifySprite.SetVertexAttribute(VertexAttribute.TexCoord1, uv1);
            _m_modifySprite.SetVertexAttribute(VertexAttribute.TexCoord2, uv2);

            return _m_modifySprite;
        }

        public bool isEqual(SpriteModifyInfo _info)
        {
            return waveType == _info.waveType
                   && Math.Abs(waveStrength - _info.waveStrength) < 0.00001f
                   && Math.Abs(waveScale - _info.waveScale) < 0.0001f
                   && Math.Abs(waveSpeed - _info.waveSpeed) < 0.0001f
                   && receiveShadow == _info.receiveShadow
                   && Math.Abs(waveStartPosY - _info.waveStartPosY) < 0.0001f
                   && Math.Abs(waveHeight - _info.waveHeight) < 0.0001f ;
        }
    }
    
}