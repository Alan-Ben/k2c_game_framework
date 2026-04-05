using UnityEngine;

namespace MG
{
public class UIMatAnimUVOffset : IUIMatAnimBase
{
    [SerializeField]private string _propertyName = "_MainTex";
    protected override string propertyName { get => _propertyName; }
    
    public Vector2 _UVOffset;
    private Vector2 _m_oldUVOffset;

    public override void onInit(Material _mat)
    {
        if(_mat != null)
        {
            _m_oldUVOffset = _mat.GetTextureOffset(_propertyName);
        }
    }
    
    private bool hasChange()
    {
        if(_m_oldUVOffset.Equals(_UVOffset))
        {
            return false;
        }
        _m_oldUVOffset = _UVOffset;
        return true;
    }

    public override void setValue(Material _mat)
    {
        if(!hasChange())
            return;

        if(_mat != null)
        {
            _mat.SetTextureOffset(_m_propertyId, _UVOffset);
        }
    }
}
}
