using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟宝箱详情
/// </summary>
public class GGUIMonoGuildBoxDetail : _AALBasicUIWndMono
{
    public GGUIMonoGuildBoxDetailGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    public Text txtBoxName;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(8704); } }
    public static string objName { get { return UIResPathAssistant.getObjName(8704);} }
}