using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("UI/Effects/ThreeColorGradient")]
[RequireComponent(typeof(Graphic))]
public class ThreeColorGradient : BaseMeshEffect
{
    public Color topColor = Color.white;
    public Color centerColor = Color.grey;
    public Color bottomColor = Color.black;
    public bool MultiplyTextColor = false;

    protected ThreeColorGradient()
    { }
    public static Color32 Multiply(Color32 a, Color32 b)
    {
        a.r = (byte)((a.r * b.r) >> 8);
        a.g = (byte)((a.g * b.g) >> 8);
        a.b = (byte)((a.b * b.b) >> 8);
        a.a = (byte)((a.a * b.a) >> 8);
        return a;
    }

    private void ModifyVertices(VertexHelper vh)
    {
        List<UIVertex> verts = new List<UIVertex>(vh.currentVertCount);
        vh.GetUIVertexStream(verts);
        vh.Clear();
        int step = 6;
        for (int i = 0; i < verts.Count; i += step)
        {
            //6 point
            var tl = multiplyColor(verts[i + 0], topColor);
            var tr = multiplyColor(verts[i + 1], topColor);
            var bl = multiplyColor(verts[i + 4], bottomColor);
            var br = multiplyColor(verts[i + 3], bottomColor);
            var cl = calcCenterVertex(verts[i + 0], verts[i + 4]);
            var cr = calcCenterVertex(verts[i + 1], verts[i + 2]);

            vh.AddVert(tl);
            vh.AddVert(tr);
            vh.AddVert(cl);
            vh.AddVert(cr);
            vh.AddVert(bl);
            vh.AddVert(br);
        }

        for (int i = 0; i < vh.currentVertCount; i += step)
        {
            vh.AddTriangle(i + 0, i + 1, i + 3);
            vh.AddTriangle(i + 3, i + 2, i + 0);
            vh.AddTriangle(i + 2, i + 3, i + 5);
            vh.AddTriangle(i + 5, i + 4, i + 2);
        }
    }
    private UIVertex multiplyColor(UIVertex vertex, Color color)
    {
        if (MultiplyTextColor)
            vertex.color = Multiply(vertex.color, color);
        else
            vertex.color = color;
        return vertex;
    }
    private UIVertex calcCenterVertex(UIVertex top, UIVertex bottom)
    {
        UIVertex center = new UIVertex();
        center.normal = (top.normal + bottom.normal) / 2;
        center.position = (top.position + bottom.position) / 2;
        center.tangent = (top.tangent + bottom.tangent) / 2;
        center.uv0 = (top.uv0 + bottom.uv0) / 2;
        center.uv1 = (top.uv1 + bottom.uv1) / 2;
        if (MultiplyTextColor)
        {
            //multiply color
            var color = Color.Lerp(top.color, bottom.color, 0.5f);
            center.color = Multiply(color, centerColor);
        }
        else
        {
            center.color = centerColor;
        }
        return center;
    }
    #region implemented abstract members of BaseMeshEffect
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!this.IsActive())
        {
            return;
        }
        ModifyVertices(vh);
    }
    #endregion
}