using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 关卡事件提示窗口
/// </summary>
public class GGUIMonoChapterEventPreTip : _AALBasicUIWndMono
{
    [ALHeader("窗口关闭时间")]
    public float showTime = 2.0f;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2115); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2115);} }
}