using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会举办弹窗
/// </summary>
public class GGUIMonoDinnerCreate : _ANPBasicUIWndResBarMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("宴会举办方式列表 普通")]
    public GGUIMonoDinnerCreateItemContainer dinnerTypeContainer;
    [ALHeader("宴会举办方式列表 凭证")]
    public GGUIMonoDinnerCreateItemContainer dinnerPermitContainer;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2902); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2902);} }
}