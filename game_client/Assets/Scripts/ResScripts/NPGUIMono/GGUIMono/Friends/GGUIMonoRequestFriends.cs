using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;


/// <summary>
/// 好友申请列表界面
/// </summary>
public class GGUIMonoRequestFriends : _AALBasicUIWndMono
{
    [ALHeader("列表容器")]
    public GGUIMonoRequestFriendsGrid gridMono;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2603); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2603); } }
}
