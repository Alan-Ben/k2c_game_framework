
using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子配音组表
/// </summary>
[Serializable]
public class ConsortVoiceGroupRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public long consort_id;//妃子id
    public EConsortVoiceType voice_type;//配音类型
    public List<long> voice_id_list;//配音id列表
    public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
}

public class GSOConsortVoiceGroupRefSet : _TALSOBasicRefSet<ConsortVoiceGroupRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_voice_group"; } }
}