using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

// 简易漫画窗口
public class GGUIMonoSimpleComicSubWnd : _AALBasicUIWndMono
{
    [ALHeader("跳过按钮")]
    public GameObject btnLast;
    [ALHeader("下一步按钮")]
    public GameObject btnNext;
    
    [ALHeader("完成按钮")]
    public GameObject btnDone;
    [ALHeader("最后一页时候显示的go列表")]
    public List<GameObject> doneShowGoList;
    [ALHeader("最后一页时候隐藏的go列表")]
    public List<GameObject> doneHideGoList;
    
    [ALHeader("每一个页面prefab的挂载父节点")]
    public Transform transformParent;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(22401); } }
    public static string objName { get { return UIResPathAssistant.getObjName(22401); } }
}
