using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MG
{
public class UIMaterialColorForAnimation : MonoBehaviour
{
    static Dictionary<string , int> _m_StaticPropertyIds = new Dictionary<string, int>();
    
    [SerializeField]public string _propertyName = "_Color";
    protected int _m_propertyId;
    
    public Graphic _renerer;
    public Material _material;
    protected Material _materialIns;

    public Color _m_color = Color.white;
    private Color _m_oldColor;
    protected void Awake()
    {
        if(_renerer == null)
            return;
        if(_material == null)
            _material = Canvas.GetDefaultCanvasMaterial();
        
        if(checkPropertyValid(_material))
            _materialIns = Instantiate(_material);

    }
    private void OnEnable()
    {
        if(_materialIns != null && _renerer != null)
        {
            _renerer.material = _materialIns;
            _m_oldColor = _m_color;
            _materialIns.SetColor(_m_propertyId, _m_color);
        }
    }

    private void Update()
    {
        if(_m_oldColor == _m_color || _materialIns == null)
        {
            return;
        }
        _m_oldColor = _m_color;
        _materialIns.SetColor(_m_propertyId, _m_color);
    }

    protected void OnDestroy()
    {
        if(_materialIns != null)
        {
            Destroy(_materialIns);
        }
    }
    
    public bool checkPropertyValid(Material _mat)
    {
        if(string.IsNullOrEmpty(_propertyName))
        {
            UnityEngine.Debug.LogError("Property 名字为空 需要检查");
            return false;
        }
        if(_mat == null)
        {
            UnityEngine.Debug.LogError($"material 为空");
            return false;
        }
        if(!_m_StaticPropertyIds.TryGetValue(_propertyName, out _m_propertyId))
        {
            _m_propertyId = Shader.PropertyToID(_propertyName);
            _m_StaticPropertyIds.Add(_propertyName, _m_propertyId);
        }
        if(_mat.HasProperty(_m_propertyId))
        {
            return true;
        }
        UnityEngine.Debug.LogError($"材质{_mat}不存在 Property:{_propertyName}");
        return false;
    }
}
}