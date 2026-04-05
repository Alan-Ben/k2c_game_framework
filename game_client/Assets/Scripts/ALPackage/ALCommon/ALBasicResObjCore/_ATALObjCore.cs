using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/**********************
 * 游戏中的资源加载与管理对象，与ALResourceCore一并使用可对资源进行方便的统一管理
 **/
public abstract class _ATALObjCore<T> where T : UnityEngine.Object
{
    /** 是否计数为0自动释放 */
    protected bool _m_bIsAutoRelease;
    /** 资源与索引id的映射表 */
    protected Dictionary<long, System.Object> _m_dLoadedObjInfoDic;

    protected _ATALObjCore()
    {
        _m_bIsAutoRelease = true;
        _m_dLoadedObjInfoDic = new Dictionary<long, System.Object>();
    }
    protected _ATALObjCore(bool _autoRelease = true)
    {
        _m_bIsAutoRelease = _autoRelease;
        _m_dLoadedObjInfoDic = new Dictionary<long, System.Object>();
    }

    public bool isAutoRelease { get { return _m_bIsAutoRelease; } }

    /******************
     * 根据对应的索引加载对应的资源对象，并在回调中对具体资源进行处理
     **/
    public void loadObj(ALBasicResIndexInfo _indexInfo, Action<_ATALObjResObj<T>> _delegate)
    {
        if (null == _indexInfo)
            return;

        loadObj(_indexInfo.mainId, _indexInfo.subId, _delegate);
    }
    public void loadObj(int _mainId, int _subId, Action<_ATALObjResObj<T>> _delegate)
    {
        //合并id，作为数据索引
        long index = ALCommon.mergeInt(_mainId, _subId);
        //检索是否有对应的数据，有则放入回调
        if (_m_dLoadedObjInfoDic.ContainsKey(index))
        {
            _ATALLoadedObjInfo<T> loadedInfo = (_ATALLoadedObjInfo<T>)_m_dLoadedObjInfoDic[index];
            loadedInfo.regDelegate(_delegate);
        }
        else
        {
            //无对应数据则创建对应数据并开始加载
            _ATALLoadedObjInfo<T> loadedInfo = _createLoadedObjInfo(_mainId, _subId);
            //放入数据集合
            _m_dLoadedObjInfoDic.Add(index, loadedInfo);
            //注册回调
            loadedInfo.regDelegate(_delegate);

            //开启加载
            loadedInfo.loadRes();
        }
    }

    /******************
     * 根据对应的索引增加和减少对应资源的使用次数
     **/
    public void addUseCount(ALBasicResIndexInfo _indexInfo)
    {
        if (null == _indexInfo)
            return;

        addUseCount(_indexInfo.mainId, _indexInfo.subId);
    }
    public void addUseCount(int _mainId, int _subId)
    {
        //合并id，作为数据索引
        long index = ALCommon.mergeInt(_mainId, _subId);
        //检索是否有对应的数据，有则放入回调
        if (!_m_dLoadedObjInfoDic.ContainsKey(index))
            return;

        _ATALLoadedObjInfo<T> loadedInfo = (_ATALLoadedObjInfo<T>)_m_dLoadedObjInfoDic[index];
        loadedInfo._addUseCount();
    }
    public void reduceUseCount(ALBasicResIndexInfo _indexInfo)
    {
        if (null == _indexInfo)
            return;

        reduceUseCount(_indexInfo.mainId, _indexInfo.subId);
    }
    public void reduceUseCount(int _mainId, int _subId)
    {
        //合并id，作为数据索引
        long index = ALCommon.mergeInt(_mainId, _subId);
        //检索是否有对应的数据，有则放入回调
        if (!_m_dLoadedObjInfoDic.ContainsKey(index))
            return;

        _ATALLoadedObjInfo<T> loadedInfo = (_ATALLoadedObjInfo<T>)_m_dLoadedObjInfoDic[index];
        loadedInfo._reduceUseCount();
    }

    /******************
     * 强制释放对应资源信息的操作，带入对应资源对象是为了验证是否删除的资源是正确的资源
     **/
    public void discardRes(ALBasicResIndexInfo _indexInfo, _ATALLoadedObjInfo<T> _info)
    {
        if (null == _indexInfo)
            return;

        discardRes(_indexInfo.mainId, _indexInfo.subId, _info);
    }
    public void discardRes(int _mainId, int _subId, _ATALLoadedObjInfo<T> _info)
    {
        //合并id，作为数据索引
        long index = ALCommon.mergeInt(_mainId, _subId);
        //判断数据是否存在
        if (!_m_dLoadedObjInfoDic.ContainsKey(index))
            return;

        //处理释放操作
        _ATALLoadedObjInfo<T> loadedInfo = (_ATALLoadedObjInfo<T>)_m_dLoadedObjInfoDic[index];
        //检查当前资源是否处理释放操作的资源
        if (null != _info && loadedInfo != _info)
            return;

        //删除映射表
        _m_dLoadedObjInfoDic.Remove(index);
        //释放资源
        loadedInfo.discard();
    }

    /// <summary>
    /// 尝试遍历所有资源，将无引用资源释放
    /// </summary>
    public void tryDiscardAllDisableRes()
    {
        List<_ATALLoadedObjInfo<T>> removeList = new List<_ATALLoadedObjInfo<T>>();
        foreach(_ATALLoadedObjInfo<T> objInfo in _m_dLoadedObjInfoDic.Values)
        {
            if(null == objInfo)
                continue;

            //先添加到队列
            if(objInfo.useCount <= 0)
                removeList.Add(objInfo);
        }

        //逐个删除
        _ATALLoadedObjInfo<T> tmpInfo = null;
        for(int i = 0; i < removeList.Count; i++)
        {
            tmpInfo = removeList[i];
            if(null == tmpInfo)
                continue;

            //移除数据
            discardRes(tmpInfo.mainId, tmpInfo.subId, tmpInfo);
        }
    }

    /***************
     * 创建一个载入资源信息的对象
     **/
    protected abstract _ATALLoadedObjInfo<T> _createLoadedObjInfo(int _mainId, int _subId);
}
