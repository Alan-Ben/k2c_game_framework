using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoDinnerDetailLog : _AALBasicUIWndMono
{
    public GameObject btnClose;
    public GGUIMonoDinnerDetailLogItemGrid itemGrid;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2924); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2924);} }
}