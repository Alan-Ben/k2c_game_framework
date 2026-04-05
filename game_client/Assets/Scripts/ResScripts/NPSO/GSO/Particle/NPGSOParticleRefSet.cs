using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

using GOE;

[System.Serializable]
public class NPParticleRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//粒子ID

    public NPGParticleIndex particle_index;//资源索引id
    public NPGSpriteIndex icon_index;//图标资源


    public float create_interval;//生成粒子的时间间隔（秒）
    public float single_particle_fly_interval;//炸开后，每个粒子开始飞行的间隔（秒）
    public float max_radius;//粒子炸开的最大半径
    public float min_radius;//粒子炸开的最小半径
    public float first_burst_acc_time_scale;//粒子炸开加速段占比
    public float first_fly_acc_time_scale;//粒子上飘加速段占比
    public float alpha_start_time;//粒子开始改变透明度的时间,大于0生效
    public float alpha_end_value;//粒子到终点的透明度的的值（0到1之间）
    public float burst_time;//粒子炸开总时长（秒）
    public float fly_time;//粒子上飘总时长（秒）
    public long burst_audio_id;//炸开音效id
    public long fly_audio_id;//飞行音效id
    public long each_done_audio_id;//每个粒子完成音效id

}


public class NPGSOParticleRefSet : _TALSOBasicRefSet<NPParticleRefObj>
{

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return NPABString.C_RefdataPath; } }
    public static string objName { get { return "particle"; } }
}
