using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 举办历史页面
/// </summary>
public class GGUIMonoDinnerStartLogPage : _AALBasicUIWndMono
{
    [ALHeader("举办列表")]
    public GGUIMonoDinnerStartItemGrid logItemGrid;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2916); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2916);} }
}