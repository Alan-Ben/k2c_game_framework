using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;

[System.Serializable]
public class NPSfxRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id

    public long duration;  //持续时间  毫秒
    public long trigger_event_time;//延迟触发下一个事件的时间（毫秒）不配或者<0则一开始就触发

    public NPGSfxIndex sfx_index;//技能特效资源索引

    public List<long> audio_id;//特效音效ID

    public int min_cache_count;//最小缓存数
    public int max_cache_count;//最大缓存数

    //关联的3d信息
    [System.NonSerialized]
    private NPSfx3DRefObj _m_sfx3dRef;
    public NPSfx3DRefObj sfx3DRef { get { return _m_sfx3dRef; } }
    public void init3DRef(NPSfx3DRefObj _ref)
    {
        _m_sfx3dRef = _ref;
    }
}

public class NPSOSfxRefSet : _TALSOBasicRefSet<NPSfxRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "sfx"; } }
}
