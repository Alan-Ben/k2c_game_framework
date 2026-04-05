using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟PVE自动开启
/// </summary>
public class GGUIMonoGuildDungeonAutoOpen : _AALBasicUIWndMono
{
    public GGUIMonoGuildDungeonAutoOpenGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    [ALHeader("消耗公会财富文本")]
    public Text txtGuildWealth;
    [ALHeader("确定按钮")]
    public GameObject btnConfirm;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    
    public GGUIMonoGuildDungeonTimeContainer hourSelectContainer;
    public GGUIMonoGuildDungeonTimeContainer minuteSelectContainer;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6902); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6902);} }
}
