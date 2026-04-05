using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerResearch : _AALBasicUIWndMono
{
    [ALHeader("进度文本")]
    public Text txtProcess;
    [ALHeader("章节容器")]
    public GGUIMonoTowerResearchItemContainer chapterContainer;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5316); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5316);} }
}