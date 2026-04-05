
/// <summary>
/// Extending Unitys Debug class by a few shapes.
/// https://raw.githubusercontent.com/fkate/Unity_Misc/master/DebugShapes/DebugPlus.cs
/// </summary>

using UnityEngine;

namespace GOE
{

	public static class DebugPlus {
	    /// <summary>
	    /// Draws a circle.
	    /// </summary>
	    /// <param name="center">Center of the circle.</param>
	    /// <param name="radius">Radius of the circle.</param>
	    /// <param name="normal">Normal pointing up.</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawCircle(Vector3 center, float radius, Vector3 normal, Color color, float duration) {
	        if(normal == Vector3.zero)
	        {
	            normal = Vector3.up;
	        }
	        Quaternion rotation = Quaternion.LookRotation(normal.normalized);

	        Vector3 x = rotation * new Vector3(radius, 0, 0);
	        Vector3 y = rotation * new Vector3(0, radius, 0);

	        Vector3[] points = new Vector3[] {
	            center + x,
	            center + (x + y).normalized * radius,
	            center + y,
	            center + (y - x).normalized * radius,
	            center - x,
	            center - (x + y).normalized * radius,
	            center - y,
	            center - (y - x).normalized * radius
	        };

	        DrawPath(points, true, color, duration);
	    }

