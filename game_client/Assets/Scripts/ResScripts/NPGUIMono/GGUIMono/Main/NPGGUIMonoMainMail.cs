using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;



public class NPGGUIMonoMainMail : _AALBasicUIWndMono
{
    public Text txtNoReadCount;
    public GameObject goNoReadIcon;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "gui/game_gui.unity3d"; } }
    public static string objName { get { return "win_main_mail"; } }

}
