using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 新晋者列表界面
/// </summary>
public class GGUIMonoGraveNewProminent : _AALBasicUIWndMono
{
    public GGUIMonoGraveNewProminentGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("祝贺按钮")]
    public GameObject btnCongrats;
    // </AutoGen:MonoDeclaration>
    [ALHeader("可获得钻石数量")]
    public Text txtDiamondCount;
    [ALHeader("有新晋者需要显示的GO")]
    public List<GameObject> hasNewProminentShowGos;
    [ALHeader("无新晋者需要显示的GO")]
    public List<GameObject> noNewProminentShowGos;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6604); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6604);} }
}