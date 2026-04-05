using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 拖尾效果的净化脚本对象
/// </summary>
public class NPSfxComponentTrainRenderMono : MonoBehaviour
{
    public List<TrailRenderer> trails;

    // Use this for initialization
    void OnEnable()
    {
        for (int i = 0; i < trails.Count; i++)
        {
            trails[i].Clear();
        }
    }
}
