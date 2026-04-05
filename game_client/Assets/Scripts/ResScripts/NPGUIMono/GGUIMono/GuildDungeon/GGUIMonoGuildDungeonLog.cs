using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟副本日志
/// </summary>
public class GGUIMonoGuildDungeonLog : _AALBasicUIWndMono
{
    public GameObject btnClose;
    public GGUIMonoGuildDungeonLogGrid itemGrid;
    [ALHeader("列表为空时显示的物体")]
    public List<GameObject> emptyShowGos;
    // <AutoGen:MonoDeclaration>
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6907); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6907);} }
}
