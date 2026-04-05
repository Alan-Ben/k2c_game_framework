using UnityEngine;
using ALPackage;
using GOE;

/// <summary>
/// 游历过程跳过界面
/// </summary>
///
public class GGUIMonoTravelProcessSkip : _AALBasicUIWndMono
{
    [ALHeader("游历按钮")]
    public GameObject btnSkip;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3617); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3617); } }
}
