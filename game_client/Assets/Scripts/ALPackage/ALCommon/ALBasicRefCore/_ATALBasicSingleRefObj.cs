using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/********************
 * 单个Ref资源对象的管理对象
 **/
public abstract class _ATALBasicSingleRefObj<T> where T : UnityEngine.Object
{
    /** 是否初始化的状态变量 */
    private bool _m_bInited;
    /** 初始化完成后的回调 */
    private Action _m_dSucDelegate;
    /** 初始化失败的回调 */
    private Action _m_dFailDelegate;

    /** 对象 */
    private T _m_tObj;

    protected _ATALBasicSingleRefObj()
    {
        _m_bInited = false;
        _m_dSucDelegate = null;
        _m_dFailDelegate = null;

        _m_tObj = default(T);
    }

    public T obj { get { return _m_tObj; } }

    /*****************
     * 初始化操作
     **/
    public void init(Action _sucDelegate)
    {
        init(_sucDelegate, _sucDelegate);
    }
    public void init(Action _sucDelegate, Action _failDelegate)
    {
        if (_m_bInited)
        {
            if(null != _sucDelegate)
                _sucDelegate();
            return;
        }

        _m_bInited = true;
        _m_dSucDelegate = _sucDelegate;
        _m_dFailDelegate = _failDelegate;

        //加载3D场景信息
        //开始加载
#if UNITY_EDITOR
        ALLocalResLoaderMgr.instance.loadRefdataGameObjectAsset(_resPath, _objName, _onObjLoaded, null, _onLocalObjLoaded, _resCore);
#else
        _resCore.loadAsset(_resPath, _onObjLoaded, null);
#endif
    }

    /*******************
     * 场景信息加载完后的处理函数
     **/
    public void _onObjLoaded(bool _isSuc, ALAssetBundleObj _assetObj)
    {
        //判断是否成功
        if (!_isSuc || null == _assetObj)
        {
            Debug.LogError($"Load {typeof(T)} Path {_resPath} Error!: {_isSuc}");
            _onFail();
            return;
        }
        if(null == _assetObj.assetBundle)
        {
            Debug.LogError($"_assetObj.assetBundle is null: {_assetObj.idx}");
            _onFail();
            return;
        }

        //加载对象
        _m_tObj = _assetObj.load(_objName) as T;
        if (null == _m_tObj)
        {
            Debug.LogError($"{_assetObj.idx} cannot find {_objName}");
            _onFail();
            return;
        }

        //调用完成操作
        _onSuc();
    }
#if UNITY_EDITOR
    public void _onLocalObjLoaded(UnityEngine.Object _obj)
    {
        //判断是否成功
        if(null == _obj)
        {
            Debug.LogError("Load " + typeof(T) + " Error!");
            _onFail();
            return;
        }

        //加载对象
        _m_tObj = _obj as T;
        if(null == _m_tObj)
        {
            Debug.LogError("Load " + typeof(T) + " Error!");
            _onFail();
            return;
        }

        //调用完成操作
        _onSuc();
    }
#endif

    /*********************
     * 初始化完成后的函数
     **/
    protected void _onSuc()
    {
        _m_dFailDelegate = null;
        //判断回调是否需要调用
        if (null != _m_dSucDelegate)
        {
            Action tempSucDelegate = _m_dSucDelegate;
            _m_dSucDelegate = null;
            tempSucDelegate();
        }
    }

    /*********************
     * 初始化失败的函数
     **/
    protected void _onFail()
    {
        _m_dSucDelegate = null;
        //判断回调是否需要调用
        if (null != _m_dFailDelegate)
        {
            Action tempFailDelegate = _m_dFailDelegate;
            _m_dFailDelegate = null;
            tempFailDelegate();
        }
    }

    //释放asset
    protected void _discardAsset()
    {
        _m_tObj = null;
    }
    
    /** 获取资源管理对象 */
    protected abstract _AALResourceCore _resCore { get; }
    /** 获取加载路径 */
    protected abstract string _resPath { get; }
    /** 获取加载对象名 */
    protected abstract string _objName { get; }
}
