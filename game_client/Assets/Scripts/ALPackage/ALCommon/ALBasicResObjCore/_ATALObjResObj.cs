using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage; 

/*********************
 * 游戏中的game object 索引信息加载出来的资源对象
 **/
public abstract class _ATALObjResObj<T> : _IALObjResObjInterface where T : UnityEngine.Object
{
    /** 加载的资源对象 */
    protected _ATALLoadedObjInfo<T> _m_liLoadedInfo;

    /** 本资源是否初始化 */
    protected bool _m_bInit;
    /** 创建出来的资源归属的资源管理对象 */
    protected _AALResObjContainer _m_rcResContainer;
    /** 资源对象 */
    protected T _m_oObj;

    protected internal _ATALObjResObj(_ATALLoadedObjInfo<T> _loadedInfo)
    {
        _m_liLoadedInfo = _loadedInfo;

        _m_bInit = false;
        _m_rcResContainer = null;
        _m_oObj = null;
    }

    public int mainId { get { return _m_liLoadedInfo.mainId; } }
    public int subId { get { return _m_liLoadedInfo.subId; } }
    public _ATALLoadedObjInfo<T> loadedInfo { get { return _m_liLoadedInfo; } }
    public T obj
    {
        get {
            if (!_m_bInit) {
                Debug.LogError("资源需要先进行create操作, (createObj(_AALResObjContainer _container)).");
            }

            return _m_oObj;
        }
    }

    /*****************
     * 创建对应的资源对象
     **/
    public T createObj(_AALResObjContainer _container)
    {
        if (_m_bInit)
        {
            Debug.LogError("重复调用createObj(_AALResObjContainer _container)!");
            return _m_oObj;
        }

        _m_bInit = true;
        _m_rcResContainer = _container;
        _m_oObj = _m_liLoadedInfo._cloneObj();
        //增加使用次数
        _m_liLoadedInfo._addUseCount();

        //增加引用关系
        if (null != _m_rcResContainer)
            _m_rcResContainer._addReObj(this);
        else
        {
            Debug.LogError($"创建资源对象的容器对象_AALResObjContainer为空!mainId:{mainId};subId:{subId}");
        }

        return _m_oObj;
    }

    /******************
     * 释放资源
     **/
    public void discard()
    {
        //未初始化则不处理
        if (!_m_bInit)
            return;
        
        _m_bInit = false;
        //释放资源，不论go对象是否有效都可带入，在函数中还有对引用次数的具体操作
        _m_liLoadedInfo._releaseCloneObj(_m_oObj);
        //减少使用次数
        _m_liLoadedInfo._reduceUseCount();

        //判断资源管理对象是否有效
        if (null != _m_rcResContainer)
            _m_rcResContainer._removeResObj(this);
        _m_rcResContainer = null;
    }
}
