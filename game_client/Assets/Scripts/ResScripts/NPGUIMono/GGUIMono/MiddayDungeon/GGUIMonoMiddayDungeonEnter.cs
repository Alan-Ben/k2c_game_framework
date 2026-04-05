using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoMiddayDungeonEnter : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("前往战斗按钮")]
    public GameObject btnBattle;
    [ALHeader("排行榜按钮")]
    public GameObject btnRank;
    [ALHeader("午间副本时间")]
    public Text txtMiddayDungeonTime;
    [ALHeader("晚间副本时间")]
    public Text txtNightDungeonTime;
    [ALHeader("Mini宝箱窗口")]
    public GGUIMonoMiddayDungeonMiniBox monoMiniBox;
    [ALHeader("宝箱窗口")]
    public GGUIMonoMiddayDungeonBox monoBox;
    [ALHeader("副本开启倒计时")]
    public Text txtOpenCd;
    [ALHeader("活动再准备中要显示的go")]
    public List<GameObject> goShowInPrepare;
    [ALHeader("活动进行中要显示的go")]
    public List<GameObject> goShowInBattle;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5400); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5400);} }
}