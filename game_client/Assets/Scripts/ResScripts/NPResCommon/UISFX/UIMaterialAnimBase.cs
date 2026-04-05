using System;
using System.Collections;
using System.Collections.Generic;
using MG;
using UnityEngine;
using UnityEngine.UI;

namespace MG
{
public enum TinyAnimationWrapMode
{
    Once = 1,

    /// <summary>
    ///   <para>When time reaches the end of the animation clip, time will continue at the beginning.</para>
    /// </summary>
    Loop = 2,

    /// <summary>
    ///   <para>When time reaches the end of the animation clip, time will ping pong back between beginning and end.</para>
    /// </summary>
    PingPong = 4,
}
[Serializable]
public abstract class ITinyAnimKey
{
    static Dictionary<string , int> _m_StaticPropertyIds = new Dictionary<string, int>();
    
    [SerializeField]public string _propertyName = "_Color";
    protected int _m_propertyId;
    protected bool _m_hasProperty = false;

    public bool checkPropertyValid(Material _mat)
    {
        if(string.IsNullOrEmpty(_propertyName))
        {
            _m_hasProperty = false;
            UnityEngine.Debug.LogError("Property 名字为空 需要检查");
            return false;
        }
        if(_mat == null)
        {
            _m_hasProperty = false;
            UnityEngine.Debug.LogError($"material 为空");
            return false;
        }
        if(!_m_StaticPropertyIds.TryGetValue(_propertyName, out _m_propertyId))
        {
            _m_propertyId = Shader.PropertyToID(_propertyName);
            _m_StaticPropertyIds.Add(_propertyName, _m_propertyId);
        }
            
        _m_hasProperty = _mat.HasProperty(_m_propertyId);
        if(_m_hasProperty)
        {
            return true;
        }
        UnityEngine.Debug.LogError($"材质{_mat}不存在 Property:{_propertyName}");
        return false;
    }

    public void setValue(Material _mat, float _duration)
    {
        if(!_m_hasProperty)
            return;
        onValueChange(_mat, _duration);
    }
    public abstract void onValueChange(Material _mat, float _duration);
}


public class UIMaterialAnimBase : MonoBehaviour
{
    public Graphic _renerer;
    public Material _material;
    protected Material _materialIns;

    public TinyAnimationWrapMode _wrapMode = TinyAnimationWrapMode.Loop;
    public float _timeLength = 1;
    private float _m_curTime;
    protected float _m_curDuration;
    private bool _m_isStop = false;
    
    protected void Awake()
    {
        if(_material != null)
        {
            if(checkValid(_material))
                _materialIns = Instantiate(_material);
        }
        else
        {
            _material = Canvas.GetDefaultCanvasMaterial();
            if(checkValid(_material))
                _materialIns = Instantiate(_material);
        }
        
    }

    private void OnEnable()
    {
        _m_curTime = 0;
        _m_isStop = false;
        if(_materialIns != null && _renerer != null)
        {
            _renerer.material = _materialIns;
        }
        else
        {
            _m_isStop = true;
        }
        if(_timeLength.Equals(0))
        {
            _m_isStop = true;
        }
       
    }

    private void OnDisable()
    {
        _m_curTime = 0;
    }

    protected void OnDestroy()
    {
        if(_materialIns != null)
        {
            Destroy(_materialIns);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(_m_isStop)
            return;

        _m_curTime += Time.deltaTime;
        switch (_wrapMode)
        {
            case TinyAnimationWrapMode.Loop:
                _m_curDuration =(_m_curTime/_timeLength )% 1f;
                break;
            case TinyAnimationWrapMode.Once:
                if(_timeLength < _m_curTime)
                {
                    _m_isStop = true;
                }
                _m_curDuration =(_m_curTime/_timeLength )% 1f;
                break;
            case TinyAnimationWrapMode.PingPong:
                _m_curDuration =Mathf.Abs((_m_curTime/_timeLength +1) % 2f - 1);
                break;
        }
        setValue(_materialIns, _m_curDuration);
    }
    protected virtual bool checkValid(Material _mat)
    {
        return false;
    }

    protected virtual void setValue(Material _mat, float _duration)
    {
        
    }
}
}