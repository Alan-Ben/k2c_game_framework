using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 爬塔战报
/// </summary>
public class GGUIMonoTowerLog : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("战报itemGrid")]
    public GGUIMonoTowerLogItemGrid itemGrid;
    [ALHeader("列表为空时显示的物体")]
    public List<GameObject> emptyShowGos;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5306); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5306);} }
}