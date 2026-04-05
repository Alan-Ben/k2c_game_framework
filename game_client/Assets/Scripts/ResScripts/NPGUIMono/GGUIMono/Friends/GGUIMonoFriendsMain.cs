using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;


/// <summary>
/// 好友主界面
/// </summary>
///
public class GGUIMonoFriendsMain : _AALBasicUIWndMono
{
    [ALHeader("好友列表容器")]
    public GGUIMonoFriendsMainGrid gridMono;

    [ALHeader("添加好友按钮")]
    public GameObject addFriendBtn;

    [ALHeader("好友申请按钮")]
    public GameObject friendRequestBtn;

    [ALHeader("屏蔽列表按钮")]
    public GameObject blockListBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2601); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2601); } }
}
