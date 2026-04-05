using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

//有向边的数据
[System.Serializable]
public class NPTutorialEdgeRef : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//每条有向边的唯一识别ID
    public string start_node;//起点
    public string end_node;//终点
    public long res_path_id;//起点到终点需要执行的引导
}

public class NPGSOTutorialEdgeRefSet : _TALSOBasicRefSet<NPTutorialEdgeRef>
{

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return NPABString.C_RefdataPath; } }
    public static string objName { get { return "tutorial_edge"; } }
}
