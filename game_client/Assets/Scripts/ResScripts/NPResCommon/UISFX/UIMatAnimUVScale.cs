using UnityEngine;

namespace MG
{
public class UIMatAnimUVScale : IUIMatAnimBase
{
    [SerializeField]private string _propertyName = "_MainTex";
    protected override string propertyName { get => _propertyName; }
    
    public Vector2 _UVScale;
    private Vector2 _m_oldUVScale;

    public override void onInit(Material _mat)
    {
        if(_mat != null)
        {
            _m_oldUVScale = _mat.GetTextureOffset(_propertyName);
        }
    }
    
    private bool hasChange()
    {
        if(_m_oldUVScale.Equals(_UVScale))
        {
            return false;
        }
        _m_oldUVScale = _UVScale;
        return true;
    }

    public override void setValue(Material _mat)
    {
        if(!hasChange())
            return;

        if(_mat != null)
        {
            _mat.SetTextureScale(_m_propertyId, _UVScale);
        }
    }
}
}