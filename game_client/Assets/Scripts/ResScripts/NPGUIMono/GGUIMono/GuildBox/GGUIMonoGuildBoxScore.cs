using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟宝箱
/// </summary>
public class GGUIMonoGuildBoxScore : _AALBasicUIWndMono
{
    public GGUIMonoGuildBoxScoreContainer itemContainer;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(8703); } }
    public static string objName { get { return UIResPathAssistant.getObjName(8703);} }
}
