using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏通用的一些方法
    /// </summary>
    public static class NPGameUtility
    {
        /// <summary>
        /// 一个极小值
        /// </summary>
        public const float Epsilon = 1E-4F;
        
        /// <summary>
        /// 把 Vector2 作为 XZ 平面上的点转成 Vector3，并指定一个 y 值 
        /// </summary>
        public static Vector3 toVector3(this Vector2 _v2, float _newY)
        {
            return new Vector3(_v2.x, _newY, _v2.y);
        }
        /// <summary>
        /// 把 Vector3 以 XZ 平面上的投影坐标，转换为 Vector2
        /// </summary>
        public static Vector2 toXZPlane(this Vector3 _v3)
        {
            return new Vector2(_v3.x, _v3.z);
        }
        /// <summary>
        /// 把 Vector3 转成 Vector4
        /// </summary>
        public static Vector4 toVector4(this Vector3 _v3, float _w)
        {
            return new Vector4(_v3.x, _v3.y, _v3.z, _w);
        }
        /// <summary>
        /// 齐次坐标转笛卡尔坐标系
        /// </summary>
        public static Vector3 homogeneousToCartesian(this Vector4 _v4)
        {
            return _v4.w == 0 ? new Vector3(_v4.x, _v4.y, _v4.z) : new Vector3(_v4.x / _v4.w, _v4.y / _v4.w, _v4.z / _v4.w);
        }
        /// <summary>
        /// 对比两个向量是否近似
        /// </summary>
        public static bool Approximately(Vector3 _v1, Vector3 _v2)
        {
            return
                Mathf.Approximately(_v1.x, _v2.x) &&
                Mathf.Approximately(_v1.y, _v2.y) &&
                Mathf.Approximately(_v1.z, _v2.z);
        }
        /// <summary>
        /// 对比两个向量是否近似
        /// </summary>
        /// <returns></returns>
        public static bool Approximately(Vector3 _v1, Vector3 _v2, float _range)
        {
            
            return
                Mathf.Abs(_v1.x - _v2.x) <= _range &&
                Mathf.Abs(_v1.y - _v2.y) <= _range &&
                Mathf.Abs(_v1.z - _v2.z) <= _range;
        }
        /// <summary>
        /// 对比两个向量是否近似
        /// </summary>
        public static bool Approximately(Vector2 _v1, Vector2 _v2)
        {
            return
                Mathf.Approximately(_v1.x, _v2.x) &&
                Mathf.Approximately(_v1.y, _v2.y);
        }
        /// <summary>
        /// 对比两个四元数是否近似
        /// </summary>
        public static bool Approximately(Quaternion _q1, Quaternion _q2)
        {
            // 先检查正数
            if (Mathf.Approximately(_q1.x, _q2.x) &&
                Mathf.Approximately(_q1.y, _q2.y) &&
                Mathf.Approximately(_q1.z, _q2.z) &&
                Mathf.Approximately(_q1.w, _q2.w))
                return true;

            return
                Mathf.Approximately(_q1.x, -_q2.x) &&
                Mathf.Approximately(_q1.y, -_q2.y) &&
                Mathf.Approximately(_q1.z, -_q2.z) &&
                Mathf.Approximately(_q1.w, -_q2.w);
        }
        /// <summary>
        /// 判断点是否在圆里
        /// </summary>
        public static bool isPointInCircle(Vector2 _center, float _radius, Vector2 _point)
        {
            return Vector2.SqrMagnitude(_point - _center) < _radius * _radius;
        }
        /// <summary>
        /// 判断点是否在椭圆内
        /// </summary>
        public static bool isPointInEllipse(Vector2 _center, float _a, float _b, Vector2 _point)
        {
            float xOffset = _point.x - _center.x;
            float yOffset = _point.y - _center.y;
            return (xOffset * xOffset) / (_a * _a) + (yOffset * yOffset) / (_b * _b) < 1;
        }

        /// <summary>
        /// 保证返回的方向的 up 和 normal 一致，forward 大致一直
        /// </summary>
        public static Quaternion LookRotation(Vector3 _forward, Vector3 _normal)
        {
            
            float num = Vector3.Angle(_forward, _normal);
            if (num == 0)
                return Quaternion.LookRotation(_normal);
            
            Vector3 actualForward = Vector3.SlerpUnclamped(_normal, _forward, 90 / num);
            return Quaternion.LookRotation(actualForward, _normal);
        }
        
        /// <summary>
        /// 欧拉角转 2D 的方向
        /// </summary>
        public static Vector2 eulerAngle2XZDirection(Vector3 _eulerAngle)
        {
            return (Quaternion.Euler(_eulerAngle) * Vector3.forward).toXZPlane().normalized;
        }

        /// <summary>
        /// 判断某个点是否在另一个点的球形范围内
        /// </summary>
        /// <param name="_originPos"></param>
        /// <param name="_targetPos"></param>
        /// <param name="_radius"></param>
        /// <param name="_closestPoint"></param>
        /// <returns></returns>
        public static bool inRange(Vector3 _originPos, Vector3 _targetPos, float _radius, out Vector3 _closestPoint)
        {
            float distance = Vector3.Distance(_originPos, _targetPos);
            if (distance <= _radius)
            {
                _closestPoint = _originPos;
                return true;
            }

            _closestPoint = Vector3.Lerp(_targetPos, _originPos, _radius / distance);
            return false;
        }

        /// <summary>
        /// 计算一条路径的长度
        /// </summary>
        public static float calculatePathDistance(List<Vector3> _path)
        {
            float distance = 0;
            if (_path != null)
            {
                for (int i = 0; i < _path.Count - 1; i++)
                {
                    distance += Vector3.Distance(_path[i], _path[i + 1]);
                }
            }

            return distance;
        }

        /// <summary>
        /// 射线和球求交
        /// </summary>
        public static bool rayToSphereCollision(Vector3 _source, Vector3 _rayDirection, Vector3 _sphereCenter, float _sphereRadius, out Vector3 _hitPoint, out float _t)
        {
            _rayDirection.Normalize();

            Vector3 offset = _sphereCenter - _source;
            float e = offset.magnitude;
            float a = Vector3.Dot(offset, _rayDirection);
            float sqrF = _sphereRadius * _sphereRadius - (e * e - a * a);
            if (sqrF < 0)
            {
                _hitPoint = Vector3.zero;
                _t = 0;
                return false;
            }

            float f = Mathf.Sqrt(sqrF);
            _t = a - f;
            _hitPoint = _source + _rayDirection * _t;
            return true;
        }
        /// <summary>
        /// 射线和平面求交
        /// </summary>
        public static bool rayToPanelCollision(Ray _ray, Vector3 _panelPoint, Vector3 _panelNormal, out Vector3 _hitPoint, out float _t)
        {
            return rayToPanelCollision(_ray.origin, _ray.direction, _panelPoint, _panelNormal, out _hitPoint, out _t);
        }
        /// <summary>
        /// 射线和平面求交
        /// </summary>
        public static bool rayToPanelCollision(Vector3 _source, Vector3 _rayDirection, Vector3 _panelPoint, Vector3 _panelNormal, out Vector3 _hitPoint, out float _t)
        {
            _panelNormal = _panelNormal.normalized;
            float d = Vector3.Dot(_panelNormal, _rayDirection);
            if (d == 0)
            {
                _t = 0;
                _hitPoint = Vector3.zero;
                return false;
            }
            _t = Vector3.Dot(_panelNormal, _panelPoint - _source) / d;
            _hitPoint = _source + _rayDirection * _t;
            return true;
        }
        /// <summary>
        /// 计算 2D 的叉乘
        /// </summary>
        /// <remarks>
        /// 返回的其实是 3D 向量的 z 值 
        /// </remarks>
        public static float crossProduct2D(Vector2 _a, Vector2 _b)
        {
            return _a.x * _b.y - _a.y * _b.x;
        }
        /// <summary>
        /// 保持相机的高度和方向不变，计算相机的新位置，使其能看到目标点
        /// </summary>
        /// <param name="_cameraPosition">相机目前的坐标</param>
        /// <param name="_cameraFocusPosition">相机目前看着的点</param>
        /// <param name="_targetPos">新的想要让相机看着的点</param>
        /// <param name="_t">目标点到相机新的位置的距离，如果目标点在相机视线背后，就是负的</param>
        /// <returns>相机新的位置，让相机保持高度和方向不变，能够看到目标点</returns>
        public static Vector3 getNewCameraPosKeepHeightAndDirectionToSeeTargetXZ(Vector3 _cameraPosition, Vector3 _cameraFocusPosition, Vector3 _targetPos, out float _t)
        {
            // 相机朝着的方向
            Vector3 cameraDirection = (_cameraFocusPosition - _cameraPosition).normalized;
            // 如果两个点是一样的，直接放回目标点的坐标即可
            if (cameraDirection == Vector3.zero)
            {
                _t = 0;
                return _targetPos;
            }
            
            // 如果相机的视线是水平的，直接不处理（虽然也有可以处理的情况，但是就是硬气，不处理了）
            if (cameraDirection.y == 0)
            {
                _t = float.NaN;
                return _cameraPosition;
            }
            
            // 简单求解一下：
            // cameraPos 简写成 c.x, c.y, c.z
            // cameraDirection 简写成 d.x, d.y, d.z
            // targetPos 简写成 t.x, t.y, t.z
            // 计算结果的坐标简写成 r.x, r.y, r.z
            // 要求的就是 _t 和 r.x, r.y, r.z
            // 那么可以列一个等式
            // 因为要保持高度不变，就有 r.y = c.y ---- (1)
            // | r.x |        | d.x |   | t.x |
            // | r.y | + _t * | d.y | = | t.y |  
            // | r.z |        | d.z |   | t.z |
            // 根据第二行，可以得到 _t = (t.y - r.y) / d.y
            // 带入结果 (1)，的带 _t = (t.y - c.y) / d.y ---- (2)
            // 把 (2) 的结果带回上面的等式就可以直接算出 r.x, r.y, r.z

            _t = (_targetPos.y - _cameraPosition.y) / cameraDirection.y;
            return _targetPos - _t * cameraDirection;
        }
        /// <summary>
        /// 保持相机的高度和方向不变，计算相机的新位置，使其能看到目标点
        /// </summary>
        /// <param name="_cameraPosition">相机目前的坐标</param>
        /// <param name="_cameraFocusPosition">相机目前看着的点</param>
        /// <param name="_targetPos">新的想要让相机看着的点</param>
        /// <param name="_t">目标点到相机新的位置的距离，如果目标点在相机视线背后，就是负的</param>
        /// <returns>相机新的位置，让相机保持高度和方向不变，能够看到目标点</returns>
        public static Vector3 getNewCameraPosKeepHeightAndDirectionToSeeTargetXY(Vector3 _cameraPosition, Vector3 _cameraFocusPosition, Vector3 _targetPos, out float _t)
        {
            // 相机朝着的方向
            Vector3 cameraDirection = (_cameraFocusPosition - _cameraPosition).normalized;
            // 如果两个点是一样的，直接放回目标点的坐标即可
            if (cameraDirection == Vector3.zero)
            {
                _t = 0;
                return _targetPos;
            }
            
            // 如果相机的视线是水平的，直接不处理（虽然也有可以处理的情况，但是就是硬气，不处理了）
            if (cameraDirection.z == 0)
            {
                _t = float.NaN;
                return _cameraPosition;
            }

            _t = (_targetPos.z - _cameraPosition.z) / cameraDirection.z;
            return _targetPos - _t * cameraDirection;
        }
        /// <summary>
        /// 保持相机的高度和方向不变，计算相机的新位置，使其能看到目标点
        /// </summary>
        /// <param name="_cameraPosition">相机目前的坐标</param>
        /// <param name="_cameraFocusPosition">相机目前看着的点</param>
        /// <param name="_targetPos">新的想要让相机看着的点</param>
        /// <param name="_t">目标点到相机新的位置的距离，如果目标点在相机视线背后，就是负的</param>
        /// <returns>相机新的位置，让相机保持高度和方向不变，能够看到目标点</returns>
        public static Vector3 getNewCameraPosKeepHeightAndDirectionToSeeTargetYZ(Vector3 _cameraPosition, Vector3 _cameraFocusPosition, Vector3 _targetPos, out float _t)
        {
            // 相机朝着的方向
            Vector3 cameraDirection = (_cameraFocusPosition - _cameraPosition).normalized;
            // 如果两个点是一样的，直接放回目标点的坐标即可
            if (cameraDirection == Vector3.zero)
            {
                _t = 0;
                return _targetPos;
            }
            
            // 如果相机的视线是水平的，直接不处理（虽然也有可以处理的情况，但是就是硬气，不处理了）
            if (cameraDirection.x == 0)
            {
                _t = float.NaN;
                return _cameraPosition;
            }

            _t = (_targetPos.x - _cameraPosition.x) / cameraDirection.x;
            return _targetPos - _t * cameraDirection;
        }

        /// <summary>
        /// 计算参考坐标系的转动惯量
        /// </summary>
        public static Matrix4x4 getInertiaReference(Vector3[] _vertices, int _vertexMass)
        {
            Matrix4x4 result = Matrix4x4.zero;
            if (_vertices != null)
            {
                for (int i = 0; i < _vertices.Length; i++)
                {
                    float diag = _vertexMass * _vertices[i].sqrMagnitude;
                    result[0, 0] += diag;
                    result[1, 1] += diag;
                    result[2, 2] += diag;
                    result[0, 0] -= _vertexMass * _vertices[i].x * _vertices[i].x;
                    result[0, 1] -= _vertexMass * _vertices[i].x * _vertices[i].y;
                    result[0, 2] -= _vertexMass * _vertices[i].x * _vertices[i].z;
                    result[1, 0] -= _vertexMass * _vertices[i].y * _vertices[i].x;
                    result[1, 1] -= _vertexMass * _vertices[i].y * _vertices[i].y;
                    result[1, 2] -= _vertexMass * _vertices[i].y * _vertices[i].z;
                    result[2, 0] -= _vertexMass * _vertices[i].z * _vertices[i].x;
                    result[2, 1] -= _vertexMass * _vertices[i].z * _vertices[i].y;
                    result[2, 2] -= _vertexMass * _vertices[i].z * _vertices[i].z;
                }
            }

            result[3, 3] = 1;
            return result;
        }
        
        /// <summary>
        /// 把向量叉乘的左边那个向量转换成一个矩阵
        /// </summary>
        public static Matrix4x4 getCrossMatrix(Vector3 _vector)
        {
            Matrix4x4 result = Matrix4x4.zero;
            result[0, 0] = 0;
            result[0, 1] = -_vector.z;
            result[0, 2] = _vector.y;
            result[1, 0] = _vector.z;
            result[1, 1] = 0;
            result[1, 2] = -_vector.x;
            result[2, 0] = -_vector.y;
            result[2, 1] = _vector.x;
            result[2, 2] = 0;
            result[3, 3] = 1;
            return result;
        }

        public static _ALogicPlane2DPosGetter getAxisAlignedPlanePosGetter(Vector3 _forward)
        {
            Vector3 normalizeFroward = _forward.normalized;
            float sinValue = Mathf.Sin(Mathf.Deg2Rad * 10);
        
            //开始检测的时候检查摄像头和目标位置的坐标偏差，以此判断是平视还是垂直
            if (Mathf.Abs(normalizeFroward.y) < sinValue)
            {
                if (Mathf.Abs(normalizeFroward.z) < sinValue)
                {
                    Debug.Log_EditorOnly($"camera move panel use [zy]");
                    //此时表示平视
                    return new LogicPlane2DPosGetterZY(Vector3.zero);
                }
                else
                {
                    Debug.Log_EditorOnly($"camera move panel use [xy]");
                    //此时表示平视
                    return new LogicPlane2DPosGetterXY(Vector3.zero);
                }
            }
            //其他情况下按照默认处理
            else
            {
                Debug.Log_EditorOnly($"camera move panel use [xz]");
                return new LogicPlane2DPosGetterXZ(Vector3.zero);
            }
        }
        
        /// <summary>
        /// 计算直线与平面的交点
        /// </summary>
        /// <param name="point">直线上某一点</param>
        /// <param name="direct">直线的方向</param>
        /// <param name="planeNormal">垂直于平面的的向量</param>
        /// <param name="planePoint">平面上的任意一点</param>
        /// <returns></returns>
        public static bool getIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint,out Vector3 result)
        {
            result = Vector3.zero; 
            //要注意直线和平面平行的情况
            float d1 = Vector3.Dot(direct.normalized, planeNormal);
            if(d1 == 0)return false;
            float d2 = Vector3.Dot(planePoint - point, planeNormal);
            float d3 = d2 / d1;
 
            result = d3 * direct.normalized + point;
            return true;
        }
        
        /// <summary>
        /// 把一个 int 看作 32 位的颜色值，转成 Color32
        /// </summary>
        public static Color32 getColor32FromInt(int _colorInt) 
        {
            byte r = (byte)(_colorInt >> 24);
            byte g = (byte)(_colorInt >> 16);
            byte b = (byte)(_colorInt >> 8);
            byte a = (byte)(_colorInt);
            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// 把一个 Color32 转成 int 32 位的字节码
        /// </summary>
        public static int getColor32Int(Color32 _color)
        {
            int result = 0;
            result |= _color.r << 24;
            result |= _color.g << 16;
            result |= _color.b << 8;
            result |= _color.a;
            return result;
        }
        
        /// <summary>
        /// 判断两个颜色是否相等
        /// </summary>
        public static bool Color32Equal(Color32 _a, Color32 _b)
        {
            return _a.r == _b.r && _a.g == _b.g && _a.b == _b.b && _a.a == _b.a;
        }

        /// <summary>
        /// 把一个表示为 HDR 的 Color32 转成 Color 类型
        /// </summary>
        /// <remarks>
        /// HDR 的 Color32 没有 A 值， A 值被用作 Intensity 了，转换时直接忽略
        /// </remarks>
        public static Color getColorFromHDRColor32(Color32 _color)
        {
            // todo: 正确的不应该忽略 Intensity ，应该乘上去
            return new Color(_color.r / 255.0f, _color.g / 255.0f, _color.b / 255.0f, 1);
        }
        /// <summary>
        /// Loops the value t, so that it is never larger than length and never smaller than 0
        /// </summary>
        /// <remarks>
        /// [0, length)
        /// </remarks>
        [Pure]
        public static int intRepeat(int _t, int _length)
        {
            if (_length <= 0)
                return 0;
            
            if (_t >= 0)
                return _t % _length;
            
            return _t % _length + _length;
        }
        public static float SpringDamp(float current, float target, ref float velocity, float smoothTime, float deltaTime)
        {
            return SpringDamp(current, target, ref velocity, smoothTime, deltaTime, float.MaxValue);
        }

        public static float SpringDamp(float current, float target, ref float velocity, float smoothTime, float deltaTime, float maxOvershoot)
        {
            smoothTime = Mathf.Max(0.0001f, smoothTime);

            float dampingRatio = 0.2f;

            float omega = 2f / smoothTime;
            float displacement = current - target;

            float dampingOmega = omega * dampingRatio;
            float decay = Mathf.Exp(-dampingOmega * deltaTime);

            float oscillationOmega = omega * Mathf.Sqrt(1f - dampingRatio * dampingRatio);
            float cos = Mathf.Cos(oscillationOmega * deltaTime);
            float sin = Mathf.Sin(oscillationOmega * deltaTime);

            float newDisplacement = decay * (
                displacement * cos +
                (velocity + dampingOmega * displacement) / oscillationOmega * sin
            );

            float newVelocity = decay * (
                (velocity + dampingOmega * displacement) * cos -
                oscillationOmega * displacement * sin
            );

            if (Mathf.Abs(newDisplacement) > maxOvershoot)
            {
                float clampedDisplacement = Mathf.Sign(newDisplacement) * maxOvershoot;
                float overshootAmount = Mathf.Abs(newDisplacement) - maxOvershoot;

                newVelocity = newVelocity * Mathf.Max(0f, 1f - overshootAmount / maxOvershoot);
                newDisplacement = clampedDisplacement;
            }

            velocity = newVelocity;

            return target + newDisplacement;
        }
    }
}