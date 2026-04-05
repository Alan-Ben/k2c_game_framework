using System;
using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine.UI;


public class NPGGUIMonoCheat : _AALBasicUIWndMono {

    [ALHeader("关闭按钮")]
    public GameObject btnClose;//关闭按钮
    [ALHeader("获取全部命令")]
    public GameObject btnHelp;//帮助按钮
    [ALHeader("清理筛选")]
    public GameObject btnClear;//清理筛选
    [ALHeader("所有命令内容滚动")]
    public ScrollRect allCmdScrollRect;//回包内容滚动
    [ALHeader("所有命令内容")]
    public Text txtAllCMDLog;//所有命令内容
    [ALHeader("回包内容滚动")]
    public ScrollRect logScrollRect;//回包内容滚动
    [ALHeader("回包内容")]
    public Text txtConsoleLog;//回包内容

    [ALHeader("筛选内容，只筛选命令头")]
    public InputField txtFiltLog;//回包内容

    public NPGGUIMonoCheatInputItem monoInputItem;//输入条目列表
    
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/game_gui.unity3d"; } }
    public static string objName { get { return "win_cheat"; } }

}
