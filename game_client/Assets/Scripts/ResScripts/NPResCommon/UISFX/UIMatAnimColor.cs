using UnityEngine;

namespace MG
{
public class UIMatAnimColor : IUIMatAnimBase
{
    [SerializeField]private string _propertyName = "_Color";
    protected override string propertyName { get => _propertyName; }
    
    public Color _color = Color.white;
    private Color _m_oldColorValue;

    public override void onInit(Material _mat)
    {
        if(_mat != null)
        {
            _m_oldColorValue = _mat.GetColor(_m_propertyId);
        }
    }
    
    private bool hasChange()
    {
        if(_m_oldColorValue.Equals(_color))
        {
            return false;
        }
        _m_oldColorValue = _color;
        return true;
    }

    public override void setValue(Material _mat)
    {
        if(!hasChange())
            return;

        if(_mat != null)
        {
            _mat.SetColor(_m_propertyId, _color);
        }
    }
    
    
}
}