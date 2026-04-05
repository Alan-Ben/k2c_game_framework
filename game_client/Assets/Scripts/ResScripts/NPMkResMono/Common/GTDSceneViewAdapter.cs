
using System;
using ALPackage;
using GOE;
using UnityEngine;

public class GTDSceneViewAdapter : MonoBehaviour
{
    [ALHeader("宽高比")]
    public float normalRatio = 1080f / 1920f;
    [ALHeader("在上面的宽高比下的高度.")]
    [ALHeader("一般相机配置调好后，")]
    [ALHeader("这个高度按相机在1080x1920分辨率下的范围来配")]
    public float normalHeight = 1;
    [ALHeader("自适应锚点")]
    [Range(0,1)]
    public float pivotY;
    [ALHeader("为了匹配显示范围（编辑器下方便显示范围用，实际计算偏移时候没用）")]
    public float minPosY;
    [ALHeader("最小宽高比，低于这个宽高比则按最小宽高比自适应")]
    public float minRatio = 0.01f;
    [ALHeader("最大宽高比，高于这个宽高比按最大宽高比做自适应，源于相机有限制最大宽高比")]
    public float maxRatio = 1080f / 1920f;

    private Transform m_Transform;
    private Vector3 m_originPosition;

    private float _m_newHeight;
    private float _m_newMin;
    private float _m_screenRatio = 1080f / 1920f;

    public Vector3 originPosition
    {
        get
        {
            if (m_Transform == null)
            {
                m_Transform = transform;
                m_originPosition = m_Transform.position;
            }
            return m_originPosition;
        }
    }

    private void OnEnable()
    {
        _m_screenRatio = (float) Screen.width /Screen.height ;
        ALGUIMain.instance.screenSizeChgDelegate += _onScreenSizeChange;
        _updateTranPos();
    }

    private void OnDisable()
    {
        ALGUIMain.instance.screenSizeChgDelegate -= _onScreenSizeChange;
    }

    private void OnValidate()
    {
        _updateTranPos();
    }

    private void _onScreenSizeChange(int _width, int _height)
    {
        _m_screenRatio = (float) Screen.width /Screen.height ;
        _updateTranPos();
    }

    private void _updateTranPos()
    {
        float viewRatio = Math.Min(maxRatio, _m_screenRatio);
        viewRatio = Math.Max(minRatio, viewRatio);
        
        float staticPos = minPosY + pivotY * normalHeight;
        
        _m_newHeight = normalHeight / viewRatio * normalRatio;
        _m_newMin = staticPos - pivotY * _m_newHeight;

        float offsetY = minPosY + 0.5f * normalHeight - (_m_newMin + 0.5f * _m_newHeight);
        transform.position = originPosition + new Vector3(0, offsetY, 0);
    }

    public Rect calculateNewRect()
    {
        float width = normalHeight * normalRatio;
        Rect nRect = new Rect(-0.5f *width, _m_newMin, width, _m_newHeight);
        return nRect;
    }
}
