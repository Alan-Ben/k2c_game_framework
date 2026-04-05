using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟PVE地图
/// </summary>
public class GGUIMonoGuildDungeonMap : _AALBasicUIWndMono
{
    public RawImage imgBg;
    public GGUIGuildDungeonMapMono mapMono;

    // <AutoGen:MonoDeclaration>
    [ALHeader("日志按钮")]
    public GameObject btnLog;
    [ALHeader("标记按钮")]
    public GameObject btnMark;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    
    public List<GameObject> goShowOnTagMode; // 标记模式下显示的物体
    public List<GameObject> goHideOnTagMode; // 标记模式下隐藏的物体
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6909); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6909);} }
}