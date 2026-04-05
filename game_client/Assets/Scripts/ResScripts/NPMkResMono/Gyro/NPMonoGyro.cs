using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// UI陀螺仪
public class NPMonoGyro : MonoBehaviour
{
    // 是否支持陀螺仪
    private bool _m_bIsSupport = false;
    // 物体是否显示中
    private bool _m_iIsActive = false;

    // 当前屏幕方向
    private ScreenOrientation _m_eOrientation;

    private void Awake()
    {
        // 是否支持陀螺仪
        _m_bIsSupport = SystemInfo.supportsGyroscope;

        if (_m_bIsSupport)
            Input.gyro.enabled = true;

        _onInit();
    }

    private void OnEnable()
    {
        // 记录当前屏幕方向
        _m_eOrientation = Screen.orientation;

        _m_iIsActive = true;
    }

    private void OnDisable()
    {
        _m_iIsActive = false;

        _onReset();
    }

    private void LateUpdate()
    {
        if (!_m_iIsActive || !_m_bIsSupport)
            return;

        // 屏幕翻转时
        if (Screen.orientation != _m_eOrientation)
        {
            _m_eOrientation = Screen.orientation;
            _onReset();

            return;
        }

        _onMove(Input.gyro.rotationRateUnbiased);
    }

    // 初始化
    protected virtual void _onInit() { }
    // 重置
    protected virtual void _onReset() { }
    // 移动
    protected virtual void _onMove(Vector3 _xyzAngle) { }
}
