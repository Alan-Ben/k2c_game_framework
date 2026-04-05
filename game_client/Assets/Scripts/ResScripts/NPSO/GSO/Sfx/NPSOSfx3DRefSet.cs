using System;
using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using GOE;

[System.Serializable]
public class NPSfx3DRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public bool follow_actor_rotate;//是否跟随单位 旋转
    public bool break_actor_pos;//是否不跟随单位 坐标
    public bool connect_src;//是否与特效来源对象链接

    public bool is_relation_sfx_scale;//特效大小是否和模型大小关联 bool值 默认为FALSE
}

public class NPSOSfx3DRefSet : _TALSOBasicRefSet<NPSfx3DRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "sfx_3d"; } }
}
