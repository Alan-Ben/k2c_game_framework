using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 游历妃子列表
/// </summary>
public class GGUIMonoTarvelConsortList:_AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("妃子列表")]
    public GGUIMonoTarvelConsortItemContainer itemContainer;
        
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3602); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3602); } }
}