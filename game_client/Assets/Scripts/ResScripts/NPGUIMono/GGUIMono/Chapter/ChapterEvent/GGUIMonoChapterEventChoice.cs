using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoChapterEventChoice : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("标题")]
    public TextEx txtTitle;
    [ALHeader("描述")]
    public TextEx txtDesc;
    [ALHeader("选项容器")]
    public GGUIMonoChapterEventChoiceItemContainer monoChoiceContainer;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2112); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2112);} }
}