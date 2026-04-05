using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using GOE;
using NPEnum;

public class NPGGUIMonoMainRoom : _ANPBasicUIWndResBarMono
{
    [ALHeader("聊天入口小窗")]
    public NPGGUIMonoMiniChat chatMiniWndMono;
    [ALHeader("下面的收纳栏")]
    public GGUIMonoCommonSideBar belowSideBarMono;

    [ALHeader("卧室皮肤背景(用于场景加载时显示, 防止黑屏跳帧)")]
    public RawImage roomSkinBg;
    [ALHeader("在场景加载时显示的对象列表")]
    public List<GameObject> onTDLoadingShow;
    [ALHeader("在场景加载完成后隐藏的对象列表")]
    public List<GameObject> onTDLoadingHide;
    
    [ALHeader("返回按钮")]
    public GameObject btnReturn;
    
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1431); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1431);} }
}
