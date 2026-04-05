using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/*****************
 * 使用资源时的资源容器管理对象，用于管理已经加载进来的资源信息
 **/
public abstract class _AALResObjContainer
{
    /*************
     * 添加一个使用的资源对象，到本管理对象中
     **/
    protected abstract internal void _addReObj(_IALObjResObjInterface _resObj);

    /****************
     * 从本管理对象中移除一个已经使用的资源对象
     **/
    protected abstract internal void _removeResObj(_IALObjResObjInterface _resObj);

    /********************
     * 释放本容器对象以及相关资源
     **/
    public abstract void discard();
}
