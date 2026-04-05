using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

//基于mesh实现的技能指示器
public class WCGSectorMeshIndicatorMono : _AWCGBasicRangeTagMono
{
    //显示的mesh
    public Transform meshIndicator;
    //扇形左边
    public Transform leftEdge;
    //扇形右边
    public Transform rightEdge;
    //扇形图片的uv起始坐标
    private Vector2 uvBegin = new Vector2(float.MaxValue, float.MaxValue);
    //扇形图片的uv结束坐标
    private Vector2 uvEnd = new Vector2(float.MinValue, float.MinValue);
    //默认半径
    private float _m_fInitRadiusScale;
    //扇形角度
    private float _m_fAngle;
    //limit模式，显示的半径或者长度为相对位置的距离(setTargetRelativePos)
    private bool _m_bLimit;
    //最大长度
    private float _m_fMaxLength;
    //需要根据扇形角度生成的mesh filter
    private MeshFilter _m_meshFilter;
    //生成的扇形mesh的初始半径
    private float _m_fInitRadius;

    public void Awake()
    {
        if (null == meshIndicator)
            return;

        //获取初始半径
        _m_fInitRadiusScale = meshIndicator.localScale.x;
        _m_bLimit = false;
        _m_fMaxLength = -1f;
        _m_meshFilter = meshIndicator.GetComponent<MeshFilter>();

        _m_fInitRadius = _m_meshFilter.mesh.bounds.size.z / 2f;
        Vector2[] uvs = _m_meshFilter.mesh.uv;
        for (int i = 0; i < uvs.Length; i++)
        {
            uvBegin.x = Mathf.Min(uvs[i].x, uvBegin.x);
            uvBegin.y = Mathf.Min(uvs[i].y, uvBegin.y);
            uvEnd.x = Mathf.Max(uvs[i].x, uvEnd.x);
            uvEnd.y = Mathf.Max(uvs[i].y, uvEnd.y);
        }
        setAngle(270f);
    }

    //设置对象位置
    public override void setPos(Vector3 _pos)
    {
        transform.position = _pos;
    }
    //设置对象宽度或半径
    public override void setWidth(float _width)
    {
        if (null == meshIndicator)
            return;

        if (_m_fMaxLength > 0f && _width >= _m_fMaxLength)
        {
            _width = _m_fMaxLength;
        }

        //设置投影宽度
        Vector3 scale = meshIndicator.localScale;
        scale.x = scale.z = _m_fInitRadiusScale * _width;
        meshIndicator.localScale = scale;
    }
    //设置扇形角度
    public override void setAngle(float _angle)
    {
        if (_angle < 0)
        {
            _angle = 0;
        }
        if (_angle > 360)
        {
            _angle = 360;
        }
        if (_m_fAngle.Equals(_angle))
        {
            return;
        }
        _m_fAngle = _angle;
        _regenerateMesh();
        if (leftEdge != null)
        {
            leftEdge.localRotation = Quaternion.Euler(0f, -_m_fAngle / 2f, 0f);
        }
        if (rightEdge != null)
        {
            rightEdge.localRotation = Quaternion.Euler(0f, _m_fAngle / 2f, 0f);
        }
    }

