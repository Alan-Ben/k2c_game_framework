using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 杰出者祝福
/// </summary>
public class GGUIMonoGraveBlessGet : _AALBasicUIWndMono
{

    // <AutoGen:MonoDeclaration>
    [ALHeader("祝福名")]
    public Text txtBlessName;
    [ALHeader("祝福描述")]
    public Text txtBlessDesc;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6603); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6603);} }
}