using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会赴宴消耗弹窗
/// </summary>
public class GGUIMonoDinnerCostList : _ANPBasicUIWndResBarMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("关闭按钮2")]
    public GameObject btnClose2;
    [ALHeader("宴会举办消耗列表")]
    public GGUIMonoDinnerCostItemContainer dinnerTypeContainer;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2909); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2909);} }
}