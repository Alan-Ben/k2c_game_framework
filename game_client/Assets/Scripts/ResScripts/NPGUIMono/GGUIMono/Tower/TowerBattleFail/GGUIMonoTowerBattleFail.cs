using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerBattleFail : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;

    public GameObject btnClose2;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5305); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5305);} }
}