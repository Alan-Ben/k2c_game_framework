using System;
using UnityEngine;
namespace MG
{
public class IUIMatAnimBase : MonoBehaviour
{
    protected virtual string propertyName { get; }
    protected int _m_propertyId;
    internal bool _m_openProperty = false;

    public bool init(Material _mat)
    {
        if(checkPropertyValid(_mat))
        {
            onInit(_mat);
            return true;
        }
        return false;
    }

    public virtual void onInit(Material _mat)
    {
        
    }

    public virtual void setValue(Material _mat)
    {
        
    }

    public bool checkPropertyValid(Material _mat)
    {
        if(string.IsNullOrEmpty(propertyName))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError("Property 名字为空 需要检查");
#endif
            _m_openProperty = false;
            return false;
        }
        if(_mat == null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError($"material 为空");
#endif
            _m_openProperty = false;
            return false;
        }
        _m_propertyId = UIMatAnimController.getPropertyId(propertyName);
        if(_mat.HasProperty(_m_propertyId))
        {
            _m_openProperty = true;
            return true;
        }
#if UNITY_EDITOR
        GameObject gb = this.gameObject;
        UnityEngine.Debug.LogError($"材质{_mat}不存在 Property:{propertyName},{gb.name},{gb.transform.root.gameObject.name}",gb.transform.root.gameObject);
#endif
        _m_openProperty = false;
        return false;
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var controller = GetComponent<_IUIMatAnimControllerBase>();
        if(controller != null)
        {
            controller.refreshAnimList();
            return;
        }
        
        Debug.LogWarning($"需要添加UIMatAnimController或者UIMatAnimControllerV2组件,obj:{this.gameObject}, parent:{this.transform.parent.gameObject}", this.gameObject);
    }
#endif
}
}
