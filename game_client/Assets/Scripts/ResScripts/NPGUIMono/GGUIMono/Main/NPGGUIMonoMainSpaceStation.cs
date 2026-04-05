using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using GOE;
using NPEnum;

/// <summary>
/// 空间站窗口
/// </summary>
public class NPGGUIMonoMainSpaceStation : _ANPBasicUIWndResBarMono
{
    [ALHeader("聊天入口小窗")]
    public NPGGUIMonoMiniChat chatMiniWndMono;
    
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1514); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1514);} }
}
