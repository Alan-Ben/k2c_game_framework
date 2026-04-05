using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RawImageMesh : RawImage
{
    public Mesh m_mesh;
    public Vector3 m_overrideScale = new Vector3(100f, 100f, 100f);
    public bool exchangeYZ = true;
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (m_mesh != null)
        {
            if (!m_mesh.isReadable)
            {
                m_mesh = null;
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("该模型未开启Readable！，无法使用该模型");
#endif
                base.OnPopulateMesh(vh);
                return;
            }

            int[] triangles = m_mesh.triangles;
            Vector3[] vertices = m_mesh.vertices;
            Vector2[] uvs = m_mesh.uv;
            Color[] colors = m_mesh.colors;
            bool hasUVs = uvs.Length == vertices.Length;
            bool hasColors = colors.Length == vertices.Length;

            if (exchangeYZ)
            {
                // 如果数量不匹配，直接返回
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 v = vertices[i];
                    Vector3 pos = Vector3.Scale(new Vector3(v.x,v.z,v.y), m_overrideScale);
                    Color32 col = hasColors? color * colors[i] : color;
                    Vector2 uv = hasUVs ? uvs[i] : Vector2.zero;
                    //添加顶点
                    vh.AddVert(pos, col, uv);
                }
            }
            else
            {
                // 如果数量不匹配，直接返回
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 v = vertices[i];
                    Vector3 pos = Vector3.Scale(vertices[i], m_overrideScale);
                    Color32 col = hasColors? color * colors[i] : color;
                    Vector2 uv = hasUVs ? uvs[i] : Vector2.zero;
                    //添加顶点
                    vh.AddVert(pos, col, uv);
                }
            }
          
            //设置三角形索引
            for (int i = 0; i < triangles.Length; i += 3)
            {
                vh.AddTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
            }
        }
        else
        {
            base.OnPopulateMesh(vh);
        }
    }
}
