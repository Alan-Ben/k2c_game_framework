
using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// 连线风格枚举
/// </summary>
public enum ELineStyle
{
    [InspectorName("直线连接")]
    Line,              // 直线连接
    [InspectorName("正交连接")]
    Orthogonal,       // 正交连接（L形）
    [InspectorName("圆角正交")]
    RoundedOrthogonal, // 圆角正交连接
    [InspectorName("贝塞尔曲线")]
    Bezier,
}

public class GuildDungeonMapLineConfigMono : MonoBehaviour
{
    [Header("连线配置")]
    public Transform lineParent; // 连线的父节点
    public GameObject linePrefab; // 连线预制件

    [Header("连线状态材质")]
    [Tooltip("解锁状态的连接线材质（怪物血量为0或起点）")]
    public Material unlockedMaterial;
    [Tooltip("未解锁状态的连接线材质（怪物血量>0）")]
    public Material lockedMaterial;

    [FormerlySerializedAs("yPosOffset")] public float fromYPosOffset = 0.0f;
    public float toYPosOffset = 0.0f;
    public float xPosOffset = 0.0f;
    
    [Header("贝塞尔曲线参数")]
    [Range(0.0001f, 0.5f)]
    [Tooltip("连线的粗细程度")]
    public float lineWidth = 0.1f; // 线条宽度
    
    [Range(2, 50)]
    [Tooltip("曲线分段点数，越多越平滑但性能消耗越大")]
    public int curvePoints = 20; // 曲线分段点数
    
    [Header("连线风格配置")]
    public ELineStyle lineStyle = ELineStyle.Bezier; // 连线风格

    [Range(-0.0f, 3.0f)]
    [Header("曲线弯曲强度")]
    [Tooltip("曲线弯曲强度，1.0为标准弯曲\n• Bezier: 控制弯曲程度\n• RoundedOrthogonal: 无效果\n• Orthogonal/Line: 无效果")]
    public float curveStrength;
    
    [Header("圆角正交参数")]
    [Tooltip("圆角正交线条的圆角半径（绝对值，单位同场景坐标）\n仅对 RoundedOrthogonal 线型有效\n推荐值：20-100")]
    public float cornerRadius = 50.0f;
    
    [Header("贝塞尔曲线控制点")]
    [Header("开启则根据数量上层>下层，反转曲线")]
    public bool allowInvertPointOrder = true;
    [Range(-2.0f, 2.0f)]
    public float bezierStartX = 0.0f;
    [Range(-2.0f, 2.0f)]
    [Tooltip("Bezier曲线: Y方向控制点偏移")]
    public float bezierStartY = 1.0f;
    [Range(-2.0f, 2.0f)]
    public float bezierEndX = 1.0f;
    [Range(-2.0f, 2.0f)]
    public float bezierEndY = 0.0f;
    
    // <AutoGen:MonoDeclaration>
    // </AutoGen:MonoDeclaration>

#if NP_GAME && UNITY_EDITOR
    private void OnValidate()
    {
        GOE.GGUIWndGuildDungeonMap.instance.updateCurve();
        // 参数范围校验
        curvePoints = Mathf.Clamp(curvePoints, 2, 50);
        lineWidth = Mathf.Clamp(lineWidth, 0.01f, 1.0f);
        
        // 根据连线风格调整参数提示
        switch (lineStyle)
        {
            case ELineStyle.Orthogonal:
            case ELineStyle.Line:
                // 正交连接不需要太多点
                if (curvePoints > 10)
                {
                    Debug.LogWarning("正交连接风格建议使用较少的曲线点数(3-10)以提高性能");
                }
                break;
                
            case ELineStyle.RoundedOrthogonal:
                // 圆角正交需要足够的点数来绘制圆滑的圆角
                if (curvePoints < 10 || curvePoints > 30)
                {
                    Debug.Log("圆角正交风格推荐使用10-30个曲线点数以获得平滑圆角");
                }
                break;
                
            case ELineStyle.Bezier:
                // 树状图风格推荐中等点数
                if (curvePoints < 15 || curvePoints > 30)
                {
                    Debug.Log("贝塞尔曲线风格推荐使用15-30个曲线点数以获得最佳效果");
                }
                break;
        }
    }
#endif
}
