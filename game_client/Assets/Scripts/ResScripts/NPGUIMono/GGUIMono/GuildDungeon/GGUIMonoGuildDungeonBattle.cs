using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;
using GOE;


/// <summary>
/// 联盟PVE战斗
/// </summary>
public class GGUIMonoGuildDungeonBattle : _AALBasicUIWndMono
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("背景")]
    public RawImage imgBg;
    [ALHeader("选择大臣")]
    public GameObject btnSelectHero;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("boss血条")]
    public GGUIMonoCommonBlood bloodBoss;
    [ALHeader("boss名字")]
    public Text txtBossName;
    [ALHeader("攻击按钮")]
    public GameObject btnBattle;
    [ALHeader("自动战斗")]
    public NPGGUIMonoCommonToggleEx toggleAutoBattle;
    // </AutoGen:MonoDeclaration>

    [ALHeader("战斗动画")]
    public Animation battleAni;
    [ALHeader("战斗动画名称")]
    public string battleAniName;
    [ALHeader("切换角色动画")]
    public Animation changeHeroAni;
    [ALHeader("切换角色动画名称")]
    public string changeHeroAniName;
    
    [ALHeader("我方形象视频展示")]
    public GGUIMonoCommonShowCase monoShowcaseSelf;
    [ALHeader("大臣默认动画名称")]
    public string heroDefaultAniName = "idle";
    [ALHeader("boss形象视频展示")]
    public GGUIMonoCommonShowCase monoShowcaseBoss;
    [ALHeader("Boss Icon")]
    public RawImage imgBossIcon;
    [ALHeader("当前选中大臣信息")]
    public GGUIMonoHeroCommonCardItem monoCurHeroCard;
    [ALHeader("当有选中大臣时显示")]
    public List<GameObject> goShowOnSelectedHero;
    [ALHeader("当没有选中大臣时显示")]
    public List<GameObject> goHideOnSelectedHero;
    [ALHeader("开启自动战斗后, 进入下一波攻击的延迟时间")]
    public float autoBattleDelayTime = 1;
    [ALHeader("战斗中显示Gos")]
    public List<GameObject> goShowOnBattle;
    [ALHeader("战斗中隐藏Gos")]
    public List<GameObject> goHideOnBattle;

    [ALInfo("======== 血跳字相关 ========")]
    [ALHeader("血跳字预制体")]
    public GGUIMonoChapterBossHPTip hpTipPrefab;
    [ALHeader("血跳字cache父节点")]
    public Transform hpTipCacheRoot;
    [ALHeader("血跳字父节点")]
    public Transform hpTipParent;
    [ALHeader("血跳字动画名")]
    public string hpTipAniName;

    [ALInfo("======== boos被击败配置 ========")]
    [ALHeader("boos被击败时显示的GO列表")]
    public List<GameObject> goKillShowList;
    [ALHeader("boos被击败时隐藏的GO列表")]
    public List<GameObject> goKillHideList;
    [ALHeader("boos被击败后关闭按钮")]
    public GameObject btnKillClose;


    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6905); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6905); } }
}
