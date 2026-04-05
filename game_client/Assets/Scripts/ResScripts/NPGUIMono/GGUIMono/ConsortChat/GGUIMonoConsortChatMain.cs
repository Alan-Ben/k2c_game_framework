using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 页签类型
/// </summary>
public enum EConsortChatMainPage
{
    CHAT,//聊天
    MOMENTS,//朋友圈
    CONSORT_LIST, //情人列表
}

/// <summary>
/// 
/// </summary>
public class GGUIMonoConsortChatMain : _AALBasicUIWndMono
{
    [ALHeader("子窗口页面")]
    public Transform pageParent;
    [ALHeader("聊天按钮")]
    public NPGGUIMonoCommonTab tabChat;
    [ALHeader("朋友圈按钮")]
    public NPGGUIMonoCommonTab tabMoments;
    [ALHeader("妃子列表页面")]
    public NPGGUIMonoCommonTab tabConsortList;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6200); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6200);} }
}