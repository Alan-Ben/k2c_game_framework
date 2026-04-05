using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/*******************
 * 已经加载的资源对象信息
 **/
public abstract class _ATALLoadedObjInfo<T> where T : UnityEngine.Object
{
    /** 索引标记 */
    protected int _m_iMainId;
    protected int _m_iSubId;

    /** 本资源是否已经初始化完毕 */
    protected bool _m_bInit;
    /** 具体加载的资源对象 */
    protected T _m_oObj;
    /** 加载本资源的回调函数，由于带入的资源对象需要逐个生成，因此使用队列进行管理 */
    protected List<Action<_ATALObjResObj<T>>> _m_lDelegateList;

    /** 本资源被引用的次数，当卸载对应资源的时候，引用次数递减，当次数到达0的时候才需要记录到需要被释放的统计中 */
    protected int _m_iUseCount;

    public _ATALLoadedObjInfo(ALBasicResIndexInfo _indexInfo)
    {
        _m_iMainId = _indexInfo.mainId;
        _m_iSubId = _indexInfo.subId;

        _m_bInit = false;
        _m_oObj = null;
        _m_lDelegateList = new List<Action<_ATALObjResObj<T>>>(1);

        _m_iUseCount = 0;
    }
    public _ATALLoadedObjInfo(int _mainId, int _subId)
    {
        _m_iMainId = _mainId;
        _m_iSubId = _subId;

        _m_bInit = false;
        _m_oObj = null;
        _m_lDelegateList = new List<Action<_ATALObjResObj<T>>>(1);

        _m_iUseCount = 0;
    }

    public bool inited { get { return _m_bInit; } }
    public int mainId { get { return _m_iMainId; } }
    public int subId { get { return _m_iSubId; } }
    public T obj { get { return _m_oObj; } }
    public int useCount { get { return _m_iUseCount; } }

    /// <summary>
    /// 是否使用同步方式调用资源加载处理
    /// </summary>
    public virtual bool loadAssetUseSyn { get { return true; } }

    /*********************
     * 注册回调函数
     **/
    public void regDelegate(Action<_ATALObjResObj<T>> _delegate)
    {
        if (null == _delegate)
            return;

        //判断是否已经初始化完成，是则直接调用回调
        if (_m_bInit)
        {
            _delegate(_createResObj());
            return;
        }

        //注册到回调队列中
        _m_lDelegateList.Add(_delegate);
    }

    /******************
     * 从数据资源中加载对象
     **/
    public void loadRes()
    {
        if (_m_bInit)
            return;

        //开始进行加载
#if UNITY_EDITOR
        if(loadAssetUseSyn)
            ALLocalResLoaderMgr.instance.loadSynTemplateObjectAsset<T>(_getAssetPath(_m_iMainId, _m_iSubId), _getObjName(_m_iMainId, _m_iSubId)
            , _localResExName, _localResUnitySiftStr, _resLoadedDelegate, null, _resLoadedDelegate, _getALResourceCore());
        else
            ALLocalResLoaderMgr.instance.loadTemplateObjectAsset<T>(_getAssetPath(_m_iMainId, _m_iSubId), _getObjName(_m_iMainId, _m_iSubId)
                , _localResExName, _localResUnitySiftStr, _resLoadedDelegate, null, _resLoadedDelegate, _getALResourceCore());
#else
        if(loadAssetUseSyn)
            _getALResourceCore().loadSynAsset(_getAssetPath(_m_iMainId, _m_iSubId), _resLoadedDelegate, null);
        else
            _getALResourceCore().loadAsset(_getAssetPath(_m_iMainId, _m_iSubId), _resLoadedDelegate, null);
#endif
    }
    //资源加载完成的回调对象
    protected void _resLoadedDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
    {
        if(!_isSuc || null == _assetObj)
        {
#if UNITY_EDITOR
            Debug.LogError("Load " + typeof(T) + " Fail! Index: " + _m_iMainId + " - " + _m_iSubId);
#endif
            //失败则设置为null;
            initGo(null);
            return;
        }

        //加载对象
        T resGo = _assetObj.load<T>(_getObjName(_m_iMainId, _m_iSubId));
        _resLoadedDelegate(resGo);
    }
    protected void _resLoadedDelegate(T _obj)
    {
        if(null == _obj)
        {
#if UNITY_EDITOR
            Debug.LogError("Load " + typeof(T) + " Fail! Object Type not fix! obj Name: " + _getObjName(_m_iMainId, _m_iSubId));
#endif
        }

        //设置对应数据
        initGo(_obj);
    }

    /*****************
     * 初始化资源对象
     **/
    public void initGo(T _obj)
    {
        //设置加载完毕
        _m_bInit = true;
        //设置资源对象
        _m_oObj = _obj;

        //此时默认增加一次引用，避免释放资源
        _addUseCount();

        //调用init事件函数
        _onInitObj(_obj);

        //调用回调
        for (int i = 0; i < _m_lDelegateList.Count; i++)
        {
            Action<_ATALObjResObj<T>> action = _m_lDelegateList[i];
            if (null == action)
                continue;

            //调用回调
            action(_createResObj());
        }
        //清空队列
        _m_lDelegateList.Clear();

        //减少引用次数，并判断是否需要释放资源
        _reduceUseCount();
    }

    /****************
     * 释放资源的函数
     **/
    public void discard()
    {
        //调用子类的特殊处理函数
        _onDiscard();
        //设置变量为空
        _m_oObj = null;
    }

    /**************
     * 单纯的增加使用索引
     **/
    protected internal void _addUseCount()
    {
        //增加引用次数
        _m_iUseCount++;
    }

    /**************
     * 减少引用次数，并判断是否需要释放相关资源
     **/
    protected internal void _reduceUseCount()
    {
        //减少引用次数
        _m_iUseCount--;

        //根据当前引用次数判断是否需要释放资源
        if (_m_iUseCount <= 0 && _getObjCore().isAutoRelease)
            _getObjCore().discardRes(_m_iMainId, _m_iSubId, this);
    }
    
    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    protected abstract string _getAssetPath(int _mainId, int _subId);
    protected abstract string _getObjName(int _mainId, int _subId);
#if UNITY_EDITOR
    protected abstract string _localResExName { get; }
    protected abstract string _localResUnitySiftStr { get; }
#endif
    /*****************
     * 获取资源加载对象
     **/
    protected abstract _AALResourceCore _getALResourceCore();
    /*****************
     * 释放本对象拷贝出去的资源对象
     **/
    protected abstract _ATALObjCore<T> _getObjCore();
    /***************
     * 创建资源对象
     **/
    protected abstract _ATALObjResObj<T> _createResObj();
    /// <summary>
    /// 在使用资源的时候用于创建拷贝资源时调用的函数，如果类似图片类的ab资源，不需要进行instance操作
    /// </summary>
    /// <returns></returns>
    protected internal abstract T _cloneObj();
    /// <summary>
    /// 释放_cloneObj函数创建的资源，如没有实例化则不用处理
    /// </summary>
    /// <param name="_obj"></param>
    protected internal abstract void _releaseCloneObj(T _obj);
    /*****************
     * 在加载对象的时候调用的事件函数
     **/
    protected abstract void _onInitObj(T _obj);
    /// <summary>
    /// 释放LoadedObj数据对象中本身加载的资源的函数
    /// </summary>
    protected abstract void _onDiscard();
}
