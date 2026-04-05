using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;

/**********************
 * 登录界面的游戏外功能窗口
 **/
public class NPGGUIMonoStartGame : _AALBasicUIWndMono
{
    [ALHeader("服务器显示对象")]
    public NPGGUIMonoServerSingleItem serverItem;//

    [ALHeader("切服按钮")]
    public GameObject switchServerBtn;//
    [ALHeader("进入按钮")]
    public GameObject enterBtn;//

    [ALHeader("登入过程显示的go列表")]
    public List<GameObject> loginShowGoList;//
    [ALHeader("登入过程隐藏的go列表")]
    public List<GameObject> loginHideGoList;//
    
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2301); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2301); } }
}
