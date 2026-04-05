using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 午间副本宝箱领取详情
/// </summary>
public class GGUIMonoMiddayDungeonBoxRecord : _AALBasicUIWndMono
{
    public GGUIMonoMiddayDungeonBoxRecordGrid itemGrid;
    // <AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("剩余可领取次数")]
    public Text txtCount;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5406); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5406);} }
}