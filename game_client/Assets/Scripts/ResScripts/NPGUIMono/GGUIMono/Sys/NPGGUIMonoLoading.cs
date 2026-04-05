using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;

/// <summary>
/// 游戏加载的附加窗口脚本
/// </summary>
public class NPGGUIMonoLoading : _AALBasicUIWndMono {
     public Animator anim;//动画
     public float expandTime;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_LOADING_MIST); } }
    public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_LOADING_MIST); } }
}
