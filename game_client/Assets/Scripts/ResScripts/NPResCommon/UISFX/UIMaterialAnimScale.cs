using System;
using UnityEngine;
namespace MG
{
[Serializable]
public class TinyAnim_Scale : ITinyAnimKey
{
    [SerializeField]public Vector2 startPosition;
    [SerializeField]public Vector2 endPosition;
    private Vector4 _m_para;
        
    public override void onValueChange(Material _mat, float _duration)
    {
        if(_mat != null)
        {
            var curScale = Vector2.Lerp(startPosition, endPosition, _duration);
            _m_para.z = curScale.x;
            _m_para.w = curScale.y;
            _mat.SetVector(_m_propertyId, _m_para);
        }
    }

    public void setCenterPos(Vector2 _centerPos)
    {
        _m_para.x = _centerPos.x;
        _m_para.y = _centerPos.y;
    }
}

public class UIMaterialAnimScale : UIMaterialAnimBase
{
    public TinyAnim_Scale _tinyAnimScale ;
    
    protected override bool checkValid(Material _mat)
    {
        return _tinyAnimScale != null && _tinyAnimScale.checkPropertyValid(_mat);
    }

    new void Awake()
    {
        base.Awake();
        if(_renerer != null && _materialIns)
        {
            _renerer.RegisterDirtyLayoutCallback(setCenterPos);
        }
    }

    new void OnDestroy()
    {
        if(_renerer != null && _materialIns)
        {
            _renerer.UnregisterDirtyLayoutCallback(setCenterPos);
        }
        base.OnDestroy();
        
    }
    protected override void setValue(Material _mat, float _duration)
    {
        _tinyAnimScale?.setValue(_materialIns, _duration);
    }

    private void setCenterPos()
    {
        if(_renerer == null)
        {
            return;
        }
        if(_renerer.canvas == null)
        {
            return;
        }
        if(_renerer.rectTransform == null)
        {
            return;
        }
        if(_tinyAnimScale == null)
        {
            return;
        }
       
        Rect rect = _renerer.canvas.pixelRect;
        Vector2 centerPos = _renerer.rectTransform.position / rect.size;
        centerPos = centerPos * 2 - Vector2.one;
        _tinyAnimScale.setCenterPos(centerPos);
    }
}
}