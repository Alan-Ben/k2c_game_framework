using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;
using CommonEnum;

/// <summary>
/// 关卡界面鼓舞子窗口
/// </summary>
///
public class GGUIMonoChapterMain_SubWndInspire : _AALBasicUIWndMono
{
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("我方战力")]
    public Text txtSelfPower;
    [ALHeader("战力足够显示的颜色")]
    public Color colorNormalPower;
    [ALHeader("战力不足够显示的颜色")]
    public Color colorUnEnoughPower;
    [ALHeader("我方战力鼓舞加成值")]
    public Text txtSelfPowerInspire;
    
    [ALHeader("boos头像")]
    public RawImage imgBossIcon;
    [ALHeader("boss战力")]
    public Text txtBossPower;
    [ALHeader("boss名字")]
    public Text txtBossName;
  
    [ALHeader("金币鼓舞")]
    public GameObject btnInspireGold;
    [ALHeader("金币鼓舞消耗")]
    public NPGGUIMonoCommonItem costItemInspireGold;
    [ALHeader("金币鼓舞次数")]
    public Text goldInspireCount;
    [ALHeader("道具鼓舞")]
    public GameObject btnInspireItem;
    [ALHeader("道具鼓舞消耗")]
    public NPGGUIMonoCommonItem costItemInspireItem;
    [ALHeader("道具鼓舞次数")]
    public Text itemInspireCount;
    
    [ALHeader("开战按钮")]
    public GameObject btnStartBattle;
    
    [ALHeader("战力超过boss显示的go列表")]
    public List<GameObject> goPassBossShow;
    [ALHeader("战力超过boss隐藏的go列表")]
    public List<GameObject> goPassBossHide;
    
    [ALHeader("鼓舞特效父节点")]
    public Transform sfxParent;
    [ALHeader("金币鼓舞特效id")]
    public long sfxGoldInspireId;
    [ALHeader("道具鼓舞特效id")]
    public long sfxItemInspireId;
    
    [ALHeader("有鼓舞时候展示的go列表")]
    public List<GameObject> goInspireShow;
    
    [ALHeader("提升战力按钮")]
    public GameObject btnPowerUp;
}