using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using System;
using GOE;


[System.Serializable]
public class NPSimpleTutorialRefObj: _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;
    
    //终点, 根据玩家当前所在节点和引导目标节点（终点），获取到对应的引导数据（在引导切换表）
    public string end_node;
    //简易指引的窗口资源加载路径id
    public long tutorial_asset_id;
    //引导的前置条件
    [ALAutoExportVariableAttr(true, false)]
    public _NPPlayerConditionSerializeInfo pre_condition;

    //是否需要走边
    public bool need_edge;
}

public class NPGSOSimpleTutorialRefSet : _TALSOBasicRefSet<NPSimpleTutorialRefObj>
{
    public static string assetPath { get { return NPABString.C_RefdataPath; } }
    public static string objName { get { return "simple_tutorial"; } }
}

