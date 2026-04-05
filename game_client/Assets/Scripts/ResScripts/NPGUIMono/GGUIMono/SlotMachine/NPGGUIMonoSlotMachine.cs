using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

/// <summary>
/// 老虎机mono
/// </summary>
///
public class NPGGUIMonoSlotMachine : _ANPBasicUIWndResBarMono
{
    [ALHeader("滚动数字")]
    public NPGGUIMonoSlotMachineNumList numList;

    [ALHeader("开始滚动按钮")]
    public GameObject startBtn;

    [ALHeader("跳过滚动动画按钮")]
    public GameObject skipScrollBtn;

    [ALHeader("跳过金币动画按钮")]
    public GameObject skipGoldBtn;

    [ALHeader("开始滚动后显示的Go List")]
    public List<GameObject> startShowGoList;

    [ALHeader("开始滚动后隐藏的Go List")]
    public List<GameObject> startHideGoList;

    [ALHeader("粒子飞行动画的起点")]
    public RectTransform particleStartTrans;

    [ALHeader("免费时显示的Go List")]
    public List<GameObject> freeShowGoList;

    [ALHeader("免费时隐藏的Go List")]
    public List<GameObject> freeHideGoList;

    [ALHeader("单次消耗")]
    public NPGGUIMonoCommonItem costItemMono;

    [ALHeader("抽奖次数文本")]
    public Text costTimeTxt;

    [ALHeader("点击抽奖需要播放的动画")]
    public Animation startScrollAnimation;

    [ALHeader("结束时播放的转盘动画")]
    public Animation scrollAnimation;

    [ALHeader("结束时播放的金币掉落动画")]
    public Animation goldAnimation;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(4300); } }
    public static string objName { get { return UIResPathAssistant.getObjName(4300); } }
}
