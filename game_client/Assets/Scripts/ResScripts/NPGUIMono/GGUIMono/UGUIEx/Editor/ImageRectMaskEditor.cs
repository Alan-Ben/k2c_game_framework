using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ImageRectMask))]
public class ImageRectMaskEditor : Editor
{
    private static Color kShadowColor = new Color(0, 0, 0, 0.5f);
    private static Vector2 kShadowOffset = new Vector2(1, -1);
    private const float kDottedLineSize = 6.0f;
    private static Color kRectInParentSpaceColor = new Color(1.0f, 0, 0, 1f);
    public static void DrawDottedLineWithShadow(Color shadowColor, Vector2 screenOffset, Vector3 p1, Vector3 p2, float screenSpaceSize)
    {
        Camera cam = Camera.current;
        if (!cam || Event.current.type != EventType.Repaint)
            return;

        Color oldColor = Handles.color;

        // shadow
        shadowColor.a = shadowColor.a * oldColor.a;
        Handles.color = shadowColor;
        Handles.DrawDottedLine(
            cam.ScreenToWorldPoint(cam.WorldToScreenPoint(p1) + (Vector3)screenOffset),
            cam.ScreenToWorldPoint(cam.WorldToScreenPoint(p2) + (Vector3)screenOffset), screenSpaceSize);

        // line itself
        Handles.color = oldColor;
        Handles.DrawDottedLine(p1, p2, screenSpaceSize);
    }
    
    void DrawRect(Rect rect, Transform space, bool dotted)
    {
        Vector3 p0 = space.TransformPoint(new Vector2(rect.x, rect.y));
        Vector3 p1 = space.TransformPoint(new Vector2(rect.x, rect.yMax));
        Vector3 p2 = space.TransformPoint(new Vector2(rect.xMax, rect.yMax));
        Vector3 p3 = space.TransformPoint(new Vector2(rect.xMax, rect.y));
        Handles.color = kRectInParentSpaceColor;
        if (!dotted)
        {
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);
            Handles.DrawLine(p3, p0);
        }
        else
        {
           DrawDottedLineWithShadow(kShadowColor, kShadowOffset, p0, p1, kDottedLineSize);
           DrawDottedLineWithShadow(kShadowColor, kShadowOffset, p1, p2, kDottedLineSize);
           DrawDottedLineWithShadow(kShadowColor, kShadowOffset, p2, p3, kDottedLineSize);
           DrawDottedLineWithShadow(kShadowColor, kShadowOffset, p3, p0, kDottedLineSize);
        }
    }
    private void OnSceneGUI()
    {
        ImageRectMask mask = target as ImageRectMask;
        if(mask == null)
        {
            return;
        }
        RectTransform ownSpace = mask.GetComponent<RectTransform>();
        if(mask.m_byRectTransform)
        {
            var rect = ownSpace.rect;
            float minX = rect.xMin +  mask.m_rangeLeft * rect.width;
            float maxX = rect.xMax - (1 -  mask.m_rangeRight) * rect.width;
            float minY = rect.yMin + (1-  mask.m_rangeBottom) * rect.height;
            float maxY = rect.yMax - ( mask.m_rangeUp) * rect.height;

            
            rect = new Rect(minX,minY,maxX - minX,maxY - minY);
            DrawRect(rect,ownSpace,true);
        }
        else
        {
            DrawRect(mask._m_curRect, ownSpace, true);

        }
    }
}

