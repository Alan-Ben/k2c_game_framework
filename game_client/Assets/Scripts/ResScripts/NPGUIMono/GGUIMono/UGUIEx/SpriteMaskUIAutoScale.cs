using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 实现SpriteMask遮挡特效自动缩放
/// </summary>
[ExecuteAlways]
public class SpriteMaskUIAutoScale : UIBehaviour
{
    [ALHeader("目标缩放对比的ui")]
    public RectTransform _m_target;
    
    [ALHeader("缩放比例X")]
    public float _m_fScaleX = 25f;
    [ALHeader("缩放比例Y")]
    public float _m_fScaleY = 25f;

    // Start is called before the first frame update
    void OnEnable()
    {
        _setScaleByRect();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        _setScaleByRect();
    }

    private void _setScaleByRect()
    {
        if(_m_target != null)
        {
            var rect = _m_target.rect;
            float x = rect.width * _m_fScaleX;
            float y = rect.height * _m_fScaleY;
            transform.localScale = new Vector3(x,y,1);
        }
    }

}
