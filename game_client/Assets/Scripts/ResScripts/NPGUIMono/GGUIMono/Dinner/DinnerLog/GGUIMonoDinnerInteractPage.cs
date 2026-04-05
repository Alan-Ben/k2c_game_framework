using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会玩家交互页面
/// </summary>
public class GGUIMonoDinnerInteractPage : _AALBasicUIWndMono
{
    [ALHeader("交互列表")]
    public GGUIMonoDinnerInteractItemGrid logItemGrid;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2917); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2917);} }
}