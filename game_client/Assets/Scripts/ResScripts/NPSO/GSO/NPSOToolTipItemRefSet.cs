using ALPackage;
using System;
using System.Collections.Generic;
using System.Text;
using NPEnum;
using UnityEngine;

/// <summary>
/// 图标工具提示类
/// </summary>
[Serializable]
public class NPToolTipItemRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一识别标记
    
    /// <summary>
    /// /生成表唯一Id
    /// </summary>
    /// <param name="_mainId">这个应该是点击的图标的主Id</param>
    /// <param name="_subId">这个应该是点击的图标的子Id</param>
    /// <returns></returns>
    public static long generateId(long _mainId, long _subId)
    {
        return _mainId * 1000000 + _subId;
    }
}

public class NPSOToolTipItemRefSet : _TALSOBasicRefSet<NPToolTipItemRefObj> {

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "tooltip_item"; } }
}
