using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;

/**********************
 * 游戏初次进入的窗口脚本,开发包用
 **/
public class NPGGUIMonoUseAccount : _AALBasicUIWndMono
{
    [ALHeader("账号输入框")]
    public InputField userName;//账号输入框
    [ALHeader("密码输入框")]
    public InputField passWord;//密码输入框
    [ALHeader("服务器id输入框")]
    public InputField userServerID;//服务器id输入框
    [ALHeader("服务器IP输入框")]
    public InputField userServerIP;//服务器IP输入框
    [ALHeader("服务器端口输入框")]
    public InputField userServerPort;//服务器端口输入框
    [ALHeader("登录按钮")]
    public GameObject enterBtn;//登录按钮

    [ALHeader("历史用户名快捷输入")]
    public GameObject moreUserName;//历史用户名快捷输入

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2306); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2306); } }
}
