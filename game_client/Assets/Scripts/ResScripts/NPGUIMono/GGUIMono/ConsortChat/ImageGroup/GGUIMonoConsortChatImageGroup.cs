using System;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ConsortShotPos
{
    [Range(0, 1)]
    public float normalizeScale = 0.5f;
    [Range(0, 1)]
    public float normalizePosX = 0.5f;
    [Range(0, 1)]
    public float normalizePosY = 0.5f;
}

[Serializable]
public class ShotSetting
{
    public EConsortChatShotType shotType;
    public WCGFloatRange scaleRange = new WCGFloatRange(0, 1);
    public Rect scaleRect = new Rect(0, 0, 1, 1);

    public Vector2 getShot(float _normalizeScale, float _normalizePosX, float _normalizePosY, Vector2 _imgSize, Vector2 _viewSize, out float _scale)
    {
        // 1. 计算最终缩放值：根据归一化缩放值在缩放范围内插值
        _scale = scaleRange.min + (_normalizeScale * (scaleRange.max - scaleRange.min));

        // 2. 计算scaleRect在原图上的实际像素尺寸
        Vector2 visibleAreaSize = new Vector2(scaleRect.width * _imgSize.x, scaleRect.height * _imgSize.y);
        
        // 3. 计算scaleRect中心在原图上的位置（像素坐标，相对于图片中心）
        // 注意：UI坐标系需要翻转X和Y轴
        Vector2 visibleAreaCenter = new Vector2(
            -(scaleRect.x + scaleRect.width * 0.5f - 0.5f) * _imgSize.x,  // X轴翻转
            -(scaleRect.y + scaleRect.height * 0.5f - 0.5f) * _imgSize.y  // Y轴翻转
        );
        
        // 4. 计算缩放后可视区域的尺寸
        Vector2 scaledVisibleAreaSize = visibleAreaSize * _scale;
        
        // 5. 计算在可视区域内可以移动的范围（确保视野不超出可视区域边界）
        Vector2 movableRange = scaledVisibleAreaSize - _viewSize;
        movableRange = Vector2.Max(movableRange, Vector2.zero); // 确保不为负数
        
        // 6. 根据归一化位置计算在可视区域内的偏移
        // 注意：X和Y轴偏移都需要翻转
        Vector2 positionOffset = new Vector2(
            -(_normalizePosX - 0.5f) * movableRange.x,  // X轴翻转
            -(_normalizePosY - 0.5f) * movableRange.y   // Y轴翻转
        );
        
        // 7. 计算最终图像位置（可视区域中心 + 位置偏移）
        if (_scale == 0)
            return new Vector2(0,0);
        Vector2 finalPosition = (visibleAreaCenter + positionOffset / _scale) * _scale;
        
        return finalPosition;
    }
}

/// <summary>
/// 
/// </summary>
public class GGUIMonoConsortChatImageGroup : _AALBasicUIWndMono
{
    [ALHeader("背景图")]
    public RawImage imgBg;
    [ALHeader("妃子图")]
    public RawImage imgActor;

    [ALHeader("视图尺寸")]
    public Vector2 viewSize;

    public Transform imageBgParent;
    public Transform imageConsortParent;

    [ALHeader("镜头设置")]
    public ShotSetting bgShot;
    public ShotSetting consortShot;

    [ALHeader("位置参数")]
    public ConsortShotPos bgPos;
    public ConsortShotPos consortPos;

    [ALHeader("模糊材质")]
    public Material blurMat;

    [ALHeader("可视化设置")]
    [TextArea(2, 4)]
    [SerializeField] private string visualizationHelp = "可视化说明：\n• 实线框 = 图片边界范围\n• 虚线框 = ScaleRect可视区域";
    [SerializeField] [Tooltip("是否在Scene视图中显示调试可视化")] private bool showVisualization = false;
    [SerializeField] [Tooltip("显示背景图的可视区域")] private bool showBgViewArea = true;
    [SerializeField] [Tooltip("显示妃子图的可视区域")] private bool showConsortViewArea = true;
    [SerializeField] [Tooltip("背景图ScaleRect区域的显示颜色")] private Color bgScaleRectColor = Color.blue;
    [SerializeField] [Tooltip("妃子图ScaleRect区域的显示颜色")] private Color consortScaleRectColor = Color.red;
    [SerializeField] [Tooltip("视图边界框的显示颜色")] private Color viewBoundsColor = Color.green;

