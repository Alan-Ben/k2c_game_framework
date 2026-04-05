using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAtlasAlphaUVRemap : MonoBehaviour
{
    [Header("是否创建材质实例,【不创建实例的情况下，同屏出现多个可能显示异常，请留意】")]
    public bool isCreateInstanceMaterial = true;
    private Material _m_material;
    private Image _m_img;
    private static readonly int _m_shaderPro_UvRect = Shader.PropertyToID("_UVRect");
    protected virtual void Awake()
    {
        _m_img = GetComponent<Image>();
        if(_m_img != null)
        {
            if(isCreateInstanceMaterial)
            {
                _m_material = Instantiate(_m_img.material);
                _m_img.material = _m_material;
            }
            else
                _m_material = _m_img.material;

            setRemapUvRect(_m_material);
            _m_img.RegisterDirtyVerticesCallback(onNeedRemapUV);
        }
    }

    private void onNeedRemapUV()
    {
        setRemapUvRect(_m_material);
    }
    
    private void setRemapUvRect(Material _material)
    {
        if(_m_img == null)
        {
            return;
        }
        Sprite spt = _m_img.sprite;
        if(spt == null)
        {
            return;
        }
        Rect texRect = spt.textureRect;
        Vector2 texRectOff = spt.textureRectOffset;
        if(spt.texture == null)
        {
            return;
        }
        Vector2 texSize = new Vector2(spt.texture.width, spt.texture.height);
        Vector4 uvRect = new Vector4((texRect.x-texRectOff.x )/ texSize.x, (texRect.y-texRectOff.y) / texSize.y,
            (spt.rect.width) / texSize.x,(spt.rect.height) / texSize.y);
        if(_material != null)
        {
            _material.SetVector(_m_shaderPro_UvRect, uvRect);
        }
    }

    private void OnDestroy()
    {
        if(_m_img != null)
        {
            _m_img.UnregisterDirtyVerticesCallback(onNeedRemapUV);
        }
        if(isCreateInstanceMaterial)
            Destroy(_m_material);
    }
}

