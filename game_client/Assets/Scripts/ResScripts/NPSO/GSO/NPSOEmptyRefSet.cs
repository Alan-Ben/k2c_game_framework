using UnityEngine;
using System.Collections;
using ALPackage;

[System.Serializable]
public class NPSOEmptyRefObj : _IALBasicRefObj {
    public long _refId { get { return id; } }
    public long id;
}

public class NPSOEmptyRefSet : _TALSOBasicRefSet<NPSOEmptyRefObj> {

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/empty.unity3d"; } }
    public static string objName { get { return "empty"; } }
}
