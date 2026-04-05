using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游历遇到妃子的提示界面
/// </summary>
public class GGUIMonoTravelResult_MeetConsortAddLikeTip:_AALBasicUIWndMono
{
    [ALHeader("界面存在时长")]
    public float closeDelayTime = 1;
    
    [ALHeader("关闭窗口按钮")]
    public GameObject btnClose;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3611); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3611); } }
}