using System.Collections.Generic;
using ALPackage;
using GOE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoMiddayDungeonBattle : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("攻击按钮")]
    public GameObject btnAttack;
    [ALHeader("活动结束倒计时")]
    public Text txtEndCd;
    [ALHeader("自动战斗按钮")]
    public NPGGUIMonoCommonToggleEx toggleAuto;
    [ALHeader("开启自动战斗后, 进入下一波攻击的延迟时间")]
    public float autoBattleDelayTime = 1;
    [ALHeader("开启自动战斗后，自动隐藏奖励时间")]
    public float autoHideRewardTime = 2;
    [ALHeader("一键战斗按钮")]
    public GameObject btnOneKeyBattle;
    [ALHeader("选择大臣按钮")]
    public GameObject btnSelectHero;
    [ALHeader("公会协助")]
    public GameObject btnGuildHero;
    
    
    [ALHeader("boss形象展示")]
    public GGUIMonoCommonShowCase monoShowcaseBoss;

    public GGUIMonoSimpleVideo monoSimpleVideo;
    [ALHeader("当前选中大臣名字")]
    public TextEx txtCurHeroName;
    [ALHeader("当前选中大臣信息")]
    public GGUIMonoHeroCommonCardItem monoCurHeroCard;
    public GGUIMonoHeroIconItem monoCurHeroIcon;
    
    [ALHeader("大臣战斗文本")]
    public TextEx txtHeroBubble;

    [ALHeader("当前波数/总波次")]
    public Text txtCurWave;
    [ALHeader("Boss名称")]
    public TextEx txtBossName;
    public TextMeshProUGUI txtBossNameTMP;
    [ALHeader("Boss血条")]
    public NPGGUIMonoProgress bossHPSlider;
    [ALHeader("掉血时长")]
    public float hpReduceTime = 1;
    [ALHeader("进度信息")]
    public TextMeshProUGUI txtProgressTMP;
    [ALHeader("Boss血量文本")]
    public Text txtBossHp;
    [ALHeader("宝箱窗口")]
    public GGUIMonoMiddayDungeonMiniBox monoMiniBox;
    [ALHeader("宝箱窗口")]
    public GGUIMonoMiddayDungeonBox monoBox;
    [ALHeader("当有选中大臣时显示")]
    public List<GameObject> goShowOnSelectedHero;
    [ALHeader("当没有选中大臣时显示")]
    public List<GameObject> goHideOnSelectedHero;
    
    [ALHeader("当后续没有boss的时候显示和隐藏的go")]
    public List<GameObject> goShowOnNoBoss;
    public List<GameObject> goHideOnNoBoss;
    [ALHeader("开场视频")]
    public GVideoClipIndex cutSceneVideoIndex = new GVideoClipIndex(2201, 1); // 开场视频
    public float cutSceneAnimDelay = 2;
    [ALHeader("成功击败Boss的视频播放的延迟多久结束")]
    public float battleSucVideoStopDelay = 3;

    [ALHeader("窗口动画")]
    public Animation wndAnim;
    [ALHeader("开场视频播完的显示动画")]
    public string cutSceneShowAniName;
    [ALHeader("攻击动画名")]
    public string attackAniName;
    [ALHeader("成功击败Boss的动画名")]
    public string battleSucAniName;
    [ALHeader("击败Boss后的显示窗口动画名")]
    public string sucToNextBattleAniName;
    [ALHeader("大臣切换动画")]
    public Animation changeHeroAnim;
    [ALHeader("切换大臣的动画名称")]
    public string changeHeroAniName;
    
    [ALHeader("战斗过程中的显示和隐藏的go")]
    public List<GameObject> goShowOnAttackShow;
    public List<GameObject> goHideOnAttackShow;

    [ALHeader("大臣用完，但还有联盟援助次数时显示的提示列表")]
    public List<GameObject> goNoHeroButHaveGuildHeroShowList;
    [ALHeader("大臣用完，联盟援助次数也没有时显示的提示列表")]
    public List<GameObject> goNoHeroAndNoHaveGuildHeroShowList;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5401); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5401);} }
}