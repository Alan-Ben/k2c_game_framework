using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;

/**********************
 * 登录界面的游戏外功能窗口
 **/
public class NPPGUIMonoOutGameFuncs : _AALBasicUIWndMono
{
    [ALHeader("Logo的父节点")]
    public Transform logoParent;
    [ALHeader("客服按钮")]
    public GameObject aiHelpBtn;//
    [ALHeader("切换账号按钮")]
    public GameObject switchAccountBtn;//
    [ALHeader("公告按钮")]
    public GameObject noticBtn;//
    [ALHeader("清除缓存按钮")]
    public GameObject clearBtn;//

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "win_login_game_funcs"; } }
}
