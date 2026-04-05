using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/*****************
 * 使用资源时的资源容器管理对象，用于管理已经加载进来的资源信息
 **/
public class ALResObjListContainer : _AALResObjContainer
{
    /** 资源对象的存储队列 */
    private List<_IALObjResObjInterface> _m_lResObjList;

    public ALResObjListContainer()
    {
        _m_lResObjList = new List<_IALObjResObjInterface>(1);
    }

    /*************
     * 添加一个使用的资源对象，到本管理对象中
     **/
    protected override internal void _addReObj(_IALObjResObjInterface _resObj)
    {
        if (null == _resObj)
            return;

        _m_lResObjList.Add(_resObj);
    }

    /****************
     * 从本管理对象中移除一个已经使用的资源对象
     **/
    protected override internal void _removeResObj(_IALObjResObjInterface _resObj)
    {
        if (null == _resObj)
            return;

        _m_lResObjList.Remove(_resObj);
    }

    /********************
     * 释放本容器对象以及相关资源
     **/
    public override void discard()
    {
        List<_IALObjResObjInterface> allObj = new List<_IALObjResObjInterface>();
        allObj.AddRange(_m_lResObjList);

        //清空队列
        _m_lResObjList.Clear();

        for (int i = 0; i < allObj.Count; i++)
        {
            _IALObjResObjInterface resObj = allObj[i];
            if (null == resObj)
                continue;

            resObj.discard();
        }
        allObj.Clear();
    }
}
