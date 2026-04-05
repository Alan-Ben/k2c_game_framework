using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 分享icon的item
/// </summary>
public class GGUIMonoCommonIconItem : _ANPGGUIMonoSingleChoiceItem
{
    [ALHeader("图片")]
    public RawImage texIcon;
    [ALHeader("内容")]
    public TextEx txtContent;
    [ALHeader("解锁状态")]
    public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> statInfos;
    [ALHeader("跟随节点")]
    public RectTransform followRect;
    [ALHeader("x偏移")]
    public float _intervalX;
    [ALHeader("y偏移")]
    public float _intervalY;
}
