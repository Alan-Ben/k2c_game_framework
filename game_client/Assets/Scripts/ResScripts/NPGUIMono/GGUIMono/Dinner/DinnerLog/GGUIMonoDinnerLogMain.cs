using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 宴会消息主界面
/// </summary>
public class GGUIMonoDinnerLogMain : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    public GameObject btnClose2;
    [ALHeader("历史宴会页签")]
    public NPGGUIMonoCommonTab tabStartLog;
    [ALHeader("来往记录页签")]
    public NPGGUIMonoCommonTab tabInteractLog;
    [ALHeader("加载的页面父节点")]
    public Transform pageParent;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2915); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2915);} }
}