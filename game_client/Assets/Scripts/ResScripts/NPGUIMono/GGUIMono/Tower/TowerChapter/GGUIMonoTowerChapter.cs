using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerChapter : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("关闭按钮列表")]
    public List<GameObject> btnCloseList;
    [ALHeader("研究按钮")]
    public GameObject btnResearch;
    [ALHeader("章节关卡item列表")]
    public GGUIMonoTowerChapterItemGrid towerChapterItemGrid;
    [ALHeader("章节标题文本")]
    public Text txtChapterTitle;
    [ALHeader("每次刷新显示的关卡数量")]
    public int perPageItemNum = 15;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5301); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5301);} }
}