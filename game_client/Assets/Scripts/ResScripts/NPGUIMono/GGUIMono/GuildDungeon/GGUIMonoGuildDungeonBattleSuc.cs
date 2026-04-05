using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟PVE战斗成功
/// </summary>
public class GGUIMonoGuildDungeonBattleSuc : _AALBasicUIWndMono
{

    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭")]
    public GameObject btnClose;
    [ALHeader("奖励")]
    public NPGGUIMonoGetItemContainer rewardsContainer;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6908); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6908);} }
}
