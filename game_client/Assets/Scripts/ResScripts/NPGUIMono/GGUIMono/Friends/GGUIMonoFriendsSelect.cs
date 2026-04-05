using ALPackage;
using GOE;
using UnityEngine;


/// <summary>
/// 好友选择列表界面
/// </summary>
public class GGUIMonoFriendsSelect : _AALBasicUIWndMono
{
    [ALHeader("列表容器")]
    public GGUIMonoFriendsSelectGrid gridMono;
    
    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1358); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1358); } }
}
