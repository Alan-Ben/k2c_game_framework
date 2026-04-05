using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 妃子卡牌item
/// </summary>
public class GGUIMonoConsortCardItem : _ANPGGUIMonoSingleChoiceItem
{
    [ALHeader("妃子名称")]
    public TextEx txtConsortName; 
    [ALHeader("是否根据解锁状态设置名称颜色")]
    public bool needSetNameColor = false;
    [ALHeader("未解锁时名称颜色")]
    public Color lockNameColor = Color.gray;
    [ALHeader("已解锁时名称颜色")]
    public Color unLockNameColor = Color.white;
    [ALHeader("妃子名称背景")]
    public RawImage imgConsortNameBg;
    
    [ALHeader("妃子图片")] 
    public RawImage texConsortIcon;
    [ALHeader("是否根据解锁状态设置形象颜色")]
    public bool needSetIconColor = false;
    [ALHeader("未解锁时形象颜色")]
    public Color lockIconColor = Color.grey;
    [ALHeader("已解锁时形象颜色")]
    public Color unLockIconColor = Color.gray;
    [ALHeader("妃子卡牌背景")]
    public GGUIMonoConsortCardBg consortCardBg;

    [ALHeader("品质展示")]
    public GGUIMonoConsortQualityShow monoConsortQualityShow;
    
    [ALHeader("妃子称号")]
    public TextEx txtConsortTitle;
    
    [ALHeader("亲密度翻译key，放空仅展示数值")]
    public string intimacyTransKey; 
    [ALHeader("亲密度")]
    public TextEx txtIntimacy; 
    [ALHeader("魅力翻译key，放空仅展示数值")]
    public string charmTransKey; 
    [ALHeader("魅力")] 
    public TextEx txtCharm;
    [ALHeader("妃子产出来源")] 
    public TextEx txtConsortSource; 
    
    [ALHeader("解锁状态配置")] 
    public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> statInfos;
}