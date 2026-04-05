using UnityEngine;

namespace MG
{
public class UIMatAnimAlpha : IUIMatAnimBase
{
    [SerializeField]private string _propertyName = "_Color";
    protected override string propertyName { get => _propertyName; }
    
    public float _alpha = 1;
    private float _m_oldAlpha;
    private Color _m_defaultColor;

    public override void onInit(Material _mat)
    {
        if(_mat != null)
        {
            _m_defaultColor = _mat.GetColor(_m_propertyId);
            _m_oldAlpha = _m_defaultColor.a;
        }
    }

    private bool hasChange()
    {
        if(_m_oldAlpha.Equals(_alpha))
        {
            return false;
        }
        _m_oldAlpha = _alpha;
        _m_defaultColor.a = _alpha;
        return true;
    }

    public override void setValue(Material _mat)
    {
        if(!hasChange())
            return;

        if(_mat != null)
        {
            _mat.SetColor(_m_propertyId, _m_defaultColor);
        }
    }
}
}