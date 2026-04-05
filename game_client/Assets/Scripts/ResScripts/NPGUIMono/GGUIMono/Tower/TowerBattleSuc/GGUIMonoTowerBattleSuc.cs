using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerBattleSuc : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer itemContainer;
    [ALHeader("提高了多少层描述")]
    public TextEx txtUpLevelNum;
    [ALHeader("新关卡名称文本")]
    public TextEx txtLevelName;
    [ALHeader("旧关卡名称文本")]
    public TextEx txtOldLevelName;
    [ALHeader("有奖励需要显示的Go")] 
    public List<GameObject> hasRewardShowGos;
    [ALHeader("科技研究进度提升描述")] 
    public Text txtResearchUpDesc;
    [ALHeader("下一个科技点描述")]
    public TextEx txtNextResearchDesc;
    [ALHeader("有完成带科技关卡需要显示的Go")] 
    public List<GameObject> hasAddResearchShowGos;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5304); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5304);} }
}