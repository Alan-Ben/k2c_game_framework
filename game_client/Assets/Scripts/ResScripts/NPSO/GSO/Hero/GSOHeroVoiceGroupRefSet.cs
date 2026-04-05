
using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 大臣配音组表
/// </summary>
[Serializable]
public class HeroVoiceGroupRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public long hero_id;//妃子id
    public EHeroVoiceType voice_type;//配音类型
    public List<long> voice_id_list;//配音id列表
    public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
}

public class GSOHeroVoiceGroupRefSet : _TALSOBasicRefSet<HeroVoiceGroupRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "hero_voice_group"; } }
}