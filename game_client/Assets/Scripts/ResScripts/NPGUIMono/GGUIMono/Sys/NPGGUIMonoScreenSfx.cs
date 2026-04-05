using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 屏幕点击特效窗口
/// </summary>
public class NPGGUIMonoScreenSfx : _AALBasicUIWndMono
{
    [ALHeader("特效父节点")]
    public Transform sfxParent;
    [ALHeader("特效id")]
    public long sfxId;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_SCREEN_SFX); } }
    public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_SCREEN_SFX); } }
}
