
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using MG;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{
    /// <summary>
    /// 动画控制材质的参数
    /// </summary>
[ExecuteAlways]
public class MatAnimController : MonoBehaviour, _IUIMatAnimControllerBase
{
    static Dictionary<string , int> _m_StaticPropertyIds = new Dictionary<string, int>();
    
    static List<int> StaticMatAnimUpdate = new List<int>();

    static bool regMatUpdate(Material _material)
    {
        if(StaticMatAnimUpdate == null)
            StaticMatAnimUpdate = new List<int>();
        if(_material == null)
            return false;
        int instanceId = _material.GetInstanceID();
        if(StaticMatAnimUpdate.Contains(instanceId))
            return false;
        StaticMatAnimUpdate.Add(instanceId);
        return true;
    }

    static void unregMatUpdate(Material _material)
    {
        if(_material == null)
            return;
        int instanceId = _material.GetInstanceID();
        if(StaticMatAnimUpdate != null)
        {
            StaticMatAnimUpdate.Remove(instanceId);
        }
    }
    
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
    public Material _material;
    [NotNull]public List<IUIMatAnimBase> _animList = new List<IUIMatAnimBase>();
    
    [HideInInspector]public bool _m_needAnim = false;
    private bool _m_hasRegAnim = false;

    protected void Awake()
    {
        init();
    }

   
    void init()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
            initIMatAnimList();
#endif      
        _m_needAnim = false;
     
        for (int i = _animList.Count-1; i >=0; i--)
        {
            var anim = _animList[i];
            if(anim == null || !anim.init(_material))
            {
                _animList.Remove(anim);
                continue;
            }
            _m_needAnim = true;
        }
    }

    private void OnEnable()
    {
        if(!_m_needAnim)
            return;
        _m_hasRegAnim = regMatUpdate(_material);
        // OnEnable 的时候更新一次，避免update延迟一帧的bug
        ALCommonActionMonoTask.addLaterMonoTask(_update);
    }

    private void OnDisable()
    {
        if(_m_hasRegAnim)
        {
            _m_hasRegAnim = false;
            unregMatUpdate(_material);
        }
    }

    private void Update()
    {
        _update();
    }

    private void _update()
    {
        if(!_m_needAnim)
            return;
        if(!_m_hasRegAnim)
            return;
        foreach (var ani in _animList)
        {
            ani.setValue(_material);
        }
    }

#if UNITY_EDITOR

    void initIMatAnimList()
    {
        IUIMatAnimBase[] comps = GetComponents<IUIMatAnimBase>();
        if(comps == null)
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
    
    private void OnValidate()
    {
        if(Application.isPlaying)
            return;
        
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