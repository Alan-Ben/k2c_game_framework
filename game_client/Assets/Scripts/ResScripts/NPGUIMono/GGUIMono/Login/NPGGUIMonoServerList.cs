using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ALPackage;
using GOE;

/**************
 * 选择服务器
 **/
public class NPGGUIMonoServerList : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    [ALHeader("加载过程显示的go列表")] 
    public List<GameObject> loadingShowGoList;
    [ALHeader("加载过程隐藏的go列表")] 
    public List<GameObject> loadingHideGoList;
    
    [ALHeader("大区列表")]
    public NPGGUIMonoServerAreaContainer areaContainer;
    [ALHeader("服务器组列表")]
    public NPGGUIMonoServerGroupGrid serverGroupGrid;
    [ALHeader("服务器列表")]
    public NPGGUIMonoServerGrid serverGrid;
    [ALHeader("玩家自己已有服务器列表")]
    public NPGGUIMonoServerGrid serverSelfGrid;
    
    /************
    * 资源加载路径
    **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2302); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2302); } }
}
