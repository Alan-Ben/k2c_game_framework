using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using GOE;


public class NPGGUIMonoMainPlayer : _AALBasicUIWndMono
{

    [ALHeader("玩家头像框")]
    public NPGGUIMonoPlayerIcon monoPlayerIcon;

    //public WCGGGUIMonoGradeIcon monoGradeIcon;//段位信息

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1502); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1502);} }

}
