using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子故事背景表
/// </summary>
[System.Serializable]
public class ConsortStoryBgRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public long story_id;//故事id
    public List<NPGGoIndex> bg_go_index_list;//邀约背景go列表
    public _NPPlayerConditionSerializeInfo unlock_condition;//生效条件
}

public class GSOConsortStoryBgRefSet : _TALSOBasicRefSet<ConsortStoryBgRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_story_bg"; } }
}