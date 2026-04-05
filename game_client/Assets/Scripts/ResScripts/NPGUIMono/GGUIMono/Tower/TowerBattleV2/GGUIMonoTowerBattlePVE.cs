using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


[Serializable]
public class TowerBloodChangeParam
{
    [ALHeader("开始变化延时时间秒（相对于开始战斗的时间）")]
    public float delayTimeSec;
    [ALInfo("最后一条这个百分比配置会根据前几条配置的百分比计算，保证总的是100%")]
    [ALHeader("变化的血量占总变化血量的百分比")]
    public float changePercentage;
}
[Serializable]
public class TowerBloodChange
{
    public List<TowerBloodChangeParam> changeParams = new List<TowerBloodChangeParam>();
}
/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerBattlePVE : _AALBasicUIWndMono
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
    [ALHeader("关卡名称文本")]
    public TextEx txtLevelName;
    [ALHeader("守卫名称文本")]
    public TextEx txtBossName;
    [ALHeader("BossIcon图片")]
    public RawImage texIcon;
    [ALHeader("Boss血条")]
    public GGUIMonoCommonBlood bossHPSlider;
    [ALHeader("伙伴总战力")]
    public TextEx txtPlayerPower;
    [ALHeader("10倍速开关")]
    public NPGGUIMonoCommonToggleEx toggleSpeedTen;
    [ALHeader("10倍速开关点击间隔时长")]
    public float toggleSpeedTenInterval = 1f;
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

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5314); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5314);} }
}