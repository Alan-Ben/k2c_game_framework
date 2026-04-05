using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;


/// <summary>
/// 集市主界面
/// </summary>
///
public class GGUIMonoMarketMain : _AALBasicUIWndMono
{
    [ALHeader("重置倒计时")]
    public NPGGUIMonoCommonCountDown countDownMono;

    [ALHeader("升级按钮")]
    public GameObject lvUpBtn;

    [ALHeader("一键经营按钮")]
    public GameObject akeyBtn;

    [ALHeader("一键经营成功多久后弹获得弹窗")]
    public float marginTimeSec = 1.0f;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3900); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3900); } }
}
