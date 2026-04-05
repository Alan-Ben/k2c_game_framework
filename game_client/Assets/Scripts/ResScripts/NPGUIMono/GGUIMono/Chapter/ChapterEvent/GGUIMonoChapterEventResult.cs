using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoChapterEventResult : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("关闭按钮")]
    public GameObject btnBgClose;
    [ALHeader("标题")]
    public TextEx txtTitle;
    [ALHeader("描述")]
    public TextEx txtDesc;
    [ALHeader("奖励列表")]
    public NPGGUIMonoGetItemContainer itemContainer;
    [ALHeader("粒子开始位置")]
    public RectTransform particleStartRectTransform;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2113); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2113);} }
}