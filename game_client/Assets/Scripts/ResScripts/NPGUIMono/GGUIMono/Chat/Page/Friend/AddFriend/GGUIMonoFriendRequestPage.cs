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
public class GGUIMonoFriendRequestPage : _AALBasicUIWndMono
{
    [ALHeader("列表容器")]
    public GGUIMonoRequestFriendsGrid gridMono;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1318); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1318); } }
}
