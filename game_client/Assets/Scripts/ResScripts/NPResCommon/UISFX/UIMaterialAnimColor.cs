using System;
using System.Collections;
using System.Collections.Generic;
using MG;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{
[Serializable]
public class TinyAnim_Color : ITinyAnimKey
{
    [SerializeField]public Color startColor = Color.white;
    [SerializeField]public Color endColor = Color.white;
        
    public override void onValueChange(Material _mat, float _duration)
    {
        if(_mat != null)
        {
            _mat.SetColor(_m_propertyId, Color.Lerp(startColor, endColor, _duration));
        }
    }
}

public class UIMaterialAnimColor : UIMaterialAnimBase
{
    public TinyAnim_Color _colorTimeAnim ;
    
    protected override bool checkValid(Material _mat)
    {
        return _colorTimeAnim != null && _colorTimeAnim.checkPropertyValid(_mat);
    }

    protected override void setValue(Material _mat, float _duration)
    {
        _colorTimeAnim?.setValue(_materialIns, _duration);
    }
}
}