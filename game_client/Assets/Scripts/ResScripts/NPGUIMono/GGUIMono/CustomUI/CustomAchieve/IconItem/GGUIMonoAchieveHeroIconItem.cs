using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

public class GGUIMonoAchieveHeroIconItem : _AALBasicUIWndMono
{
    [ALHeader("头像")] 
    public RawImage texIcon;
    [ALHeader("解锁状态")]
    public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> statInfos;
    [ALHeader("点击按钮")] 
    public GameObject btnClick;
}