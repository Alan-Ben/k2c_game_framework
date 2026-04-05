using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;

/**********************
 * 登录界面的游戏外功能窗口
 **/
public class NPGGUIMonoQueue : _AALBasicUIWndMono
{
    [ALHeader("排队中的文本显示")]
    public TextEx textQueue;

    [ALHeader("服务器显示对象")]
    public NPGGUIMonoServerSingleItem serverItem;//

    [ALHeader("切服按钮")]
    public GameObject switchServerBtn;//

    [ALHeader("退出队列操作按钮")]
    public GameObject quitQueueBtn;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2310); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2310); } }
}
