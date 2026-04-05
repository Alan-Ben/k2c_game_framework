using UnityEngine;
using UnityEngine.UI;
 
public class WarpUIEffect : BaseMeshEffect
{
    public enum EffectType
    {
        Circle,
        Curve
    }
    [Header("如果有和Shadow/Outline一起用，要放到Shadow/Outline之前")]
    [Header("如果改了预制体没有修改，则把文字的预制体去掉，可以生效")]
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
    public override void ModifyMesh(VertexHelper vh)
    {
        if(!IsActive() || radius == 0)
        {
            return;
        }
 
        UIVertex lb = new UIVertex(); // bottom-left
        UIVertex lt = new UIVertex(); // top-left
        UIVertex rt = new UIVertex(); // top-right
        UIVertex rb = new UIVertex(); // bottom-right
 
        for(int i = 0; i < vh.currentVertCount / 4; i++)
        {
            vh.PopulateUIVertex(ref rt, i * 4 + 0);
            vh.PopulateUIVertex(ref lt, i * 4 + 1);
            vh.PopulateUIVertex(ref rb, i * 4 + 2);
            vh.PopulateUIVertex(ref lb, i * 4 + 3);
 
            Vector3 center = Vector3.Lerp(lb.position, rt.position, 0.5f);
            
            if (effectType == EffectType.Circle)
            {
                if (lowerRadiusOffset == 0)
                {
                    ApplyCircleEffect(ref lb, ref lt, ref rt, ref rb, center);
                }
                else
                {
                    ApplyDualArcEffect(ref lb, ref lt, ref rt, ref rb, center);
                }
            }
            else if (effectType == EffectType.Curve)
            {
                ApplyCurveEffect(ref lb, ref lt, ref rt, ref rb, center, i, vh.currentVertCount);
            }
 
            vh.SetUIVertex(rt, i * 4 + 0);
            vh.SetUIVertex(lt, i * 4 + 1);
            vh.SetUIVertex(rb, i * 4 + 2);
            vh.SetUIVertex(lb, i * 4 + 3);
        }
    }
    
    private void ApplyCircleEffect(ref UIVertex lb, ref UIVertex lt, ref UIVertex rt, ref UIVertex rb, Vector3 center)
    {
        Matrix4x4 move = Matrix4x4.TRS(center * -1, Quaternion.identity, Vector3.one);
        float rad = Mathf.PI / 2 - center.x * spaceCoff / radius;
        Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        Quaternion rotation = Quaternion.Euler(0, 0, rad * 180 / Mathf.PI - 90);
        Matrix4x4 rotate = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        Matrix4x4 place = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
        Matrix4x4 transform = place * rotate * move;

        lb.position = transform.MultiplyPoint(lb.position);
        lt.position = transform.MultiplyPoint(lt.position);
        rt.position = transform.MultiplyPoint(rt.position);
        rb.position = transform.MultiplyPoint(rb.position);
        lb.position.y = lb.position.y - radius + center.y;
        lt.position.y = lt.position.y - radius + center.y;
        rt.position.y = rt.position.y - radius + center.y;
        rb.position.y = rb.position.y - radius + center.y;
    }
    
    private void ApplyDualArcEffect(ref UIVertex lb, ref UIVertex lt, ref UIVertex rt, ref UIVertex rb, Vector3 center)
    {
        Matrix4x4 move = Matrix4x4.TRS(center * -1, Quaternion.identity, Vector3.one);
        
        int lowerRadius = radius + lowerRadiusOffset;
        
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

        // Apply transformations: lt(top-left) and rt(top-right) use upper arc, lb(bottom-left) and rb(bottom-right) use lower arc
        lt.position = upperTransform.MultiplyPoint(lt.position);
        rt.position = upperTransform.MultiplyPoint(rt.position);
        lb.position = lowerTransform.MultiplyPoint(lb.position);
        rb.position = lowerTransform.MultiplyPoint(rb.position);
        
        // Adjust y positions
        lt.position.y = lt.position.y - radius + center.y;
        rt.position.y = rt.position.y - radius + center.y;
        lb.position.y = lb.position.y - lowerRadius + center.y;
        rb.position.y = rb.position.y - lowerRadius + center.y;
    }
    
    private void ApplyCurveEffect(ref UIVertex lb, ref UIVertex lt, ref UIVertex rt, ref UIVertex rb, Vector3 center, int charIndex, int totalVertCount)
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
        lb.position = transform.MultiplyPoint(lb.position);
        lt.position = transform.MultiplyPoint(lt.position);
        rt.position = transform.MultiplyPoint(rt.position);
        rb.position = transform.MultiplyPoint(rb.position);
    }
}