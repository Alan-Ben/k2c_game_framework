using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 妃子入口
/// </summary>
public class GGUIMonoConsortEnter:_AALBasicUIWndMono
{
    [ALHeader("好友拜访入口")]
    public GameObject friendVisitEntry;

    [ALHeader("好友玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfoMono;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1402); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1402);} }
}