using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/********************
 * 基本的索引信息对象
 **/
[System.Serializable]
public abstract class _AALBasicLoadResIndexInfo : ALBasicResIndexInfo
{
    public _AALBasicLoadResIndexInfo()
    {

    }
    public _AALBasicLoadResIndexInfo(int _mainId, int _subId)
        : base(_mainId, _subId)
    {
    }

    /************
     * 资源加载路径
     **/
    public abstract string assetPath { get; }
    public abstract string objName { get; }
}