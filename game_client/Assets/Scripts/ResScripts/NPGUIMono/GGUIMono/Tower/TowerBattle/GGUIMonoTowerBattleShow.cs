using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

[Serializable]
public class TowerBattleChatperShowAnim
{
    [ALHeader("章节id")]
    public long chapterId;
    [ALHeader("正常显示动画名字")]
    public string oneTimesAniName;
    [ALHeader("10倍速动画名字")]
    public string tenTimesAniName;
}
/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerBattleShow : _AALBasicUIWndMono
{
    [ALHeader("正常显示动画名字")]
    public string oneTimesAniName;
    [ALHeader("10倍速动画名字")]
    public string tenTimesAniName;
    [ALHeader("章节特殊展示动画")]
    public List<TowerBattleChatperShowAnim> chapterShowAnims;
    [ALHeader("我方形象视频展示")]
    public GGUIMonoCommonShowCase monoShowcaseSelf;
    [ALHeader("boss形象视频展示")]
    public GGUIMonoCommonShowCase monoShowcaseBoss;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5310); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5310);} }
}