using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GGUIMonoMailItem : _TALUGUIMonoGridItem
{
    [ALHeader("加载出来的预制体父节点")]
    public Transform prefabParent;
    [ALHeader("等待数据的显示体")]
    public GameObject goWait;
}