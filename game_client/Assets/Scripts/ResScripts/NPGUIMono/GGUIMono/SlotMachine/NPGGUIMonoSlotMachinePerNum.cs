using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;



[System.Serializable]
public class NPGGUISlotMachineNumMono
{
    [ALHeader("数字")]
    public int num;

    [ALHeader("对应Go")]
    public GameObject go;
}

/// <summary>
/// 老虎机单个数字mono
/// </summary>
///
public class NPGGUIMonoSlotMachinePerNum : _TALUGUIMonoGridItem
{
    [ALHeader("数字预制体集合")]
    public List<NPGGUISlotMachineNumMono> numList;

    [ALHeader("滚动的content")]
    public GameObject contentGo;

    [ALHeader("单个预制体的高度 + space的大小")]
    public int perNumHeight;

    [ALHeader("每帧最大滚动距离")]
    public int maxS = 100;

    [ALHeader("达到最大滚动速度需要的时间")]
    public int toMaxVTime = 6;

    [ALHeader("最大滚动速度持续时间")]
    public int maxVHoldTime = 3;

    [ALHeader("减速到停止持续的时间")]
    public int maxVToStopTime = 3;

}
