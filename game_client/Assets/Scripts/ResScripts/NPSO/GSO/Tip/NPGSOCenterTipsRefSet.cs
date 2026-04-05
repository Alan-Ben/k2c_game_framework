using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

using GOE;

[System.Serializable]
public class NPCenterTipsRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//ID

    public ENPTipShowType show_type;//显示队列类型
    public long res_id;//UI资源路径id
    public bool disable_time_need_rate;//自动回收是否需要受加速控制
    public float disable_time;//自动回收的时间
    public bool space_time_need_rate;//tip间隔否需要受加速控制
    public float space_time;//这个tip到发送下一个tip的间隔
}


public class NPGSOCenterTipsRefSet : _TALSOBasicRefSet<NPCenterTipsRefObj>
{

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return NPABString.C_RefdataPath; } }
    public static string objName { get { return "center_tips"; } }
}
