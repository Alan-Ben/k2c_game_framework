using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class TMP_WarpUIEffect : TMP_EffectBase
{
    public enum EffectType
    {
        Circle,
        Curve
    }
    
    [Header("变形效果设置")]
    [SerializeField]
    [Tooltip("变形类型：Circle=圆形/双弧变形，Curve=自定义曲线变形")]
    public EffectType effectType = EffectType.Circle;
    
    [Header("圆形/双弧参数")]
    [SerializeField]
    [Tooltip("主半径：控制圆形的弧度大小")]
    public int radius = 100;

    [SerializeField]
    [Tooltip("间距系数：调整字符在圆弧上的分布密度")]
    public float spaceCoff = 1f;
    
    [SerializeField]
    [Tooltip("下弧偏移：设为0时为标准圆形，非0时为双弧效果（上下弧度不同）")]
    public int lowerRadiusOffset = 0;
    
    [Header("自定义曲线参数")]
    [SerializeField]
    [Tooltip("垂直变形曲线：定义文字沿水平方向的垂直位移")]
    public AnimationCurve verticalCurve = AnimationCurve.Linear(0, 0, 1, 0);
    
    [SerializeField]
    [Tooltip("曲线缩放系数：控制曲线效果的强度")]
    public float curveScale = 50f;
    

    protected override void ModifyMesh()
    {
        if (!IsActive() || radius == 0)
            return;

        m_textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_textComponent.textInfo;
        int characterCount = textInfo.characterCount;

        if (characterCount == 0)
            return;

        Vector3[] vertices;

        for (int i = 0; i < characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            vertices = textInfo.meshInfo[materialIndex].vertices;

            Vector3 center = Vector3.Lerp(vertices[vertexIndex + 0], vertices[vertexIndex + 2], 0.5f);
            
            if (effectType == EffectType.Circle)
            {
                if (lowerRadiusOffset == 0)
                {
                    ApplyCircleEffect(vertices, vertexIndex, center);
                }
                else
                {
                    ApplyDualArcEffect(vertices, vertexIndex, center);
                }
            }
            else if (effectType == EffectType.Curve)
            {
                ApplyCurveEffect(vertices, vertexIndex, center, i);
            }
        }

        m_textComponent.UpdateVertexData();
    }
    
    private void ApplyCircleEffect(Vector3[] vertices, int vertexIndex, Vector3 center)
    {
        Matrix4x4 move = Matrix4x4.TRS(center * -1, Quaternion.identity, Vector3.one);
        
        float rad = Mathf.PI / 2 - center.x * spaceCoff / radius;
        Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        Quaternion rotation = Quaternion.Euler(0, 0, rad * 180 / Mathf.PI - 90);
        Matrix4x4 rotate = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        Matrix4x4 place = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
        Matrix4x4 transform = place * rotate * move;

        vertices[vertexIndex + 0] = transform.MultiplyPoint(vertices[vertexIndex + 0]);
        vertices[vertexIndex + 1] = transform.MultiplyPoint(vertices[vertexIndex + 1]);
        vertices[vertexIndex + 2] = transform.MultiplyPoint(vertices[vertexIndex + 2]);
        vertices[vertexIndex + 3] = transform.MultiplyPoint(vertices[vertexIndex + 3]);
        
        vertices[vertexIndex + 0].y = vertices[vertexIndex + 0].y - radius + center.y;
        vertices[vertexIndex + 1].y = vertices[vertexIndex + 1].y - radius + center.y;
        vertices[vertexIndex + 2].y = vertices[vertexIndex + 2].y - radius + center.y;
        vertices[vertexIndex + 3].y = vertices[vertexIndex + 3].y - radius + center.y;
    }
    
    private void ApplyDualArcEffect(Vector3[] vertices, int vertexIndex, Vector3 center)
    {
        int lowerRadius = radius + lowerRadiusOffset;
        
        Matrix4x4 move = Matrix4x4.TRS(center * -1, Quaternion.identity, Vector3.one);
        
        // Calculate upper arc transformation (using main radius)
        float upperRad = Mathf.PI / 2 - center.x * spaceCoff / radius;
        Vector3 upperPos = new Vector3(Mathf.Cos(upperRad), Mathf.Sin(upperRad), 0) * radius;
        Quaternion upperRotation = Quaternion.Euler(0, 0, upperRad * 180 / Mathf.PI - 90);
        Matrix4x4 upperRotate = Matrix4x4.TRS(Vector3.zero, upperRotation, Vector3.one);
        Matrix4x4 upperPlace = Matrix4x4.TRS(upperPos, Quaternion.identity, Vector3.one);
        Matrix4x4 upperTransform = upperPlace * upperRotate * move;
        
        // Calculate lower arc transformation (using radius + offset)
        float lowerRad = Mathf.PI / 2 - center.x * spaceCoff / lowerRadius;
        Vector3 lowerPos = new Vector3(Mathf.Cos(lowerRad), Mathf.Sin(lowerRad), 0) * lowerRadius;
        Quaternion lowerRotation = Quaternion.Euler(0, 0, lowerRad * 180 / Mathf.PI - 90);
        Matrix4x4 lowerRotate = Matrix4x4.TRS(Vector3.zero, lowerRotation, Vector3.one);
        Matrix4x4 lowerPlace = Matrix4x4.TRS(lowerPos, Quaternion.identity, Vector3.one);
        Matrix4x4 lowerTransform = lowerPlace * lowerRotate * move;

        // For TextMeshPro: vertices[0] = bottom-left, vertices[1] = top-left, vertices[2] = top-right, vertices[3] = bottom-right
        // Apply upper transform to top vertices (1,2) and lower transform to bottom vertices (0,3)
        vertices[vertexIndex + 1] = upperTransform.MultiplyPoint(vertices[vertexIndex + 1]);
        vertices[vertexIndex + 2] = upperTransform.MultiplyPoint(vertices[vertexIndex + 2]);
        vertices[vertexIndex + 0] = lowerTransform.MultiplyPoint(vertices[vertexIndex + 0]);
        vertices[vertexIndex + 3] = lowerTransform.MultiplyPoint(vertices[vertexIndex + 3]);
        
        // Adjust y positions
        vertices[vertexIndex + 1].y = vertices[vertexIndex + 1].y - radius + center.y;
        vertices[vertexIndex + 2].y = vertices[vertexIndex + 2].y - radius + center.y;
        vertices[vertexIndex + 0].y = vertices[vertexIndex + 0].y - lowerRadius + center.y;
        vertices[vertexIndex + 3].y = vertices[vertexIndex + 3].y - lowerRadius + center.y;
    }
    
    private void ApplyCurveEffect(Vector3[] vertices, int vertexIndex, Vector3 center, int charIndex)
    {
        // Get RectTransform width for normalization
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) return;
        
        float rectWidth = rectTransform.rect.width;
        float rectMinX = rectTransform.rect.xMin;
        
        // Calculate normalized position based on character's X position within RectTransform width
        float normalizedPos = (center.x - rectMinX) / rectWidth;
        normalizedPos = Mathf.Clamp01(normalizedPos);
        
        // Sample the curve for vertical offset
        float verticalOffset = verticalCurve.Evaluate(normalizedPos) * curveScale;
        
        // Calculate curve slope for character rotation
        float deltaTime = 0.01f;
        float nextPos = Mathf.Clamp01(normalizedPos + deltaTime);
        float prevPos = Mathf.Clamp01(normalizedPos - deltaTime);
        
        float nextY = verticalCurve.Evaluate(nextPos) * curveScale;
        float prevY = verticalCurve.Evaluate(prevPos) * curveScale;
        
        // Calculate slope and rotation angle
        float slope = (nextY - prevY) / (deltaTime * 2 * rectWidth);
        float angle = Mathf.Atan(slope) * Mathf.Rad2Deg;
        
        // Create transformation matrix
        Matrix4x4 move = Matrix4x4.TRS(center * -1, Quaternion.identity, Vector3.one);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Matrix4x4 rotate = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        Vector3 newPos = new Vector3(center.x, center.y + verticalOffset, 0);
        Matrix4x4 place = Matrix4x4.TRS(newPos, Quaternion.identity, Vector3.one);
        Matrix4x4 transform = place * rotate * move;
        
        // Apply transformation
        vertices[vertexIndex + 0] = transform.MultiplyPoint(vertices[vertexIndex + 0]);
        vertices[vertexIndex + 1] = transform.MultiplyPoint(vertices[vertexIndex + 1]);
        vertices[vertexIndex + 2] = transform.MultiplyPoint(vertices[vertexIndex + 2]);
        vertices[vertexIndex + 3] = transform.MultiplyPoint(vertices[vertexIndex + 3]);
    }
    
    protected bool IsActive()
    {
        return gameObject.activeInHierarchy && enabled;
    }
}