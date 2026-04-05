using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerBattleCrossChapter : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("解锁新章节描述文本")]
    public Text txtDesc;
    [ALHeader("解锁新章节图片")]
    public RawImage imgChapter;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer itemContainer;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5312); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5312);} }
}