	    /// <summary>
	    /// Draws a circle.
	    /// </summary>
	    /// <param name="center">Center of the circle.</param>
	    /// <param name="radius">Radius of the circle.</param>
	    /// <param name="normal">Normal pointing up.</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawCircle(Vector3 center, float radius, Vector3 normal, Color color) {
	        DrawCircle(center, radius, normal, color, 0);
	    }

	    /// <summary>
	    /// Draws a circle.
	    /// </summary>
	    /// <param name="center">Center of the circle.</param>
	    /// <param name="radius">Radius of the circle.</param>
	    /// <param name="normal">Normal pointing up.</param>
	    public static void DrawCircle(Vector3 center, float radius, Vector3 normal) {
	        DrawCircle(center, radius, normal, Color.white, 0);
	    }
    
	    /// <summary>
	    /// Draws a sphere.
	    /// </summary>
	    /// <param name="center">Center of the sphere.</param>
	    /// <param name="radius">Radius of the sphere.</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawSphere(Vector3 center, float radius, Color color, float duration) {
	        DrawCircle(center, radius, Vector3.up, color, duration);
	        DrawCircle(center, radius, Vector3.right, color, duration);
	        DrawCircle(center, radius, Vector3.forward, color, duration);
	    }

	    /// <summary>
	    /// Draws a sphere.
	    /// </summary>
	    /// <param name="center">Center of the sphere.</param>
	    /// <param name="radius">Radius of the sphere.</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawSphere(Vector3 center, float radius, Color color) {
	        DrawSphere(center, radius, color, 0);
	    }

	    /// <summary>
	    /// Draws a sphere.
	    /// </summary>
	    /// <param name="center">Center of the sphere.</param>
	    /// <param name="radius">Radius of the sphere.</param>
	    public static void DrawSphere(Vector3 center, float radius) {
	        DrawSphere(center, radius, Color.white, 0);
	    }


	    /// <summary>
	    /// Draws a path from multiple input points.
	    /// </summary>
	    /// <param name="points">Points to draw.</param>
	    /// <param name="closedLoop">Should the last and first point be connected?</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawPath(Vector3[] points, bool closedLoop, Color color, float duration) {
	        if (points.Length < 2) return;

	        // Draw the path
	        for (int i = 0; i < points.Length; i++) {
	            if (i + 1 < points.Length) Debug.DrawLine(points[i], points[i + 1], color, duration);
	        }

	        // Connect to the beginning
	        if (closedLoop) Debug.DrawLine(points[points.Length - 1], points[0], color, duration);
	    }

	    /// <summary>
	    /// Draws a path from multiple input points.
	    /// </summary>
	    /// <param name="points">Points to draw.</param>
	    /// <param name="closedLoop">Should the last and first point be connected?</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawPath(Vector3[] points, bool closedLoop, Color color) {
	        DrawPath(points, closedLoop, color, 0);
	    }

	    /// <summary>
	    /// Draws a path from multiple input points.
	    /// </summary>
	    /// <param name="points">Points to draw.</param>
	    /// <param name="closedLoop">Should the last and first point be connected?</param>
	    public static void DrawPath(Vector3[] points, bool closedLoop) {
	        DrawPath(points, closedLoop, Color.white, 0);
	    }


	    /// <summary>
	    /// Draws a box.
	    /// </summary>
	    /// <param name="center">Center of the box.</param>
	    /// <param name="size">Size of the box.</param>
	    /// <param name="rotation">Rotation of the box.(Base rotation faces forward).</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawBox(Vector3 center, Vector3 size, Quaternion rotation, Color color, float duration) {
	        Vector3 hSize = size * 0.5f;

	        // Calculate max and min point
	        Vector3 min = center + rotation * -hSize;
	        Vector3 max = center + rotation * hSize;

	        Vector3 x = rotation * new Vector3(size.x, 0, 0);
	        Vector3 y = rotation * new Vector3(0, size.y, 0);
	        Vector3 z = rotation * new Vector3(0, 0, size.z);

	        // Bottom
	        Debug.DrawLine(min, min + x, color, duration);
	        Debug.DrawLine(min + x, min + x + z, color, duration);
	        Debug.DrawLine(min + x + z, min + z, color, duration);
	        Debug.DrawLine(min + z, min, color, duration);

	        // Top
	        Debug.DrawLine(max, max - x, color, duration);
	        Debug.DrawLine(max - x, max - x - z, color, duration);
	        Debug.DrawLine(max - x - z, max - z, color, duration);
	        Debug.DrawLine(max - z, max, color, duration);

	        // Side
	        Debug.DrawLine(min, min + y, color, duration);
	        Debug.DrawLine(min + x, min + x + y, color, duration);
	        Debug.DrawLine(min + x + z, min + x + z + y, color, duration);
	        Debug.DrawLine(min + z, min + z + y, color, duration);
	    }

	    /// <summary>
	    /// Draws a box.
	    /// </summary>
	    /// <param name="center">Center of the box.</param>
	    /// <param name="size">Size of the box.</param>
	    /// <param name="rotation">Rotation of the box.(Base rotation faces forward).</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawBox(Vector3 center, Vector3 size, Quaternion rotation, Color color) {
	        DrawBox(center, size, rotation, color, 0);
	    }

	    /// <summary>
	    /// Draws a box.
	    /// </summary>
	    /// <param name="center">Center of the box.</param>
	    /// <param name="size">Size of the box.</param>
	    /// <param name="rotation">Rotation of the box.(Base rotation faces forward).</param>
	    public static void DrawBox(Vector3 center, Vector3 size, Quaternion rotation) {
	        DrawBox(center, size, rotation, Color.white, 0);
	    }


	    /// <summary>
	    /// Draws a cross.
	    /// </summary>
	    /// <param name="center">Center of the cross.</param>
	    /// <param name="size">Size of the cross.</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawCross(Vector3 center, Vector3 size, Color color, float duration) {
	        Vector3 x = new Vector3(size.x, 0, 0);
	        Vector3 y = new Vector3(0, size.y, 0);
	        Vector3 z = new Vector3(0, 0, size.z);

	        Debug.DrawLine(center - x * 0.5f, center + x * 0.5f, color, duration);
	        Debug.DrawLine(center - y * 0.5f, center + y * 0.5f, color, duration);
	        Debug.DrawLine(center - z * 0.5f, center + z * 0.5f, color, duration);
	    }

	    /// <summary>
	    /// Draws a cross.
	    /// </summary>
	    /// <param name="center">Center of the cross.</param>
	    /// <param name="size">Size of the cross.</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawCross(Vector3 center, Vector3 size, Color color) {
	        DrawCross(center, size, color, 0);
	    }

	    /// <summary>
	    /// Draws a cross.
	    /// </summary>
	    /// <param name="center">Center of the cross.</param>
	    /// <param name="size">Size of the cross.</param>
	    public static void DrawCross(Vector3 center, Vector3 size) {
	        DrawCross(center, size, Color.white, 0);
	    }


	    /// <summary>
	    /// Draws a cone along a direction.
	    /// </summary>
	    /// <param name="origin">Origin of the cone.</param>
	    /// <param name="direction">Direction and length of the cone.</param>
	    /// <param name="radius">Radius at the cones end position.</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawCone(Vector3 origin, Vector3 direction, float radius, Color color, float duration) {
	        if(direction == Vector3.zero)
	        {
	            direction = Vector3.up;
	        }
	        Quaternion rotation = Quaternion.LookRotation(direction);
	        Vector3 target = origin + direction;

	        // Draw radius
	        DrawCircle(target, radius, direction, color, duration);

	        // Normal axis
	        Debug.DrawRay(origin, direction + rotation * Vector3.up * radius, color, duration);
	        Debug.DrawRay(origin, direction + rotation * -Vector3.up * radius, color, duration);
	        Debug.DrawRay(origin, direction + rotation * Vector3.right * radius, color, duration);
	        Debug.DrawRay(origin, direction + rotation * -Vector3.right * radius, color, duration);

	        // Diagonal axis
	        Debug.DrawRay(origin, direction + rotation * new Vector3(1, 1, 0).normalized * radius, color, duration);
	        Debug.DrawRay(origin, direction + rotation * new Vector3(1, -1, 0).normalized * radius, color, duration);
	        Debug.DrawRay(origin, direction + rotation * new Vector3(-1, 1, 0).normalized * radius, color, duration);
	        Debug.DrawRay(origin, direction + rotation * new Vector3(-1, -1, 0).normalized * radius, color, duration);
	    }

	    /// <summary>
	    /// Draws a cone along a direction.
	    /// </summary>
	    /// <param name="origin">Origin of the cone.</param>
	    /// <param name="direction">Direction and length of the cone.</param>
	    /// <param name="radius">Radius at the cones end position.</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawCone(Vector3 origin, Vector3 direction, float radius, Color color) {
	        DrawCone(origin, direction, radius, color, 0);
	    }

	    /// <summary>
	    /// Draws a cone along a direction.
	    /// </summary>
	    /// <param name="origin">Origin of the cone.</param>
	    /// <param name="direction">Direction and length of the cone.</param>
	    /// <param name="radius">Radius at the cones end position.</param>
	    public static void DrawCone(Vector3 origin, Vector3 direction, float radius) {
	        DrawCone(origin, direction, radius, Color.white, 0);
	    }


	    /// <summary>
	    /// Draws a cylinder along a direction.
	    /// </summary>
	    /// <param name="origin">Origin of the cylinder.</param>
	    /// <param name="direction">Direction of the cylinder.</param>
	    /// <param name="radius">Radius of the cylinder.</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawCylinder(Vector3 origin, Vector3 direction, float radius, Color color, float duration) {
	        if(direction == Vector3.zero)
	        {
	            direction = Vector3.up;
	        }
	        Quaternion rotation = Quaternion.LookRotation(direction);
	        Vector3 target = origin + direction;

	        // Draw radius
	        DrawCircle(origin, radius, direction, color, duration);
	        DrawCircle(target, radius, direction, color, duration);

	        Vector3 localUp = rotation * Vector3.up * radius;
	        Vector3 localRight = rotation * Vector3.right * radius;

	        // Normal axis
	        Debug.DrawRay(origin + localUp, direction, color, duration);
	        Debug.DrawRay(origin - localUp, direction, color, duration);
	        Debug.DrawRay(origin + localRight, direction, color, duration);
	        Debug.DrawRay(origin - localRight, direction, color, duration);
	    }

	    /// <summary>
	    /// Draws a cylinder along a direction.
	    /// </summary>
	    /// <param name="origin">Origin of the cylinder.</param>
	    /// <param name="direction">Direction of the cylinder.</param>
	    /// <param name="radius">Radius of the cylinder.</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawCylinder(Vector3 origin, Vector3 direction, float radius, Color color) {
	        DrawCylinder(origin, direction, radius, color, 0);
	    }

	    /// <summary>
	    /// Draws a cylinder along a direction.
	    /// </summary>
	    /// <param name="origin">Origin of the cylinder.</param>
	    /// <param name="direction">Direction of the cylinder.</param>
	    /// <param name="radius">Radius of the cylinder.</param>
	    public static void DrawCylinder(Vector3 origin, Vector3 direction, float radius) {
	        DrawCylinder(origin, direction, radius, Color.white, 0);
	    }

	    public static void DrawArrow2(Vector3 origin, Vector3 end, Color color, float duration)
	    {
	        DrawArrow(origin, end - origin, color, duration);
	    }
    
	    public static void DrawArrow2(Vector3 origin, Vector3 end, Color color)
	    {
	        DrawArrow(origin, end - origin, color);
	    }
    
	    public static void DrawArrow2(Vector3 origin, Vector3 end)
	    {
	        DrawArrow(origin, end - origin);
	    }

	    /// <summary>
	    /// Draws an arrow.
	    /// </summary>
	    /// <param name="origin">Origin of the arrow.</param>
	    /// <param name="direction">Direction and length of the arrow.</param>
	    /// <param name="color">Color of the lines.</param>
	    /// <param name="duration">How long the lines should be visible for.</param>
	    public static void DrawArrow(Vector3 origin, Vector3 direction, Color color, float duration) {
	        if(direction == Vector3.zero)
	        {
	            direction = Vector3.up;
	        }
	        float dist = Mathf.Min(direction.magnitude * 0.005f, 0.2f);
	        Vector3 arrowRing = origin + direction * 0.95f;
	        Vector3 arrowHead = origin + direction;

	        Quaternion rotation = Quaternion.LookRotation(direction);

	        // Draw line
	        Debug.DrawLine(origin, arrowHead, color, duration);

	        // Draw arrow ring
	        Vector3 x = rotation * new Vector3(dist, 0, 0);
	        Vector3 y = rotation * new Vector3(0, dist, 0);

	        Vector3[] points = new Vector3[] {
	            arrowRing + (x + y),
	            arrowRing + (y - x),
	            arrowRing - (x + y),
	            arrowRing - (y - x)
	        };

	        DrawPath(points, true, color, duration);

	        // Draw arrow  lines
	        Debug.DrawLine(arrowHead, points[0], color, duration);
	        Debug.DrawLine(arrowHead, points[1], color, duration);
	        Debug.DrawLine(arrowHead, points[2], color, duration);
	        Debug.DrawLine(arrowHead, points[3], color, duration);
	    }

	    /// <summary>
	    /// Draws an arrow.
	    /// </summary>
	    /// <param name="origin">Origin of the arrow.</param>
	    /// <param name="direction">Direction and length of the arrow.</param>
	    /// <param name="color">Color of the lines.</param>
	    public static void DrawArrow(Vector3 origin, Vector3 direction, Color color) {
	        DrawArrow(origin, direction, color, 0);
	    }

	    /// <summary>
	    /// Draws an arrow.
	    /// </summary>
	    /// <param name="origin">Origin of the arrow.</param>
	    /// <param name="direction">Direction and length of the arrow.</param>
	    public static void DrawArrow(Vector3 origin, Vector3 direction) {
	        DrawArrow(origin, direction, Color.white, 0);
	    }
    
	    public static void DrawRect(Rect _rect, float _y, Color _color, float _duration = 0f)
	    {
		    var lb = new Vector3(_rect.xMin, _y, _rect.yMin);
		    var lu = new Vector3(_rect.xMin, _y, _rect.yMax);
		    var rb = new Vector3(_rect.xMax, _y, _rect.yMin);
		    var ru = new Vector3(_rect.xMax, _y, _rect.yMax);

		    Debug.DrawLine(lb, rb, _color, _duration);
		    Debug.DrawLine(lb, lu, _color, _duration);
		    Debug.DrawLine(ru, rb, _color, _duration);
		    Debug.DrawLine(ru, lu, _color, _duration);
	    }

	    public static void DrawRectRotate(Rect _rect, Quaternion rot, float _y, Color _color)
	    {
	        var lb = new Vector3(_rect.xMin, _y, _rect.yMin);
	        var lu = new Vector3(_rect.xMin, _y, _rect.yMax);
	        var rb = new Vector3(_rect.xMax, _y, _rect.yMin);
	        var ru = new Vector3(_rect.xMax, _y, _rect.yMax);

	        var center = (lb + lu + rb + ru) / 4;
            
	        var l_lb  = lb - center;
	        var l_lu  = lu - center;
	        var l_rb  = rb - center;
	        var l_ru  = ru - center;

	        var nl_lb = rot * l_lb ;
	        var nl_lu = rot * l_lu ;
	        var nl_rb = rot * l_rb ;
	        var nl_ru = rot * l_ru ;
            
	        var nlb = nl_lb + center;
	        var nlu = nl_lu + center;
	        var nrb = nl_rb + center;
	        var nru = nl_ru + center;
                
	        Debug.DrawLine(nlb, nrb, _color);
	        Debug.DrawLine(nlb, nlu, _color);
	        Debug.DrawLine(nru, nrb, _color);
	        Debug.DrawLine(nru, nlu, _color);
	    }
    
	    public static void DrawRectRotate(Rect _rect, Vector2 center, Vector3 f, float _y, Color _color)
	    {
	        var lb = new Vector2(_rect.xMin, _rect.yMin);
	        var lu = new Vector2(_rect.xMin, _rect.yMax);
	        var rb = new Vector2(_rect.xMax, _rect.yMin);
	        var ru = new Vector2(_rect.xMax, _rect.yMax);
        
	        var l_lb  = lb - center;
	        var l_lu  = lu - center;
	        var l_rb  = rb - center;
	        var l_ru  = ru - center;

	        float num0 = Mathf.Sqrt(f.x * f.x + f.z * f.z);
	        float cos = f.z / num0;
	        float sin = -f.x / num0;
        
	        var nl_lb = new Vector2(l_lb.x * cos - l_lb.y * sin, l_lb.x * sin + l_lb.y * cos);
	        var nl_lu = new Vector2(l_lu.x * cos - l_lu.y * sin, l_lu.x * sin + l_lu.y * cos);
	        var nl_rb = new Vector2(l_rb.x * cos - l_rb.y * sin, l_rb.x * sin + l_rb.y * cos);
	        var nl_ru = new Vector2(l_ru.x * cos - l_ru.y * sin, l_ru.x * sin + l_ru.y * cos);
            
	        var nlb = nl_lb + center;
	        var nlu = nl_lu + center;
	        var nrb = nl_rb + center;
	        var nru = nl_ru + center;
                
	        Debug.DrawLine(new Vector3(nlb.x, _y, nlb.y) , new Vector3(nrb.x, _y, nrb.y), _color);
	        Debug.DrawLine(new Vector3(nlb.x, _y, nlb.y) , new Vector3(nlu.x, _y, nlu.y), _color);
	        Debug.DrawLine(new Vector3(nru.x, _y, nru.y) , new Vector3(nrb.x, _y, nrb.y), _color);
	        Debug.DrawLine(new Vector3(nru.x, _y, nru.y) , new Vector3(nlu.x, _y, nlu.y), _color);
	    }
    
	    // 绘制四边形，点按固定方向排序
	    public static void DrawQuad(Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3, Color _color)
	    {
	        Debug.DrawLine(_p0, _p1, _color);
	        Debug.DrawLine(_p1, _p2, _color);
	        Debug.DrawLine(_p2, _p3, _color);
	        Debug.DrawLine(_p3, _p0, _color);
	    }

	    // 绘制六面体
	    public static void DrawView(Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3, Vector3 _fp0, Vector3 _fp1, Vector3 _fp2, Vector3 _fp3, Color _color)
	    {
	        DrawQuad(_p0, _p1, _p2, _p3, _color);
	        DrawQuad(_fp0, _fp1, _fp2, _fp3, _color);
	        Debug.DrawLine(_p0, _fp0, _color);
	        Debug.DrawLine(_p1, _fp1, _color);
	        Debug.DrawLine(_p2, _fp2, _color);
	        Debug.DrawLine(_p3, _fp3, _color);
	    }
	    /// <summary>
	    /// 获得一个椭圆的点
	    /// </summary>
	    public static void MakeEllipsePoint(Vector3 _center, float _a, float _b, Vector3[] _points)
	    {
	        if (_points == null || _points.Length <= 0)
	            return;
        
	        for (int i = 0; i < _points.Length; i++)
	        {
	            float angle = i / (float) _points.Length * 2f * Mathf.PI;
	            _points[i] = new Vector3(_a * Mathf.Cos(angle), _b * Mathf.Sin(angle), 0);
	            _points[i] = _points[i] + _center;
	        }
	    }
	}
}