using UnityEngine;

namespace GOE
{
    public class BezierUtils
    {
        /// <summary>
        /// 根据T值，计算三阶贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="t">T值</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">起始点的控制点</param>
        /// <param name="p2">目标点的控制点</param>
        /// <param name="p3">目标点</param>
        /// <returns></returns>根据T值计算出来的贝赛尔曲线点
        public static Vector2 CalculateCubicBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float ttt = t * t * t;
            float uuu = u * u * u;

            Vector2 p = uuu * p0
                        + 3 * t * uu * p1
                        + 3 * tt * u * p2
                        + ttt * p3;

            return p;
        }

        /// <summary>
        /// 根据T值，计算贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="t"></param>T值
        /// <param name="p0"></param>起始点
        /// <param name="p1"></param>控制点
        /// <param name="p2"></param>目标点
        /// <returns></returns>根据T值计算出来的贝赛尔曲线点
        public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float ttt = t * t * t;
            float uuu = u * u * u;

            Vector3 p = uuu * p0
                        + 3 * t * uu * p1
                        + 3 * tt * u * p2
                        + ttt * p3;

            return p;
        }

        /// <summary>
        /// 根据T值，计算贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="t"></param>T值
        /// <param name="p0"></param>起始点
        /// <param name="p1"></param>控制点
        /// <param name="p2"></param>目标点
        /// <returns></returns>根据T值计算出来的贝赛尔曲线点
        public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }

        /// <summary>
        /// 根据T值，计算贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="t">T值</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">控制点</param>
        /// <param name="p2">目标点</param>
        /// <returns></returns>
        public static Vector2 CalculateCubicBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector2 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }

        /// <summary>
        /// 根据T值，计算三阶贝塞尔曲线上对应点的切线向量（未归一化）
        /// </summary>
        /// <param name="t">T值</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">起始点的控制点</param>
        /// <param name="p2">目标点的控制点</param>
        /// <param name="p3">目标点</param>
        /// <returns>切线向量（未归一化）</returns>
        public static Vector3 CalculateCubicBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float uu = u * u;
            float tt = t * t;

            // B'(t) = 3(1-t)²(p1-p0) + 6(1-t)t(p2-p1) + 3t²(p3-p2)
            Vector3 tangent = 3 * uu * (p1 - p0)
                            + 6 * u * t * (p2 - p1)
                            + 3 * tt * (p3 - p2);

            return tangent;
        }

        /// <summary>
        /// 根据T值，计算三阶贝塞尔曲线上对应点的切线向量（未归一化）
        /// </summary>
        /// <param name="t">T值</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">起始点的控制点</param>
        /// <param name="p2">目标点的控制点</param>
        /// <param name="p3">目标点</param>
        /// <returns>切线向量（未归一化）</returns>
        public static Vector2 CalculateCubicBezierTangent(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float u = 1 - t;
            float uu = u * u;
            float tt = t * t;

            // B'(t) = 3(1-t)²(p1-p0) + 6(1-t)t(p2-p1) + 3t²(p3-p2)
            Vector2 tangent = 3 * uu * (p1 - p0)
                            + 6 * u * t * (p2 - p1)
                            + 3 * tt * (p3 - p2);

            return tangent;
        }

        /// <summary>
        /// 根据T值，计算二阶贝塞尔曲线上对应点的切线向量（未归一化）
        /// </summary>
        /// <param name="t">T值</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">控制点</param>
        /// <param name="p2">目标点</param>
        /// <returns>切线向量（未归一化）</returns>
        public static Vector3 CalculateCubicBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;

            // B'(t) = 2(1-t)(p1-p0) + 2t(p2-p1)
            Vector3 tangent = 2 * u * (p1 - p0)
                            + 2 * t * (p2 - p1);

            return tangent;
        }

        /// <summary>
        /// 根据T值，计算二阶贝塞尔曲线上对应点的切线向量（未归一化）
        /// </summary>
        /// <param name="t">T值</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">控制点</param>
        /// <param name="p2">目标点</param>
        /// <returns>切线向量（未归一化）</returns>
        public static Vector2 CalculateCubicBezierTangent(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float u = 1 - t;

            // B'(t) = 2(1-t)(p1-p0) + 2t(p2-p1)
            Vector2 tangent = 2 * u * (p1 - p0)
                            + 2 * t * (p2 - p1);

            return tangent;
        }

        /// <summary>
        /// 获取存储贝塞尔曲线点的数组
        /// </summary>
        /// <param name="startPoint"></param>起始点
        /// <param name="controlPoint"></param>控制点
        /// <param name="endPoint"></param>目标点
        /// <param name="segmentNum"></param>采样点的数量
        /// <returns></returns>存储贝塞尔曲线点的数组
        public static Vector3[] GetBeizerList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int segmentNum) {
            Vector3[] path = new Vector3[segmentNum];
            for (int i = 1; i <= segmentNum; i++) {
                float t = i / (float) segmentNum;
                Vector3 pixel = CalculateCubicBezierPoint(t, startPoint,
                    controlPoint, endPoint);
                path[i - 1] = pixel;
                Debug.Log(path[i - 1]);
            }
            return path;
        }
    }
}