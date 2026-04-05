

using JetBrains.Annotations;
using GOE;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 一个轴对齐椭圆形的点击区域
/// </summary>
/// <remarks>
/// 点击范围是这个 RectTransform 的最大内接椭圆，
/// 只支持轴对齐的椭圆
/// </remarks>
public class NPAxisAlignedEllipseClickTriggerMono : NPClickTriggerMono
{
#if NP_GAME
    public override bool Raycast(Vector2 sp, Camera eventCamera)
    {
        // 做椭圆检测
        bool isValid = true;
        if (isActiveAndEnabled && rectTransform != null && eventCamera != null)
        {
            Vector3 worldPoint = eventCamera.ScreenToWorldPoint(sp);
            Vector2 localPoint = worldPoint;
            if (rectTransform.parent != null)
                localPoint = rectTransform.parent.InverseTransformPoint(worldPoint);
            
            // 判断是否在椭圆内
            Rect rect = rectTransform.rect;
            isValid = NPGameUtility.isPointInEllipse(rect.center, rect.width / 2f, rect.height / 2f, localPoint);
        }

        // 通过检测了，再走底层的检测
        if (isValid)
            return base.Raycast(sp, eventCamera);
        
        // 没通过检测直接返回 false
        return false;
    }
#endif

#if UNITY_EDITOR
    [NotNull] private Vector3[] _m_pointList = new Vector3[50];
    public void OnDrawGizmosSelected()
    {
        if (rectTransform == null || !isActiveAndEnabled)
            return;
        
        // 画一个最大内接椭圆
        Rect rect = rectTransform.rect;
        Vector3 center = rectTransform.TransformPoint(new Vector3(rect.center.x, rect.center.y, rectTransform.localPosition.z));
        float a = rect.width / rectTransform.lossyScale.x / 2f;
        float b = rect.height / rectTransform.lossyScale.y / 2f;
        DebugPlus.MakeEllipsePoint(center, a, b, _m_pointList);
        for (int i = 0; i < _m_pointList.Length - 1; i++)
        {
            Gizmos.DrawLine(_m_pointList[i], _m_pointList[i + 1]);
        }
        Handles.DrawLine(_m_pointList[_m_pointList.Length - 1], _m_pointList[0]);
    }
#endif
}