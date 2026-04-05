using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;
using System;
using UnityEngine.UI;

//签到弹出窗口
public class GGUIMonoDailyCheckMain : _AALBasicUIWndMono
{
    [ALHeader("加载父节点")]
    public Transform parPosTrans;

    [ALHeader("加载路径id")]
    public long resPathId;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    /************
* 资源加载路径
*/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3502); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3502); } }
}
