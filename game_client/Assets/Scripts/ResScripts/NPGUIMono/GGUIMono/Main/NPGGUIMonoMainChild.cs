using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using GOE;
using NPEnum;

/// <summary>
/// 子嗣界面窗口
/// </summary>
public class NPGGUIMonoMainChild : _ANPBasicUIWndResBarMono
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1517); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1517);} }
}
