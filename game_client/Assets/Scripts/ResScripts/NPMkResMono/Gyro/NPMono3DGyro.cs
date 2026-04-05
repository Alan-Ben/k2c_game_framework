using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Gyro3DSettings
{
    // 是否开启
    public bool enable;
    // 敏感值
    public float sensitivity;
    // 最大位移
    public float maxDistance;
    // 倾斜方向向量
    public Vector2 directionVector;
}

// 2d陀螺仪
public class NPMono3DGyro : NPMonoGyro
{
    // 目标物体
    public Transform target;

    // 水平设置
    public Gyro3DSettings horizontalSettings;
    public Gyro3DSettings verticalSettings;

    // 旋转累加值
    private float _m_fTotalRotationX;
    private float _m_fTotalRotationY;

    // 水平总位移
    private float _m_fHorizontalTotalMove;
    private float _m_fVerticalTotalMove;

    // 最大旋转值
    private float _m_fMaxAbsRotationX;
    private float _m_fMaxAbsRotationY;

    // 当前帧水平位移
    private float _m_fHorizontalFrameMove;
    private float _m_fVerticalFrameMove;

    // ui原始坐标
    private Vector2 _m_vOriginalPos;

    private float _m_fTempValueX;
    private float _m_fTempValueY;

    protected override void _onInit()
    {
        if (target == null)
            return;

        // 记录UI的原始位置
        _m_vOriginalPos = target.localPosition;

        if (horizontalSettings != null)
        {
            // 最大旋转值
            _m_fMaxAbsRotationY = horizontalSettings.maxDistance / horizontalSettings.sensitivity;

            // 标准化方向向量
            horizontalSettings.directionVector.Normalize();
        }

        if (verticalSettings != null)
        {
            // 最大旋转值
            _m_fMaxAbsRotationX = verticalSettings.maxDistance / verticalSettings.sensitivity;

            // 标准化方向向量
            verticalSettings.directionVector.Normalize();
        }
    }

    protected override void _onMove(Vector3 _xyzAngle)
    {
        if (target == null)
            return;

        _m_fTempValueX = 0;
        _m_fTempValueY = 0;

        // 处理水平方向位移
        if (horizontalSettings != null && horizontalSettings.enable)
        {
            // 累加旋转值
            _m_fTotalRotationY += _xyzAngle.y;

            // 向右过度旋转
            if (_m_fTotalRotationY > _m_fMaxAbsRotationY)
            {
                _m_fHorizontalFrameMove = horizontalSettings.maxDistance - _m_fHorizontalTotalMove;
            }
            // 向左过度旋转
            else if (_m_fTotalRotationY < -_m_fMaxAbsRotationY)
            {
                _m_fHorizontalFrameMove = -horizontalSettings.maxDistance - _m_fHorizontalTotalMove;
            }
            // 不存在过度旋转
            else
            {
                _m_fHorizontalFrameMove = horizontalSettings.sensitivity * _xyzAngle.y;
            }

            // 净位移
            _m_fHorizontalTotalMove += _m_fHorizontalFrameMove;

            // 方向位移
            _m_fTempValueX += horizontalSettings.directionVector.x * _m_fHorizontalFrameMove;
            _m_fTempValueY += horizontalSettings.directionVector.y * _m_fHorizontalFrameMove;
        }

        // 处理竖直方向位移
        if (verticalSettings != null && verticalSettings.enable)
        {
#if UNITY_IOS
            _m_fTotalRotationX += _xyzAngle.x;
#elif UNITY_ANDROID
            _m_fTotalRotationX -= _xyzAngle.x;
#endif

            // 向上过度旋转
            if (_m_fTotalRotationX > _m_fMaxAbsRotationX)
            {
                _m_fVerticalFrameMove = verticalSettings.maxDistance - _m_fVerticalTotalMove;
            }
            // 向下过度旋转
            else if (_m_fTotalRotationX < -_m_fMaxAbsRotationX)
            {
                _m_fVerticalFrameMove = -verticalSettings.maxDistance - _m_fVerticalTotalMove;
            }
            // 不存在过度旋转
            else
            {
#if UNITY_IOS
            _m_fVerticalFrameMove = verticalSettings.sensitivity * _xyzAngle.x;
#elif UNITY_ANDROID
                _m_fVerticalFrameMove = -verticalSettings.sensitivity * _xyzAngle.x;
#endif
            }

            // 净位移
            _m_fVerticalTotalMove += _m_fVerticalFrameMove;

            // 方向位移
            _m_fTempValueX += verticalSettings.directionVector.x * _m_fVerticalFrameMove;
            _m_fTempValueY += verticalSettings.directionVector.y * _m_fVerticalFrameMove;
        }

        // 做位移
        target.localPosition += new Vector3(_m_fTempValueX, _m_fTempValueY, 0);
    }

    // 重置数据
    protected override void _onReset()
    {
        _m_fTotalRotationX = 0;
        _m_fTotalRotationY = 0;
        _m_fHorizontalTotalMove = 0;
        _m_fVerticalTotalMove = 0;
        _m_fHorizontalFrameMove = 0;
        _m_fVerticalFrameMove = 0;
        _m_fTempValueX = 0;
        _m_fTempValueY = 0;

        if (target != null)
            target.localPosition = _m_vOriginalPos;
    }
}
