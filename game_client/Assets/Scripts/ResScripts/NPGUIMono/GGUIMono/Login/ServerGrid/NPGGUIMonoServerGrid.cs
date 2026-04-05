using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using  ALPackage;

/// <summary>
/// 服务器列表grid
/// </summary>
public class NPGGUIMonoServerGrid :_TALUGUIMonoGridWnd<NPGGUIMonoServerGridItem>
{
    [ALHeader("空数据显示的go列表")]
    public List<GameObject> emptyShowGo;
}
