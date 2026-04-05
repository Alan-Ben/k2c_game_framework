using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    
    public class GuildDungeonMapLinePoint
    {
        public GuildDungeonMonster monster;
        public Vector3 _m_Pos;
        public GuildDungeonMapLinePoint(Vector3 _pos, GuildDungeonMonster _monster)
        {
            monster = _monster;
            _m_Pos = _pos;
        }

        public Vector3 getWorldPosition()
        {
            return _m_Pos;
            
        }
    }
    /// <summary>
    /// 单条连线组件 - 使用贝塞尔曲线
    /// </summary>
    public class GuildDungeonMapLine
    {
        private GameObject _m_lineGO;
        private LineRenderer _m_lineRenderer;
        private Vector3 _m_fromPos = Vector3.zero;
        private Vector3 _m_toPos = Vector3.one;
        private bool _m_flip;
        private bool _m_revertPointOrder;
        
        private Vector2 _m_bezierStart = new Vector2(0,1f);
        private Vector2 _m_bezierEnd = new Vector2(0,1f);
        
        // 贝塞尔曲线参数
        private int _m_curvePoints = 20; // 曲线点数
        private float _m_lineWidth = 0.1f; // 线条宽度
        private ELineStyle _m_lineStyle = ELineStyle.Bezier; // 连线风格
        
        // 圆角正交参数
        private float _m_cornerRadius = 50.0f; // 圆角半径
        
        public bool flip { get { return _m_flip; } }

        public GuildDungeonMapLine(GameObject lineGO)
        {
            _m_lineGO = lineGO;
        }

        /// <summary>
        /// 设置连线数据
        /// </summary>
        /// <param name="_fromPos"></param>
        /// <param name="_toPos"></param>
        /// <param name="_flip"> toTotalCount > fromTotalCount </param>
        public void setLineData(Vector3 _fromPos, Vector3 _toPos, bool _flip)
        {
            _m_fromPos = _fromPos;
            _m_toPos = _toPos;
            _m_flip = _flip;
        }

        /// <summary>
        /// 设置曲线参数
        /// </summary>
        public void setCurveParams(int curvePoints, float lineWidth, ELineStyle lineStyle, bool _revertPointOrder, Vector2 _bezierStart, Vector2 _bezierEnd, float cornerRadius = 50.0f)
        {
            _m_curvePoints = curvePoints;
            _m_lineWidth = lineWidth;
            _m_lineStyle = lineStyle;
            _m_revertPointOrder = _revertPointOrder;
            _m_bezierStart = _bezierStart;
            _m_bezierEnd = _bezierEnd;
            _m_cornerRadius = cornerRadius;
            
            // 如果LineRenderer已经初始化，立即更新其宽度属性
            if (_m_lineRenderer != null)
            {
                _m_lineRenderer.startWidth = _m_lineWidth;
                _m_lineRenderer.endWidth = _m_lineWidth;
            }
        }

        /// <summary>
        /// 设置连接线材质（用于解锁/未解锁状态）
        /// </summary>
        /// <param name="material">要设置的材质</param>
        public void setLineMaterial(Material material)
        {
            if (_m_lineRenderer != null && material != null)
            {
                _m_lineRenderer.material = material;
            }
        }
        

        public void init()
        {
            if (_m_lineGO == null)
                return;

            _m_lineRenderer = _m_lineGO.GetComponent<LineRenderer>();
            if (_m_lineRenderer == null)
            {
                _m_lineRenderer = _m_lineGO.AddComponent<LineRenderer>();
                setupLineRenderer();
            }
            if (_m_lineRenderer != null)
            {
                _m_lineRenderer.startWidth = _m_lineWidth;
                _m_lineRenderer.endWidth = _m_lineWidth;
            }
            updateLine();
        }

        private void setupLineRenderer()
        {
            if (_m_lineRenderer == null)
                return;

            _m_lineRenderer.material = getLineMaterial();
            _m_lineRenderer.startWidth = _m_lineWidth;
            _m_lineRenderer.endWidth = _m_lineWidth;
            _m_lineRenderer.positionCount = _m_curvePoints; // 设置为曲线点数
            _m_lineRenderer.useWorldSpace = true;
            _m_lineRenderer.sortingLayerName = "Default";
            _m_lineRenderer.sortingOrder = -1; // 确保线条在怪物下方
        }

        private Material getLineMaterial()
        {
            // 可以在这里设置特定的材质，或者使用默认材质
            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = Color.white;
            return mat;
        }

        public void updateLine()
        {
            if (_m_lineRenderer == null)
                return;


            // 根据风格生成不同的连线
            Vector3[] curvePoints = null;
            switch (_m_lineStyle)
            {
                case ELineStyle.Orthogonal:
                    curvePoints = generateOrthogonalLine(_m_fromPos, _m_toPos);
                    break;
                case ELineStyle.RoundedOrthogonal:
                    curvePoints = generateRoundedOrthogonalLine(_m_fromPos, _m_toPos, _m_curvePoints);
                    break;
                case ELineStyle.Bezier:
                    curvePoints = generateBezierCurve(_m_fromPos, _m_toPos, _m_bezierStart, _m_bezierEnd,  _m_curvePoints);
                    break;
                case ELineStyle.Line:
                default:
                    curvePoints = generateLine(_m_fromPos, _m_toPos);
                    break;
            }
            
            if (curvePoints != null)
            {
                _m_lineRenderer.positionCount = curvePoints.Length;
                _m_lineRenderer.SetPositions(curvePoints);
            }
        }

        /// <summary>
        /// 计算三次贝塞尔曲线上的点
        /// </summary>
        private Vector3 calculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 point = uuu * p0;
            point += 3 * uu * t * p1;
            point += 3 * u * tt * p2;
            point += ttt * p3;

            return point;
        }

        /// <summary>
        /// 生成正交连接（方案C：中点转折-垂直优先）
        /// 走势：垂直走一半 → 水平走全程 → 垂直走一半
        /// </summary>
        private Vector3[] generateOrthogonalLine(Vector3 start, Vector3 end)
        {
            // 计算Y轴中点
            float midY = (start.y + end.y) / 2;
            
            // 三个转折点：
            // 1. 从起点垂直向上/下走到Y中点
            Vector3 point1 = new Vector3(start.x, midY, start.z);
            
            // 2. 水平走到终点的X坐标，保持在Y中点
            Vector3 point2 = new Vector3(end.x, midY, start.z);
            
            // 3. 垂直走到终点
            // end 已经是最后一个点
            
            return new Vector3[] { start, point1, point2, end };
        }

        /// <summary>
        /// 生成圆角正交连接（方案C：中点转折-垂直优先）
        /// 走势：垂直走一半 → 水平走全程 → 垂直走一半（带圆角过渡）
        /// 目的：在转折的直角处用圆弧平滑过渡
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="pointCount">总点数（用于控制圆角平滑度）</param>
        /// <returns>路径点数组</returns>
        private Vector3[] generateRoundedOrthogonalLine(Vector3 start, Vector3 end, int pointCount)
        {
            Vector3 deltaPos = end - start;
            
            // 使用专门的 cornerRadius 参数作为圆角半径
            // 但要限制最大值不超过路径段长度的一半
            float maxRadius = Mathf.Min(Mathf.Abs(deltaPos.x), Mathf.Abs(deltaPos.y)) * 0.5f;
            float cornerRadius = Mathf.Min(_m_cornerRadius, maxRadius);
            
            List<Vector3> points = new List<Vector3>();
            
            // 固定顶点：起点(1) + 终点(1) = 2个
            // 剩余点数均分给2个圆角
            // 例如：pointCount=20，固定2个，剩余18个，每个圆角9个点
            int fixedPoints = 2;  // 起点 + 终点
            int remainingPoints = pointCount - fixedPoints;
            int cornerCount = 2;  // 两个圆角
            int cornerPoints = Mathf.Max(3, remainingPoints / cornerCount);
            
            // 方案C：垂直优先
            float midY = (start.y + end.y) / 2;
            
            // === 路径的三个关键转折点（直角点） ===
            Vector3 corner1 = new Vector3(start.x, midY, start.z);      // 第一个直角转折点
            Vector3 corner2 = new Vector3(end.x, midY, start.z);        // 第二个直角转折点
            
            // 起点
            points.Add(start);
            
            // === 第一个圆角 ===
            // 在 corner1 处从垂直转向水平，需要圆角过渡
            AddSmoothCorner(points, start, corner1, corner2, cornerRadius, cornerPoints);
            
            // === 第二个圆角 ===
            // 在 corner2 处从水平转向垂直，需要圆角过渡
            AddSmoothCorner(points, corner1, corner2, end, cornerRadius, cornerPoints);
            
            // 终点
            points.Add(end);
            
            return points.ToArray();
        }
        
        /// <summary>
        /// 在转折点添加平滑圆角
        /// </summary>
        /// <param name="points">点列表</param>
        /// <param name="fromPoint">来的方向点</param>
        /// <param name="cornerPoint">转折的直角点</param>
        /// <param name="toPoint">去的方向点</param>
        /// <param name="radius">圆角半径</param>
        /// <param name="segments">圆角分段数</param>
        private void AddSmoothCorner(List<Vector3> points, Vector3 fromPoint, Vector3 cornerPoint, Vector3 toPoint, float radius, int segments)
        {
            // 计算来的方向和去的方向
            Vector3 dirFrom = (cornerPoint - fromPoint).normalized;
            Vector3 dirTo = (toPoint - cornerPoint).normalized;
            
            // 圆角的起点：从转折点沿来的方向反向走 radius 距离
            Vector3 arcStart = cornerPoint - dirFrom * radius;
            
            // 圆角的终点：从转折点沿去的方向走 radius 距离
            Vector3 arcEnd = cornerPoint + dirTo * radius;
            
            // 添加到达圆角起点的直线段
            points.Add(arcStart);
            
            // 圆心位置
            Vector3 center = arcStart + dirTo * radius;
            
            // 计算起始角度和结束角度
            Vector3 startDir = (arcStart - center).normalized;
            Vector3 endDir = (arcEnd - center).normalized;
            
            float startAngle = Mathf.Atan2(startDir.y, startDir.x);
            float endAngle = Mathf.Atan2(endDir.y, endDir.x);
            
            // 确保角度走最短路径（90度）
            float angleDiff = endAngle - startAngle;
            if (angleDiff > Mathf.PI) angleDiff -= 2 * Mathf.PI;
            if (angleDiff < -Mathf.PI) angleDiff += 2 * Mathf.PI;
            
            // 生成圆弧上的中间点（不包括起点，包括终点）
            // segments 是圆弧的分段数，会生成 segments 个点（arcStart 到 arcEnd 之间有 segments-1 个中间点，加上 arcEnd）
            for (int i = 1; i <= segments; i++)
            {
                float t = (float)i / segments;
                float angle = startAngle + angleDiff * t;
                
                Vector3 arcPoint = center + new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius,
                    center.z
                );
                
                points.Add(arcPoint);
            }
        }
     
        /// <summary>
        /// 生成平滑曲线（原始贝塞尔）
        /// </summary>
        private Vector3[] generateBezierCurve(Vector3 _start, Vector3 _end, Vector3 _controlStart, Vector3 _controlEnd, int pointCount)
        {
            Vector3[] points = new Vector3[pointCount];
      

            if (_m_revertPointOrder && !_m_flip)
            {
                Vector3 start = _end;
                Vector3 end = _start;
                      
                Vector3 dir = end - start;
            
                Vector3 control1 = start + Vector3.Scale(dir , _controlStart);
                Vector3 control2 = start + Vector3.Scale(dir , _controlEnd);
                
                for (int i = 0; i < pointCount; i++)
                {
                    float t = (float)i / (pointCount - 1);
                    int index = pointCount - i - 1 ;
                    points[index] = calculateBezierPoint(t, start, control1, control2, end);
                }
            }
            else
            {
                Vector3 start = _start;
                Vector3 end = _end;
                      
                Vector3 dir = end - start;
            
                Vector3 control1 = start + Vector3.Scale(dir , _controlStart);
                Vector3 control2 = start + Vector3.Scale(dir , _controlEnd);
                
                for (int i = 0; i < pointCount; i++)
                {
                    float t = (float)i / (pointCount - 1);
                    int index = i;
                    points[index] = calculateBezierPoint(t, start, control1, control2, end);
                }
            }

            return points;
        }
        
        /// <summary>
        /// 生成正交连接（L形）
        /// </summary>
        private Vector3[] generateLine(Vector3 start, Vector3 end)
        {
            return new Vector3[] { start, end };
        }

        public void discard()
        {
            if (_m_lineGO != null)
            {
                Object.Destroy(_m_lineGO);
                _m_lineGO = null;
            }
            _m_lineRenderer = null;
        }
    }
}