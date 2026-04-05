using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会赴宴玩家信息列表
/// </summary>
public class GGUIMonoDinnerGuestInfoList : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("赴宴信息列表")]
    public GGUIMonoDinnerGuestItemGrid guestItemGrid;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2912); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2912);} }
}