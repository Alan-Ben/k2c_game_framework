using System.Collections.Generic;
using ALPackage;
using NPEnum;

/// <summary>
/// 红点
/// </summary>
[System.Serializable]
public class NPRedTipRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;
    public List<long> sub_id_list;// 直系子节点id列表
    public ENPFunctionType function_type;//系统功能类型
}

public class NPGSORedTipRefSet : _TALSOBasicRefSet<NPRedTipRefObj>
{
    /************
 * 资源加载路径
 **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "red"; } }
}