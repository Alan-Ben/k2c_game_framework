using UnityEngine;

namespace MG
{
public class UIMatAnimFloat : IUIMatAnimBase
{
    [SerializeField]private string _propertyName = "_Float";
    protected override string propertyName { get => _propertyName; }

    public float _floatValue = 1;
    private float _m_oldFloat;
    private float _m_defaultFloat;

    public override void onInit(Material _mat)
    {
        if(_mat != null)
        {
            _m_defaultFloat = _mat.GetFloat(_m_propertyId);
            _m_oldFloat = _m_defaultFloat;
        }
    }

    private bool hasChange()
    {
        if(_m_oldFloat.Equals(_floatValue))
        {
            return false;
        }
        _m_oldFloat = _floatValue;
        _m_defaultFloat = _floatValue;
        return true;
    }

    public override void setValue(Material _mat)
    {
        if(!hasChange())
            return;

        if(_mat != null)
        {
            _mat.SetFloat(_m_propertyId, _m_defaultFloat);
        }
    }
}
}