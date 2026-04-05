using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

// 只有详情的物品信息对象
public class GGUIMonoBagPopItemDetailSimple : _AALBasicUIWndMono
{
    [ALHeader("名称")]
    public Text itemName;

    [ALHeader("描述")]
    public Text itemDesc;

    [ALHeader("获取途径入口")]
    public GameObject gainWayBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1906); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1906); } }
}
