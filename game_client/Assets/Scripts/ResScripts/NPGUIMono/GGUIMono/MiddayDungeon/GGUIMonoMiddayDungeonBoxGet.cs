using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoMiddayDungeonBoxGet : _AALBasicUIWndMono
{
    [ALHeader("打开宝箱按钮")]
    public GameObject btnOpen;
    [ALHeader("宝箱图标")]
    public RawImage boxIcon;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5402); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5402);} }
}