using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoConsortChatImageGroupDetail : _AALBasicUIWndMono
{
    [ALHeader("点击按钮")]
    public GameObject btnClick;

    public Transform imageGroupParent;
    [ALHeader("左右按钮")]
    public GameObject btnLeft;
    public GameObject btnRight;
    
    [ALHeader("可左右切换显示对象")]
    public List<GameObject> hasLeftShowGos;
    public List<GameObject> hasRightShowGos;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6204); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6204);} }
}