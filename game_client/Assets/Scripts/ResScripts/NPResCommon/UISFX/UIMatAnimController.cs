
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using MG;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{

    public interface _IUIMatAnimControllerBase
    {
        void refreshAnimList();
    }
[ExecuteAlways]
public class UIMatAnimController : MonoBehaviour, _IUIMatAnimControllerBase
{
    static Dictionary<string , int> _m_StaticPropertyIds = new Dictionary<string, int>();
    
    public static int getPropertyId(string _propertyName)
    {
        int id;
        if(!_m_StaticPropertyIds.TryGetValue(_propertyName, out id))
        {
            id = Shader.PropertyToID(_propertyName);
            _m_StaticPropertyIds.Add(_propertyName, id);
        }
        return id;
    }
    public Graphic _renerer;
    public Material _material;
    [NotNull]public List<IUIMatAnimBase> _animList = new List<IUIMatAnimBase>();
    
    [HideInInspector]public Material _materialIns;

    [HideInInspector]public bool _m_needAnim = false;

    [Header("需要预览的时候开启这个选项，预览完后要记得改为false")]
    public bool _m_openDebugInEditor = false;
    
    [Header("是否创建材质实例,【不创建实例的情况下，同屏出现多个可能显示异常，请留意】")]
    public bool _m_createInstanceMaterial = true;
    
    protected void Awake()
    {
        if(_renerer == null)
            return;

        init();

        #if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            if (_renerer != null)
            {
                _renerer.UnregisterDirtyLayoutCallback(renderMatValidate);
                _renerer.RegisterDirtyMaterialCallback(renderMatValidate);
            }
        }
        #endif
    }

   
    void init()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
            initIMatAnimList();
#endif      
        if(_renerer == null)
            return;
        
        Material mat;
        if (_material == null)
        {
            mat = Canvas.GetDefaultCanvasMaterial();
        }
        else
            mat =_material;

        _m_needAnim = false;
#if UNITY_EDITOR   
        if(!Application.isPlaying && !_m_openDebugInEditor)
        {
            _renerer.material = _material;
            return;
        }
#endif        
        for (int i = _animList.Count-1; i >=0; i--)
        {
            var anim = _animList[i];
            if(anim == null || !anim.init(mat))
            {
                _animList.Remove(anim);
                continue;
            }
            _m_needAnim = true;
        }
        if(!_m_needAnim)
            return;

        if (_materialIns != null)
        {
            DestroyImmediate(_materialIns);
            _materialIns = null;
        }
        
        if(_m_createInstanceMaterial)
            _materialIns = Instantiate(mat);
        else
            _materialIns = mat;

        _renerer.material = _materialIns;
    }

    private void OnEnable()
    {
        if(!_m_needAnim)
            return;
        // OnEnable 的时候更新一次，避免update延迟一帧的bug
        ALCommonActionMonoTask.addLaterMonoTask(_update);
    }

    private void Update()
    {
        _update();
    }

    private void _update()
    {
        if(!_m_needAnim)
            return;
        foreach (var ani in _animList)
        {
            ani.setValue(_materialIns);
        }
    }

    protected void OnDestroy()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            if (_renerer != null) _renerer.UnregisterDirtyLayoutCallback(renderMatValidate);
        }
#endif
        if(_materialIns != null)
        {
#if UNITY_EDITOR    
            if(!Application.isPlaying)
                DestroyImmediate(_materialIns);
            else
                Destroy(_materialIns);
#else
            Destroy(_materialIns);
#endif        
            _materialIns = null;
        }
    }

#if UNITY_EDITOR

    void initIMatAnimList()
    {
        IUIMatAnimBase[] comps = GetComponents<IUIMatAnimBase>();
        if(comps == null)
            return;

        if(_renerer == null)
            return;
        
        Material mat;
        if (_material == null)
        {
            mat = Canvas.GetDefaultCanvasMaterial();
        }
        else
            mat =_material;
        
        _animList.Clear();
        foreach (var comp in comps)
        {
            if(comp.init(mat))
                _animList.Add(comp);
        }
    }
    
    void renderMatValidate()
    {
        if(_materialIns == null)
            return;
        if (_renerer.material != _materialIns)
        {
            _material = _renerer.material;
            init();
        }
    }
    private void OnValidate()
    {
        if(Application.isPlaying)
            return;
        
        _renerer = GetComponent<Graphic>();
        init();
    }
#endif
    public void refreshAnimList()
    {
#if UNITY_EDITOR
        init();
#endif
    }
}
}