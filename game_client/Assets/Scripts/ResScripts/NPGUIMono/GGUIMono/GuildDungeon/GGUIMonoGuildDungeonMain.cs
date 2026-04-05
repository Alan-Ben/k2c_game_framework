using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟PVE主界面
/// </summary>
public class GGUIMonoGuildDungeonMain : _AALBasicUIWndMono
{
    public GGUIMonoGuildDungeonMainGrid itemGrid;
    [ALHeader("关闭")]
    public GameObject btnClose;
    // <AutoGen:MonoDeclaration>
    [ALHeader("自动开启")]
    public GameObject btnAutoStart;
    [ALHeader("副本升级")]
    public GameObject btnUpgrate;
    [ALHeader("战报排行")]
    public GameObject btnRank;
    [ALHeader("一键领取")]
    public GameObject btnClaimAll;
    [ALHeader("重置时间")]
    public Text txtResetTime;

    [ALHeader("自动升级显示Gos")]
    public List<GameObject> autoStartShowGos;
    [ALHeader("有权限升级显示Gos")]
    public List<GameObject> upgradeShowGos;
    // </AutoGen:MonoDeclaration>
    
    [ALHeader("有奖励可领取显示")]
    public List<GameObject> hasRewardShowGos;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6900); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6900);} }
}