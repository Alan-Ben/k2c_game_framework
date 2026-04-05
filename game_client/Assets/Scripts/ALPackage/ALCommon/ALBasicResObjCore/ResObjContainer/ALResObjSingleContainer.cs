using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/*****************
 * 使用资源时的资源容器管理对象，用于管理已经加载进来的资源信息
 **/
public class ALResObjSingleContainer : _AALResObjContainer
{
    /** 资源对象的存储队列 */
    private _IALObjResObjInterface _m_lResObj;

    public ALResObjSingleContainer()
    {
        _m_lResObj = null;
    }

    /*************
     * 添加一个使用的资源对象，到本管理对象中
     **/
    protected override internal void _addReObj(_IALObjResObjInterface _resObj)
    {
        //清除当前资源
        _clearSingleObj();

        if (null == _resObj)
            return;

        _m_lResObj = _resObj;
    }

    /****************
     * 从本管理对象中移除一个已经使用的资源对象
     **/
    protected override internal void _removeResObj(_IALObjResObjInterface _resObj)
    {
        if (_m_lResObj != _resObj)
            return;

        _clearSingleObj();
    }

    /********************
     * 释放本容器对象以及相关资源
     **/
    public override void discard()
    {
        _clearSingleObj();
    }

    /***************
     * 清除单个资源对象
     **/
    protected void _clearSingleObj()
    {
        if (null == _m_lResObj)
            return;

        _m_lResObj.discard();
        _m_lResObj = null;
    }
}
