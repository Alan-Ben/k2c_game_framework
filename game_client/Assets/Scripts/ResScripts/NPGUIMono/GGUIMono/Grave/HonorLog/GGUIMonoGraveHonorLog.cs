using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 荣誉列表界面
/// </summary>
public class GGUIMonoGraveHonorLog : _AALBasicUIWndMono
{
    [ALHeader("每次刷新列表显示的数量")]
    public int perPageItemNum = 6;
    public GGUIMonoGraveHonorLogGrid itemGrid;
    [ALHeader("列表为空时显示的物体")]
    public List<GameObject> emptyShowGos;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6606); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6606);} }
}