using System;
using System.Collections;
using System.Collections.Generic;
using MG;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{
[Serializable]
public class TinyAnim_Position : ITinyAnimKey
{
    [SerializeField]public Vector3 startPosition;
    [SerializeField]public Vector3 endPosition;
        
    public override void onValueChange(Material _mat, float _duration)
    {
        if(_mat != null)
        {
            _mat.SetVector(_m_propertyId, Vector3.Lerp(startPosition, endPosition, _duration));
        }
    }
}

public class UIMaterialAnimPosition : UIMaterialAnimBase
{
    public TinyAnim_Position _tineAnimPosition ;
    
    protected override bool checkValid(Material _mat)
    {
        return _tineAnimPosition != null && _tineAnimPosition.checkPropertyValid(_mat);
    }

    protected override void setValue(Material _mat, float _duration)
    {
        _tineAnimPosition?.setValue(_materialIns, _duration);
    }
}
}