    public override void setForward(Vector3 _forward)
    {
        _forward.y = 0f;
        transform.rotation = Quaternion.LookRotation(_forward);
    }
    //设置目标相对位置
    public override void setTargetRelativePos(Vector3 _pos, float _minDis)
    {
        if (_m_bLimit)
        {
            setWidth(_pos.magnitude);
        }
    }
    //设置Limit模式，显示的半径或者长度为相对位置的距离(setTargetRelativePos)
    public override void setLimitMode(bool limitMode)
    {
        _m_bLimit = limitMode;
    }
    //作为缓存对象重置接口
    public override void reset()
    {
        _m_bLimit = false;
        _m_fMaxLength = -1f;
    }
    //设置最大宽度或半径
    public override void setMaxWidth(float _width)
    {
        _m_fMaxLength = _width;
    }
    //重新生成mesh
    private void _regenerateMesh()
    {
        if (_m_meshFilter == null)
        {
            return;
        }
        List<Vector3> vertexes = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        float uvWidth = uvEnd.x - uvBegin.x;
        float uvHalfWidth = uvWidth / 2f;
        float uvHeight = uvEnd.y - uvBegin.y;
        float uvHalfHeight = uvHeight / 2f;
        float uvMidX = uvBegin.x + uvHalfWidth;
        float uvMidY = uvBegin.y + uvHalfHeight;

        vertexes.Add(new Vector3(0f, 0f, 0f));
        uvs.Add(new Vector2(uvMidX, uvMidY));

        float halfRad = _m_fAngle / 2f * Mathf.Deg2Rad;

        if (_m_fAngle <= 90)
        {
            float edgePtX = Mathf.Tan(halfRad) * _m_fInitRadius;
            Vector3 edgePtLeft = new Vector3(-edgePtX, 0f, _m_fInitRadius);
            Vector3 edgePtRight = new Vector3(edgePtX, 0f, _m_fInitRadius);

            vertexes.Add(edgePtLeft);
            vertexes.Add(edgePtRight);

            float uvEdgeX = Mathf.Tan(halfRad) * uvHalfHeight;
            uvs.Add(new Vector2(uvMidX - uvEdgeX, uvEnd.y));
            uvs.Add(new Vector2(uvMidX + uvEdgeX, uvEnd.y));

            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
        }
        else if (_m_fAngle > 270)
        {
            float edgePtX = -_m_fInitRadius * Mathf.Tan(halfRad);
            Vector3 edgePtLeft = new Vector3(-edgePtX, 0f, -_m_fInitRadius);
            Vector3 edgePtRight = new Vector3(edgePtX, 0f, -_m_fInitRadius);

            vertexes.Add(edgePtLeft);
            vertexes.Add(new Vector3(-_m_fInitRadius, 0f, -_m_fInitRadius));
            vertexes.Add(new Vector3(-_m_fInitRadius, 0f, _m_fInitRadius));
            vertexes.Add(new Vector3(_m_fInitRadius, 0f, _m_fInitRadius));
            vertexes.Add(new Vector3(_m_fInitRadius, 0f, -_m_fInitRadius));
            vertexes.Add(edgePtRight);

            float uvEdgeX = -uvHalfHeight * Mathf.Tan(halfRad);
            uvs.Add(new Vector2(uvMidX - uvEdgeX, uvBegin.y));
            uvs.Add(uvBegin);
            uvs.Add(new Vector2(uvBegin.x, uvEnd.y));
            uvs.Add(uvEnd);
            uvs.Add(new Vector2(uvEnd.x, uvBegin.y));
            uvs.Add(new Vector2(uvMidX + uvEdgeX, uvBegin.y));

            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(3);
            triangles.Add(0);
            triangles.Add(3);
            triangles.Add(4);
            triangles.Add(0);
            triangles.Add(4);
            triangles.Add(5);
            triangles.Add(0);
            triangles.Add(5);
            triangles.Add(6);
        }
        else
        {
            float edgePtZ = _m_fInitRadius / Mathf.Tan(halfRad);
            Vector3 edgePtLeft = new Vector3(-_m_fInitRadius, 0f, edgePtZ);
            Vector3 edgePtRight = new Vector3(_m_fInitRadius, 0f, edgePtZ);

            vertexes.Add(edgePtLeft);
            vertexes.Add(new Vector3(-_m_fInitRadius, 0f, _m_fInitRadius));
            vertexes.Add(new Vector3(_m_fInitRadius, 0f, _m_fInitRadius));
            vertexes.Add(edgePtRight);

            float uvEdgeY = uvHalfWidth / Mathf.Tan(halfRad);
            uvs.Add(new Vector2(uvBegin.x, uvMidY + uvEdgeY));
            uvs.Add(new Vector2(uvBegin.x, uvEnd.y));
            uvs.Add(new Vector2(uvEnd.x, uvEnd.y));
            uvs.Add(new Vector2(uvEnd.x, uvMidY + uvEdgeY));

            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(3);
            triangles.Add(0);
            triangles.Add(3);
            triangles.Add(4);
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertexes.ToArray();
        newMesh.uv = uvs.ToArray();
        newMesh.triangles = triangles.ToArray();

        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();

        Mesh oldMesh = _m_meshFilter.mesh;
        _m_meshFilter.mesh = newMesh;
        ALUnityCommon.releaseGameObj(oldMesh);
    }
}
