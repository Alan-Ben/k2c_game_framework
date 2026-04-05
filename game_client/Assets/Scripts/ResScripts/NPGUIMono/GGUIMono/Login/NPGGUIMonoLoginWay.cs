using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;

/**********************
 * 登录方式选择界面的窗口脚本
 **/
public class NPGGUIMonoLoginWay : _AALBasicUIWndMono
{
    [ALHeader("选择登录方式的容器")]
    public NPGGUIMonoLoginWayContainer loginWayContainer;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("正在登录中需要展示的GO列表")]
    public List<GameObject> goLoginShowList;
    [ALHeader("正在登录中需要隐藏的GO列表")]
    public List<GameObject> goLoginHideList;


    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2304); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2304); } }
}
