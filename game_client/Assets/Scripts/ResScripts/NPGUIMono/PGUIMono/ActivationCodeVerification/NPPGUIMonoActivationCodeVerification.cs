using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

/**********************
 * 游戏初次进入的窗口脚本
 **/
public class NPPGUIMonoActivationCodeVerification : _AALBasicUIWndMono
{
    public InputField activeCode;//激活码输入框
    public GameObject checkBtn;//验证按钮
    public GameObject backBtn;//返回登陆按钮
    public Animation codeErrorAnim;//验证码错误的表现
    public int codeLenght;//激活码长度

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "win_activation_code_verification"; } }
}
