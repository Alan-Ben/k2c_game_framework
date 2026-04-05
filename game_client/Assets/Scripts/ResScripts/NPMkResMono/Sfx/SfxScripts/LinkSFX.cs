using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkSFX : MonoBehaviour
{
    //UV跳跃速度
    public Vector2 texOffset = Vector2.one;
    //跳跃间隔
    public float texOffsetInterval = 0.1f;
    //当前计数时间
    private float _m_fTime = 0f;
    //UV动画的material
    private Material _m_material;
    //初始缩放
    private float _m_initScale = 1f;

    // Use this for initialization
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            return;
        }
        _m_material = renderer.material;
        _m_initScale = _m_material.mainTextureScale.y;
    }

    public void SetTexScale(float _scale)
    {
        Vector2 scale = _m_material.mainTextureScale;
        scale.y = _m_initScale * _scale;
        _m_material.mainTextureScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_m_material == null)
        {
            return;
        }

        //累加时间
        _m_fTime += Time.unscaledDeltaTime;
        if(_m_fTime > texOffsetInterval)
        {
            _m_fTime = 0f;
            //切换UV
            Vector2 offset = _m_material.mainTextureOffset;
            offset += texOffset;
            if (offset.y > 1f)
                offset.y -= 1f;
            if (offset.x > 1f)
                offset.x -= 1f;

            _m_material.mainTextureOffset = offset;
        }
    }
}
