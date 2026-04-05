using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟副本升级
/// </summary>
public class GGUIMonoGuildDungeonUpgrade : _AALBasicUIWndMono
{
    public GGUIMonoGuildDungeonUpgradeGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6903); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6903);} }
}
