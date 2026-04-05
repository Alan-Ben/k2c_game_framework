using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

/**********************
 * 游戏版本号显示 客户端版本号 服务端版本号 资源版本号
 **/
public class NPPGUIMonoVersion : _AALBasicUIWndMono
{
    [ALHeader("客户端版本号")]
    public Text ClientVersion;//客户端版本号
    [ALHeader("服务端版本号")]
    public Text ServerVersion;//服务端版本号
    [ALHeader("资源版本号")]
    public Text AssetsVersion;//资源版本号
    [ALHeader("配表版本号")]
    public Text refDataVersion;//配表版本号\
    [ALHeader("平台资源版本号")]
    public Text platVersion;//配表版本号
    [ALHeader("IF热更版本号")]
    public Text InjectFixVersion;//IF热更版本号
    [ALHeader("Hotfix热更版本号")]
    public Text HotfixVersion;//Hotfix热更版本号

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
    public static string objName { get { return "win_versioninfo"; } }
}
