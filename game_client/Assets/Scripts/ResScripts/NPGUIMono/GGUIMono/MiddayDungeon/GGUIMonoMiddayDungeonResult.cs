using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GGUIMonoMiddayDungeonResult : _AALBasicUIWndMono
{
    public GameObject btnClose;
    [ALHeader("奖励列表")]
    public NPGGUIMonoGetItemContainer itemContainer;
    [ALHeader("积分")]
    public TextEx txtScore;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5404); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5404);} }
}