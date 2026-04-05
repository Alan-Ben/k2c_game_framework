using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效控制脚本，用于手动控制播放停止等功能
/// </summary>
public class SfxMonoParticleSystemList : MonoBehaviour
{
    [ALHeader("需要手动控制播放停止等功能的特效列表")]
    public List<ParticleSystem> particleSystemList;
}
