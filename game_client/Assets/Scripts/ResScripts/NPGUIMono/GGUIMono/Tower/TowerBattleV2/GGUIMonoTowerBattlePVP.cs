using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerBattlePVP : _AALBasicUIWndMono
{
    [ALHeader("开场停留时间")]
    public float cutSceneDelay = 2f; 
    [ALHeader("开场停留时间10倍速 ")]
    public float cutSceneDelay10x = 1f; 
    [ALHeader("最大战斗结束延迟时间，从开场播完开始算保底用，避免策划忘记配置战斗结束事件")]
    public float maxEndDelay = 15f;
    [ALHeader("战斗开场界面需要显示的Go")]
    public List<GameObject> cutSceneShowGos = new List<GameObject>();
    
    [ALHeader("血跳字预制体")]
    public GGUIMonoTowerBattleHPTip hpTipPrefab;
    [ALHeader("血跳字父节点")]
    public Transform hpTipParent;
    [ALHeader("跳过按钮")]
    public GameObject btnJump;
    [ALHeader("10倍速开关")]
    public NPGGUIMonoCommonToggleEx toggleSpeedTen;	
    [ALHeader("10倍速开关点击间隔时长")]
    public float toggleSpeedTenInterval = 1f;
    
    [ALHeader("关卡名称文本")]
    public TextEx txtLevelName;

    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("大臣战力进度调")]
    public GGUIMonoCommonBlood heroPowerSlider;
    [ALHeader("敌方信息")]
    public NPGGUIMonoPlayerIcon bossInfo;
    [ALHeader("敌方血条")]
    public GGUIMonoCommonBlood bossHPSlider;
    
    [ALHeader("10倍速开关开启后的视频速度")] 
    public float videoSpeedTenSpeed = 2;
    
    [ALHeader("boss形象视频展示")]
    public GGUIMonoSimpleVideo monoShowcaseBoss;
    [ALHeader("战斗动画:事件如下：")]
    [ALHeader("ReduceHP:扣血事件目标boss血量比从1到0")]
    [ALHeader("EndBattle:战斗结束事件")]
    public Animation battleAnim;
    [ALHeader("战斗动画名称")]
    public string battleAniName;
    
    public Action<float> onEventReduceHp;
    public Action onEventBattleEnd;

    /// <summary>
    /// 执行扣血表现
    /// </summary>
    public void ReduceHP(float _percentage)
    {
        onEventReduceHp?.Invoke(_percentage);
    }

    /// <summary>
    /// 
    /// </summary>
    public void EndBattle()
    {
        onEventBattleEnd?.Invoke();
    }
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5315); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5315);} }
}