    public void setBgShot(ShotSetting _shot)
    {
        setShot(bgShot, _shot);
    }
    public void setConsortShot(ShotSetting _shot)
    {
        setShot(consortShot, _shot);
    }

    public void setBgPos(float _scale, float _x, float _y)
    {
        bgPos.normalizeScale = _scale;
        bgPos.normalizePosX = _x;
        bgPos.normalizePosY = _y;
    }
    public void setConsortPos(float _scale, float _x, float _y)
    {
        consortPos.normalizeScale = _scale;
        consortPos.normalizePosX = _x;
        consortPos.normalizePosY = _y;
    }

    private void setShot(ShotSetting from, ShotSetting to)
    {
        from.scaleRange = to.scaleRange;
        from.scaleRect = to.scaleRect;
        from.shotType = to.shotType;
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        _update();
    }

    public void _update()
    {
        if (imgBg != null)
        {
            Vector2 imgPos = Vector2.zero;
            float scale = 1;
            if (bgPos != null)
            {
                imgPos = bgShot.getShot(bgPos.normalizeScale, bgPos.normalizePosX, bgPos.normalizePosY, imgBg.rectTransform.sizeDelta, viewSize, out scale);
            }

            imgBg.rectTransform.anchoredPosition = imgPos;
            imgBg.rectTransform.localScale = new Vector3(scale, scale, scale);
        }

        if (imgActor != null)
        {
            Vector2 imgPos = Vector2.zero;
            float scale = 1;
            if (consortPos != null)
            {
                imgPos = consortShot.getShot(consortPos.normalizeScale, consortPos.normalizePosX, consortPos.normalizePosY, imgActor.rectTransform.sizeDelta, viewSize, out scale);
            }

            imgActor.rectTransform.anchoredPosition = imgPos;
            imgActor.rectTransform.localScale = new Vector3(scale, scale, scale);
        }

    }

    private void OnDrawGizmos()
    {
        if (!showVisualization) return;

        // 绘制视图边界
        DrawViewBounds();
        
        // 绘制背景图ScaleRect区域
        if (showBgViewArea && imgBg != null)
        {
            DrawImageBounds(imgBg, bgShot, bgPos, bgScaleRectColor, "BG");
        }
        
        // 绘制妃子图ScaleRect区域
        if (showConsortViewArea && imgActor != null)
        {
            DrawImageBounds(imgActor, consortShot, consortPos, consortScaleRectColor, "Consort");
        }
    }

    private void DrawViewBounds()
    {
        Gizmos.color = viewBoundsColor;
        
        // 获取UI元素的世界坐标
        Vector3 center = transform.TransformPoint(Vector3.zero);
        Vector3 size = transform.TransformVector(new Vector3(viewSize.x, viewSize.y, 0));
        
        // 绘制视图边界框
        Gizmos.DrawWireCube(center, size);
        
        // 在Scene视图中显示标签
        Handles.color = viewBoundsColor;
        Handles.Label(center + transform.TransformVector(Vector3.up * (viewSize.y / 2 + 20)), $"View Size: {viewSize}");
    }

    private void DrawImageBounds(RawImage img, ShotSetting shot, ConsortShotPos pos, Color color, string label)
    {
        if (img == null || shot == null || pos == null) return;

        Vector2 imgSize = img.rectTransform.sizeDelta;
        
        // 计算当前shot的信息
        float scale;
        Vector2 shotPos = shot.getShot(pos.normalizeScale, pos.normalizePosX, pos.normalizePosY, imgSize, viewSize, out scale);
        
        // 计算图片在UI坐标系中的位置
        Vector3 localImagePos = new Vector3(shotPos.x, shotPos.y, 0);
        Vector3 imgWorldPos = transform.TransformPoint(localImagePos);
        
        // 绘制整个图片区域（实线）
        DrawImageArea(imgWorldPos, imgSize * scale, color);
        
        // 绘制scaleRect可视区域（虚线）
        DrawScaleRectArea(shot.scaleRect, imgWorldPos, imgSize * scale, color, "Visible");
        
        // 显示信息标签
        string info = $"{label} 区域\nScale: {scale:F2}\nPos: ({shotPos.x:F1}, {shotPos.y:F1})\nScaleRect: {shot.scaleRect}";
        Vector3 labelOffset = transform.TransformVector(Vector3.up * (viewSize.y / 2 + 60));
        Handles.Label(imgWorldPos + labelOffset, info);
    }

