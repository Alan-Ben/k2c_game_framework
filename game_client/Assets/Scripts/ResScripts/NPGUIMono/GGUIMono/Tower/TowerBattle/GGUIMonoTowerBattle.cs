using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerBattle : _AALBasicUIWndMono
{
    [ALHeader("跳过按钮")]
    public GameObject btnJump;
    [ALHeader("关卡名称文本")]
    public TextEx txtLevelName;
    [ALHeader("守卫名称文本")]
    public TextEx txtBossName;
    [ALHeader("BossIcon图片")]
    public RawImage texIcon;
    [ALHeader("剩余伙伴数量")]
    public TextEx txtHeroNum;
    [ALHeader("伙伴总战力")]
    public TextEx txtPlayerPower;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("10倍速开关")]
    public NPGGUIMonoCommonToggleEx toggleSpeedTen;	
    [ALHeader("Boss血条一次掉血的时间")]
    public float bossHPSliderDuration = 0.5f;
    [ALHeader("Boss血条一次掉血的时间")]
    public float bossHPSlider10xDuration = 0.1f;
    [ALHeader("Boss血条")]
    public NPGGUIMonoProgress bossHPSlider;
    [ALHeader("大臣数量进度条")]
    public NPGGUIMonoProgress heroCountSlider;
    [ALHeader("大臣战力进度调")]
    public NPGGUIMonoProgress heroPowerSlider;
    [ALHeader("血跳字预制体")]
    public GGUIMonoTowerBattleHPTip hpTipPrefab;
    [ALHeader("血跳字父节点")]
    public Transform hpTipParent;
    
    [ALHeader("伙伴气泡鼓舞描述文本随机列表")]
    public List<string> inspireDescList = new List<string>();
    
    
    [ALHeader("十倍速显隐Gos")]
    public List<GameObject> goSpeedTenShowList = new List<GameObject>();
    public List<GameObject> goSpeedTenHideList = new List<GameObject>();
    [ALHeader("可以跳过显隐Gos")]
    public List<GameObject> goSkipShowList = new List<GameObject>();
    public List<GameObject> goSkipHideList = new List<GameObject>();
    [ALHeader("第一个鼓舞组出现延迟时间")]
    public float firstInspireDelay = 1;
    [ALHeader("10倍速第一个鼓舞组出现延迟时间")]
    public float firstInspire10xDelay = 0.5f;
    [ALHeader("鼓舞组出现延迟时间")] 
    public WCGFloatRange inspireGroupDelay = new WCGFloatRange(2, 4);
    [ALHeader("10倍速鼓舞组出现延迟时间")]
    public WCGFloatRange inspireGroup10xDelay = new WCGFloatRange(0.5f,1);
    [ALHeader("鼓舞左预制体")]
    public GGUIMonoTowerBattleInspire inspireLeftPrefab;
    [ALHeader("鼓舞右预制体")]
    public GGUIMonoTowerBattleInspire inspireRightPrefab;
    [ALHeader("鼓舞预制体Cache父节点")]
    public Transform inspireCacheRoot;
    [ALHeader("鼓舞组随机列表")]
    public List<TowerBattleInspireGroup> inspireGroupList;
    
    /// <summary>
    /// 获取随机的鼓舞描述
    /// </summary>
    /// <returns></returns>
    public string getRandomInspireDesc()
    {
        return inspireDescList.GetRandomItem();
    }

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5303); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5303);} }
}