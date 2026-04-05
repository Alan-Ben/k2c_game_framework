using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 物品概率详情界面
/// </summary>
public class GGUIMonoItemPercentDetail : _AALBasicUIWndMono
{
    public GGUIMonoItemPercentDetailGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(55); } }
    public static string objName { get { return UIResPathAssistant.getObjName(55);} }
}