    private void DrawImageArea(Vector3 imgWorldPos, Vector2 scaledImgSize, Color color)
    {
        // 绘制整个图片区域（实线边框）
        Vector3 worldImgSize = transform.TransformVector(new Vector3(scaledImgSize.x, scaledImgSize.y, 0));
        
        Gizmos.color = color;
        Gizmos.DrawWireCube(imgWorldPos, worldImgSize);
        
        // 用半透明填充显示图片范围
        Color fillColor = color;
        fillColor.a = 0.1f;
        Gizmos.color = fillColor;
        Gizmos.DrawCube(imgWorldPos, worldImgSize);
        
        // 显示标签
        Handles.color = color;
        Handles.Label(imgWorldPos + transform.TransformVector(Vector3.up * (scaledImgSize.y / 2 + 20)), "Image Area");
    }

    private void DrawScaleRectArea(Rect scaleRect, Vector3 imgWorldPos, Vector2 scaledImgSize, Color color, string rectType)
    {
        // 计算scaleRect在图片上的实际尺寸
        Vector2 rectSize = new Vector2(scaleRect.width * scaledImgSize.x, scaleRect.height * scaledImgSize.y);
        
        // 计算scaleRect的中心位置（相对于图片中心）
        Vector2 rectCenter = new Vector2(
            (scaleRect.x + scaleRect.width * 0.5f - 0.5f) * scaledImgSize.x,
            (scaleRect.y + scaleRect.height * 0.5f - 0.5f) * scaledImgSize.y
        );
        
        // 转换为世界坐标
        Vector3 localRectCenter = new Vector3(rectCenter.x, rectCenter.y, 0);
        Vector3 worldRectCenter = imgWorldPos + transform.TransformVector(localRectCenter);
        Vector3 worldRectSize = transform.TransformVector(new Vector3(rectSize.x, rectSize.y, 0));
        
        // 绘制scaleRect区域虚线边框
        Color dashedColor = color;
        dashedColor.a = 0.8f;
        Gizmos.color = dashedColor;
        
        // 计算四个角点的世界坐标
        Vector3 halfSize = worldRectSize * 0.5f;
        Vector3[] corners = new Vector3[4];
        corners[0] = worldRectCenter + new Vector3(-halfSize.x, -halfSize.y, 0);
        corners[1] = worldRectCenter + new Vector3(halfSize.x, -halfSize.y, 0);
        corners[2] = worldRectCenter + new Vector3(halfSize.x, halfSize.y, 0);
        corners[3] = worldRectCenter + new Vector3(-halfSize.x, halfSize.y, 0);
        
        // 绘制虚线边界
        float dashSize = 15f * transform.lossyScale.x; // 根据UI缩放调整虚线大小
        for (int i = 0; i < 4; i++)
        {
            DrawDashedLine(corners[i], corners[(i + 1) % 4], dashSize);
        }
        
        // 用更透明的颜色填充可视区域
        Color fillColor = color;
        fillColor.a = 0.2f;
        Gizmos.color = fillColor;
        Gizmos.DrawCube(worldRectCenter, worldRectSize);
        
        // 在区域中心显示标签
        Handles.color = color;
        Handles.Label(worldRectCenter, $"{rectType} Area");
    }

    private void DrawDashedLine(Vector3 start, Vector3 end, float dashSize)
    {
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);
        
        for (float i = 0; i < distance; i += dashSize * 2)
        {
            Vector3 dashStart = start + direction * i;
            Vector3 dashEnd = start + direction * Mathf.Min(i + dashSize, distance);
            Gizmos.DrawLine(dashStart, dashEnd);
        }
    }
#endif
}